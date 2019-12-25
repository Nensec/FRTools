using Discord;
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
                    ITextChannel annChannel = null;
                    var annChannelId = settingManager.GetSettingValue("GUILDCONFIG_ANN_CHANNEL", guild);
                    if (annChannelId != null)
                        annChannel = (ITextChannel)guild.GetChannel(ulong.Parse(annChannelId));

                    foreach (var user in dominanceRole.Members.ToList())
                        await user.RemoveRoleAsync(dominanceRole);

                    using (var ctx = new DataContext())
                    {
                        var latestDominance = ctx.FRDominances.OrderByDescending(x => x.Timestamp).First();
                        if ((Flight)latestDominance.First != Flight.Beastclans)
                        {
                            if (ulong.TryParse(settingManager.GetSettingValue($"GUILDCONFIG_DOMINANCE_ROLE_{latestDominance.First}", guild), out var firstPlaceFlight))
                            {
                                var firstPlaceRole = guild.GetRole(firstPlaceFlight);
                                foreach (var user in firstPlaceRole.Members.ToList())
                                {
                                    if (settingManager.GetSettingValue($"GUILDCONFIG_DOMINANCE_USER_{user.Id}", guild) == "true")
                                        await user.AddRoleAsync(dominanceRole);
                                }
                                if (annChannel != null)
                                    await annChannel.SendMessageAsync($"Congratulations to the {(Flight)latestDominance.First} flight for gaining dominance this week! Members who opted for the {dominanceRole.Mention} role have been given the role, go activate that dom-shop!\r\n\nSecond place is {(Flight)latestDominance.Second}\r\nThird place is {(Flight)latestDominance.Third}");
                            }
                        }
                        else
                            await annChannel.SendMessageAsync($"Beastclan won 1st place on dominance this week, somehow, nobody therefor has the dominance role : (\r\n\nSecond place is {(Flight)latestDominance.Second}\r\nThird place is {(Flight)latestDominance.Third}");
                    }
                }
            }

        }
    }
}
