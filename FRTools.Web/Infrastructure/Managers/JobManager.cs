using FRTools.Data;
using FRTools.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FRTools.Web.Infrastructure.Managers
{
    public static class JobManager
    {
        private static Dictionary<string, List<Job>> _activeJobs { get; set; } = new Dictionary<string, List<Job>>();

        public static (Guid JobId, DateTime StartTime) StartNewJob(Task task, string relatedEntityId, string description)
        {
            var job = new Job { Id = Guid.NewGuid(), StartTime = DateTime.UtcNow, RelatedEntity = relatedEntityId, Description = description, Status = JobStatus.Running };
            using (var ctx = new DataContext())
            {
                ctx.Jobs.Add(job);
                ctx.SaveChanges();
                ctx.Entry(job).State = EntityState.Detached;
            }

            if (!_activeJobs.TryGetValue(relatedEntityId, out var jobList))
                _activeJobs.Add(relatedEntityId, new List<Job>());

            _activeJobs[relatedEntityId].Add(job);

            _ = Task.Run(async () =>
              {
                  using (var ctx = new DataContext())
                  {
                      ctx.Jobs.Attach(job);
                      while (!task.IsCompleted)
                      {

                          job.Heartbeat = DateTime.UtcNow;
                          await ctx.SaveChangesAsync();

                          await Task.Delay(1000);
                      }
                      _activeJobs[relatedEntityId].Remove(job);
                      job.Status = task.IsFaulted ? JobStatus.Error : JobStatus.Finished;
                      await ctx.SaveChangesAsync();
                      if (!_activeJobs[relatedEntityId].Any())
                          _activeJobs.Remove(relatedEntityId);
                  }
              });

            return (job.Id, job.StartTime);
        }

        public static List<Job> GetActiveJobs(string relatedEntityId) => _activeJobs.ContainsKey(relatedEntityId) ? _activeJobs[relatedEntityId] : new List<Job>();
    }
}