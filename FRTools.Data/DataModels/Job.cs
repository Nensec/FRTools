using System;

namespace FRTools.Data.DataModels
{
    public class Job
    {
        public Guid Id { get; set; }
        public string RelatedEntity { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? Heartbeat { get; set; }
        public JobStatus Status { get; set; }

        public string Errors { get; set; }
    }
}
