using System.Net.Http;
using System.Threading.Tasks;
using FRTools.Core.Common;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class GeneralFunctions
    {
        [FunctionName(nameof(KeepSiteHot))]
        public async Task KeepSiteHot([TimerTrigger("0 */4 * * * *")] TimerInfo timer, ILogger log)
        {
            using var client = new HttpClient();
            await client.GetByteArrayAsync(Helpers.GetProxyIconUrl(25174));
        }
    }
}
