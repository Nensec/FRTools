using System.Net.Http;
using System.Threading.Tasks;
using FRTools.Core.Common;
using FRTools.Core.Common.Extentions;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.DiscordModels;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class GeneralFunctions
    {
        private readonly ILogger<GeneralFunctions> _logger;

        public GeneralFunctions(ILogger<GeneralFunctions> logger)
        {
            _logger = logger;
        }

        [Function(nameof(KeepSiteHot))]
        public async Task KeepSiteHot([TimerTrigger("0 */4 * * * *")] TimerInfo timer)
        {
            try
            {
                // This will return an 401 since we aren't supplying the correct headers but we really don't care
                using (var client = new HttpClient())
                    await client.PostAsJsonAsync(Helpers.GetDiscordInteractionUrl(), new DiscordInteractionRequest { Type = InteractionType.PING });
            }
            catch { }
        }
    }
}
