using FRTools.Common.Jobs;
using FRTools.Data;
using FRTools.Data.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FRTools.Common
{
    public static class JobManager
    {
        private static Dictionary<string, List<Job>> _activeJobs { get; set; } = new Dictionary<string, List<Job>>();

        public static (Guid JobId, DateTime StartTime, Task Task) StartNewJob(BaseJob job)
        {
            var dbJob = new Job { Id = Guid.NewGuid(), StartTime = DateTime.UtcNow, RelatedEntity = job.RelatedEntityId, Description = job.Description, Status = JobStatus.Running };
            using (var ctx = new DataContext())
            {
                ctx.Jobs.Add(dbJob);
                ctx.SaveChanges();
                ctx.Entry(dbJob).State = EntityState.Detached;
            }

            if (!_activeJobs.TryGetValue(dbJob.RelatedEntity, out var jobList))
                _activeJobs.Add(dbJob.RelatedEntity, new List<Job>());

            _activeJobs[dbJob.RelatedEntity].Add(dbJob);

            var task = job.JobTask();
            bool taskSetup = false;
            _ = Task.Run(async () =>
              {
                  using (var ctx = new DataContext())
                  {
                      ctx.Jobs.Attach(dbJob);
                      job.ReportError = async x => await SaveError(x, dbJob, ctx);
                      taskSetup = true;

                      while (!task.IsCompleted)
                      {

                          dbJob.Heartbeat = DateTime.UtcNow;
                          await ctx.SaveChangesAsync();

                          await Task.Delay(1000);
                      }
                      if (task.IsFaulted)
                      {
                          await SaveError(task.Exception.GetBaseException().Message, dbJob, ctx);
                          dbJob.Status = JobStatus.Error;
                      }
                      else if (dbJob.Errors != null)
                          dbJob.Status = JobStatus.FinishedWithErrors;
                      else
                          dbJob.Status = JobStatus.Finished;

                      await ctx.SaveChangesAsync();
                      _activeJobs[dbJob.RelatedEntity].Remove(dbJob);
                      if (!_activeJobs[dbJob.RelatedEntity].Any())
                          _activeJobs.Remove(dbJob.RelatedEntity);
                  }
              });

            while (!taskSetup)
                Task.Delay(10).GetAwaiter().GetResult();

            return (dbJob.Id, dbJob.StartTime, task);
        }

        public static List<Job> GetActiveJobs(string relatedEntityId) => _activeJobs.ContainsKey(relatedEntityId) ? _activeJobs[relatedEntityId] : new List<Job>();
        public static List<Job> GetUnconfirmedFinishedJobs(string relatedEntityId)
        {
            using (var ctx = new DataContext())
                return ctx.Jobs.Where(x => x.RelatedEntity == relatedEntityId && (x.Status == JobStatus.Finished || x.Status == JobStatus.FinishedWithErrors || x.Status == JobStatus.Error)).ToList();
        }

        private static async Task SaveError(string error, Job dbJob, DataContext ctx)
        {
            var errors = dbJob.Errors != null ? JsonConvert.DeserializeObject<List<string>>(dbJob.Errors) : new List<string>();
            errors.Add(error);
            dbJob.Errors = JsonConvert.SerializeObject(errors);

            await ctx.SaveChangesAsync();
        }

        public static void MarkFinishedJobRead(Guid jobId)
        {
            using (var ctx = new DataContext())
            {
                var job = ctx.Jobs.Find(jobId);
                if (job != null && (job.Status == JobStatus.FinishedWithErrors || job.Status == JobStatus.Finished || job.Status == JobStatus.Error))
                {
                    job.Status = JobStatus.UserConfirmedDone;
                    ctx.SaveChanges();
                }
            }
        }
    }
}