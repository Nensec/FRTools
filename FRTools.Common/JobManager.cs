using FRTools.Common.Jobs;
using FRTools.Data;
using FRTools.Data.DataModels;
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

        public static (Guid JobId, DateTime StartTime) StartNewJob(IJob job)
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

            _ = Task.Run(async () =>
              {
                  using (var ctx = new DataContext())
                  {
                      ctx.Jobs.Attach(dbJob);
                      while (!task.IsCompleted)
                      {

                          dbJob.Heartbeat = DateTime.UtcNow;
                          await ctx.SaveChangesAsync();

                          await Task.Delay(1000);
                      }
                      _activeJobs[dbJob.RelatedEntity].Remove(dbJob);
                      dbJob.Status = task.IsFaulted ? JobStatus.Error : JobStatus.Finished;
                      await ctx.SaveChangesAsync();
                      if (!_activeJobs[dbJob.RelatedEntity].Any())
                          _activeJobs.Remove(dbJob.RelatedEntity);
                  }
              });

            return (dbJob.Id, dbJob.StartTime);
        }

        public static List<Job> GetActiveJobs(string relatedEntityId) => _activeJobs.ContainsKey(relatedEntityId) ? _activeJobs[relatedEntityId] : new List<Job>();
    }
}