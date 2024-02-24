using System.Text;
using FRTools.Core.Services.Interfaces;

namespace FRTools.Core.Services
{
    public class AzurePipelineService : IAzurePipelineService
    {
        public async Task TriggerPipeline(string azureDevOpsPipeline)
        {
            var azureDevOpsAccount = Environment.GetEnvironmentVariable("AzureDevOpsAccount");
            var azureDevOpsProject = Environment.GetEnvironmentVariable("AzureDevOpsProject");
            var azureDevOpsBasicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{azureDevOpsAccount}:{Environment.GetEnvironmentVariable("AzureDevOpsPipelinesPAT")}"));

            if (!string.IsNullOrEmpty(azureDevOpsBasicAuth) && !string.IsNullOrEmpty(azureDevOpsAccount) && !string.IsNullOrEmpty(azureDevOpsProject) && !string.IsNullOrEmpty(azureDevOpsPipeline))
            {
                using (var client = new HttpClient())
                {
                    var body = $"{{\"definition\":{{\"id\":{azureDevOpsPipeline}}}}}";
                    var url = $"https://dev.azure.com/{azureDevOpsAccount}/{azureDevOpsProject}/_apis/build/builds?api-version=7.0";
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {azureDevOpsBasicAuth}");
                    await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                }
            }
        }
    }
}
