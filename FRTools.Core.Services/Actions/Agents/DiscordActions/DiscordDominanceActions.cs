using FRTools.Core.Data;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Actions.Agents.DiscordActions
{
    public interface IDiscordDominanceActions
    {
        Task AssignDominanceRole(DominanceAgentData data, ulong guildId);
        Task PerformDominanceTasks(DominanceAgentData dominanceAgentData);
    }

    public class DiscordDominanceActions : IDiscordDominanceActions
    {
        private readonly IConfigService _configService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly ILogger<DiscordDominanceActions> _logger;

        public DiscordDominanceActions(IConfigService configService, IDiscordGuildService discordGuildService, ILogger<DiscordDominanceActions> logger)
        {
            _configService = configService;
            _discordGuildService = discordGuildService;
            _logger = logger;
        }

        public async Task PerformDominanceTasks(DominanceAgentData data)
        {
            var allDominanceEnabledGuilds = (await _configService.GetAllConfig("GUILDCONFIG_DOMINANCE")).Where(x => x.Value == $"{true}");

            foreach (var dominanceEnabledGuild in allDominanceEnabledGuilds)
            {
                try
                {
                    await AssignDominanceRole(data, dominanceEnabledGuild.GuildId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing guild {dominanceEnabledGuild.GuildId}");
                }
            }
        }

        public async Task AssignDominanceRole(DominanceAgentData data, ulong guildId)
        {
            // Check if all config is present
            var domRole = await _configService.GetConfigValue("GUILDCONFIG_DOMINANCE_ROLE", guildId);
            if (domRole == null)
                return;

            var flights = Enum.GetValues<Flight>().Except([Flight.Beastclans]);
            var guildFlightRoles = new Dictionary<Flight, ulong>();
            foreach (var flight in flights)
            {
                var flightRole = await _configService.GetConfigValue($"GUILDCONFIG_{flight.ToString().ToUpper()}_ROLE", guildId);
                if (flightRole == null)
                    break;

                guildFlightRoles.Add(flight, ulong.Parse(flightRole));
            }

            if (guildFlightRoles.Count != flights.Count())
                return;

            var winningFlight = guildFlightRoles[data.Flights[0]];

            // Get al the users who opted in
            var userIds = ((await _configService.GetConfigValue($"GUILDCONFIG_DOMINANCE_OPTIN", guildId))?.Split(';') ?? []).Select(x => ulong.Parse(x));

            foreach (var userId in userIds)
            {
                try
                {
                    var guildMember = await _discordGuildService.GetGuildMember(guildId, userId);
                    if (guildMember == null)
                        continue;

                    if (guildMember.Roles.Intersect(guildFlightRoles.Values).Any())
                    {
                        // Taketh Dominance role
                        guildMember.Roles = [.. guildMember.Roles.Except([ulong.Parse(domRole)])];
                    }

                    if (guildMember.Roles.Any(x => x == winningFlight))
                    {
                        // Giveth Dominance role
                        guildMember.Roles = [.. guildMember.Roles, ulong.Parse(domRole)];
                    }

                    await _discordGuildService.ModifyGuildMember(guildId, guildMember);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating user {userId}, dom role id: {domRole}");
                }
            }
        }
    }
}
