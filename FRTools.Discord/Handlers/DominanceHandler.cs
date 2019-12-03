using Discord.WebSocket;
using FRTools.Data;
using FRTools.Discord.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FRTools.Discord.Handlers
{
    public static class DominanceHandler
    {
        public static async Task UpdateGuild(SettingManager settingManager, SocketGuild guild)
        {
            if (bool.TryParse(settingManager.GetSettingValue("GUILDCONFIG_DOMINANCE", guild), out var result) && result)
            {
                if (ulong.TryParse(settingManager.GetSettingValue("GUILDCONFIG_DOMINANCE_ROLE", guild), out var roleId))
                {
                    var dominanceRole = guild.GetRole(roleId);
                    foreach (var user in dominanceRole.Members.ToList())
                        await user.RemoveRoleAsync(dominanceRole);
                    using (var ctx = new DataContext())
                    {
                        var latestDominance = ctx.FRDominances.OrderByDescending(x => x.Timestamp).First();
                        if ((Flight)latestDominance.First != Flight.Beastclans)
                        {
                            if (ulong.TryParse(settingManager.GetSettingValue($"GUILDCONFIG_DOMINANCE_ROLE_{latestDominance.First}", guild), out var flightRoleId))
                            {
                                var flightRole = guild.GetRole(flightRoleId);
                                foreach (var user in flightRole.Members.ToList())
                                {
                                    if(settingManager.GetSettingValue($"GUILDCONFIG_DOMINANCE_USER_{user.Id}", guild) == "true")
                                        await user.AddRoleAsync(dominanceRole);
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
