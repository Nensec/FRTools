using System;
using System.Threading.Tasks;

namespace FRTools.Common.Jobs
{
    public abstract class BaseJob
    {
        public abstract Task JobTask();
        public abstract string RelatedEntityId { get; set; }
        public abstract string Description { get; set; }

        public Action<string> ReportError { get; set; }
    }
}
