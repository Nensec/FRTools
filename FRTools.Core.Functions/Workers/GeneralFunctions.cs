using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class GeneralFunctions
    {
        [FunctionName(nameof(KeepSiteHot))]
        public Task KeepSiteHot([TimerTrigger("0 */4 * * * *")] TimerInfo timer, ILogger log) => Task.CompletedTask;
    }
}
