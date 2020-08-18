using System.Threading.Tasks;

namespace FRTools.Common.Jobs
{
    public interface IJob
    {
        Task JobTask();
        string RelatedEntityId { get; set; }
        string Description { get; set; }
    }
}
