namespace FRTools.Core.Services.Interfaces
{
    public interface IAzurePipelineService
    {
        Task TriggerPipeline(string azureDevOpsPipeline);
    }
}