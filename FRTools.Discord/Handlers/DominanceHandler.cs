using Discord;
using Discord.WebSocket;
using FRTools.Common;
using FRTools.Data;
using FRTools.Discord.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FRTools.Discord.Handlers
{
    public static class DominanceHandler
    {
        public static async Task UpdateGuild(SettingManager settingManager, SocketGuild guild, SocketGuildUser selfUser)
        {
            if (bool.TryParse(settingManager.GetSettingValue("GUILDCONFIG_DOMINANCE", guild), out var result) && result)
            {
                if (ulong.TryParse(settingManager.GetSettingValue("GUILDCONFIG_DOMINANCE_ROLE", guild), out var roleId))
                {
                    var dominanceRole = guild.GetRole(roleId);
                    ITextChannel annChannel = null;
                    var annChannelId = settingManager.GetSettingValue("GUILDCONFIG_ANN_CHANNEL", guild);
                    if (annChannelId != null)
                        annChannel = guild.GetChannel(ulong.Parse(annChannelId)) as ITextChannel;
                    if (selfUser.Roles.Any(x => x.Position > dominanceRole.Position))
                    {
                        foreach (var user in dominanceRole.Members.ToList())
                            await user.RemoveRoleAsync(dominanceRole);

                        using (var ctx = new DataContext())
                        {
                            var externalEmojis = guild.CurrentUser.GuildPermissions.UseExternalEmojis;

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
                                    {
                                        var embed = new EmbedBuilder()
                                            .WithTitle($"Congratulations to the {(externalEmojis ? (DiscordHelpers.DiscordEmojis[(DiscordEmoji)latestDominance.First] + " ") : "")}**{(Flight)latestDominance.First}** flight for claiming 1st place!")
                                            .WithFields(
                                                new EmbedFieldBuilder().WithIsInline(true).WithName("Info").WithValue($"Those who opted in for the {dominanceRole.Mention} role now have it, enjoy your discounts!\r\n\nSecond place: {(externalEmojis ? (DiscordHelpers.DiscordEmojis[(DiscordEmoji)latestDominance.Second] + " ") : "")}{(Flight)latestDominance.Second}\r\nThird place: {(externalEmojis ? (DiscordHelpers.DiscordEmojis[(DiscordEmoji)latestDominance.Third] + " ") : "")}{(Flight)latestDominance.Third}"),
                                                new EmbedFieldBuilder().WithIsInline(true).WithName("Benefits").WithValue($"{(externalEmojis ? (DiscordHelpers.DiscordEmojis[DiscordEmoji.Dominance] + " ") : "")}15% off marketplace treasure items\r\n{(externalEmojis ? (DiscordHelpers.DiscordEmojis[DiscordEmoji.Lair] + " ") : "")}5% off lair expansions\r\n{(externalEmojis ? (DiscordHelpers.DiscordEmojis[DiscordEmoji.Treasure] + " ") : "")}+1500 treasure a day\r\n{(externalEmojis ? (DiscordHelpers.DiscordEmojis[DiscordEmoji.Food] + " ") : "")}+3 gathering turns a day")
                                                );
                                        await annChannel.SendMessageAsync(embed: embed.Build());
                                    }
                                }
                            }
                            else
                            {
                                if (annChannel != null)
                                {
                                    var embed = new EmbedBuilder()
                                    .WithTitle($"{(externalEmojis ? (DiscordHelpers.DiscordEmojis[DiscordEmoji.Beastclans] + " ") : "")}Beastclans won 1st place this week, somehow, so nobody gets the dominance role : (")
                                    .WithFields(
                                        new EmbedFieldBuilder().WithIsInline(true).WithName("Info").WithValue($"How did {(externalEmojis ? (DiscordHelpers.DiscordEmojis[DiscordEmoji.Earth] + " ") : "")}Earth not get 1st place? Strange..\r\n\nSecond place: {(externalEmojis ? (DiscordHelpers.DiscordEmojis[(DiscordEmoji)latestDominance.Second] + " ") : "")}{(Flight)latestDominance.Second}\r\nThird place: {(externalEmojis ? (DiscordHelpers.DiscordEmojis[(DiscordEmoji)latestDominance.Third] + " ") : "")}{(Flight)latestDominance.Third}"),
                                        new EmbedFieldBuilder().WithIsInline(true).WithName("Benefits (for 2nd place)").WithValue($"{(externalEmojis ? (DiscordHelpers.DiscordEmojis[DiscordEmoji.Dominance] + " ") : "")}7% off marketplace treasure items\r\n{(externalEmojis ? (DiscordHelpers.DiscordEmojis[DiscordEmoji.Lair] + " ") : "")}1% off lair expansions\r\n{(externalEmojis ? (DiscordHelpers.DiscordEmojis[DiscordEmoji.Treasure] + " ") : "")}+750 treasure a day\r\n{(externalEmojis ? (DiscordHelpers.DiscordEmojis[DiscordEmoji.Food] + " ") : "")}+2 gathering turns a day")
                                        );
                                    await annChannel.SendMessageAsync(embed: embed.Build());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (annChannel != null)
                            await annChannel.SendMessageAsync("Dominance has updated, but I does not have permission to grant dominance role on this server");
                    }
                }
            }

        }
    }
}
