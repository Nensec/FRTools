using FRTools.Core.Data;
using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Announce.Announcers.DiscordAnnouncers
{
    public interface IDiscordDominanceAnnouncer
    {
        Task AnnounceDominance(DominanceAnnounceData dominanceAnnounceData);
    }

    public class DiscordDominanceAnnouncer : BaseDiscordAnnouncer, IDiscordDominanceAnnouncer
    {
        public DiscordDominanceAnnouncer(IConfigService configService, IDiscordInteractionService discordInteractionService, ILogger<DiscordDominanceAnnouncer> logger) : base(configService, discordInteractionService, logger)
        {
        }

        public async Task AnnounceDominance(DominanceAnnounceData data)
        {
            var allDominanceWebhooks = (await ConfigService.GetAllConfig("GUILDCONFIG_DOMINANCEWEBHOOK")).Concat(await ConfigService.GetAllConfig("GUILDCONFIG_DEFAULTWEBHOOK")).GroupBy(x => x.GuildId);

            foreach (var guildWebhooks in allDominanceWebhooks)
            {
                var domRole = await ConfigService.GetConfigValue("GUILDCONFIG_DOMINANCE_ROLE", guildWebhooks.Key);

                var flights = Enum.GetValues<Flight>().Except([Flight.Beastclans]);
                var guildFlightRoles = new Dictionary<Flight, ulong>();
                foreach (var flight in flights)
                {
                    var flightRole = await ConfigService.GetConfigValue($"GUILDCONFIG_{flight.ToString().ToUpper()}_ROLE", guildWebhooks.Key);
                    if (flightRole == null)
                        break;

                    guildFlightRoles.Add(flight, ulong.Parse(flightRole));
                }

                var firstPlace = $"{data.Flights[0]}";
                var secondPlace = $"{data.Flights[1]}";
                var thirdPlace = $"{data.Flights[2]}";

                if (domRole != null && guildFlightRoles.Count == flights.Count())
                {
                    firstPlace = $"<@&{guildFlightRoles[data.Flights[0]]}>";
                    secondPlace = $"<@&{guildFlightRoles[data.Flights[1]]}>";
                    thirdPlace = $"<@&{guildFlightRoles[data.Flights[2]]}>";
                }

                var webhook = new DiscordWebhookRequest();
                var embed = new DiscordEmbed
                {
                    Title = $"Congratulations to the **{data.Flights[0]}** flight for claiming 1st place!",
                    Fields = new List<DiscordEmbedField>
                    {
                        new() { Name = "Info", Value = $"Those who are part of the {firstPlace} flight, enjoy your discounts!{(domRole == null ? "" : $" Those who have opted in will receive the <@&{domRole}> role.")}\r\n\nSecond place: {secondPlace}\r\nThird place: {thirdPlace}"},
                        new() { Name = "Benefits", Value = $"15% off marketplace treasure items\r\n5% off lair expansions\r\n+1500 treasure a day\r\n+3 gathering turns a day"}
                    }
                };
                webhook.Embeds = new List<DiscordEmbed> { embed };

                await AttemptPostToWebhook(guildWebhooks, webhook);
            }
        }
    }
}
