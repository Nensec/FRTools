namespace FRTools.Core.Services
{
    public interface IAzurePipelineService
    {
        Task TriggerPipeline(string azureDevOpsPipeline);
    }
}