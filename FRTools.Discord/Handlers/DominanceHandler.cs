using Discord;
using Discord.WebSocket;
using FRTools.Data;
using FRTools.Discord.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRTools.Discord.Handlers
{
    public static class DominanceHandler
    {
        public static async Task UpdateGuild(SettingManager settingManager, SocketGuild guild)
        {
            var flightEmojis = new Dictionary<Flight, string>
            {
                { Flight.Earth, "<:earthflight:703609147987329044>" },
                { Flight.Plague, "<:plagueflight:703609147907637258>" },
                { Flight.Wind, "<:windflight:703609147815362621>" },
                { Flight.Water, "<:waterflight:703609147958100039>" },
                { Flight.Lightning, "<:lightningflight:703609147899248681>" },
                { Flight.Ice, "<:iceflight:703609148142649504>" },
                { Flight.Shadow, "<:shadowflight:703609148130066562>" },
                { Flight.Light, "<:lightflight:703609148054700113>" },
                { Flight.Arcane, "<:arcaneflight:703609147962425424>" },
                { Flight.Nature, "<:natureflight:703609148109226014>" },
                { Flight.Fire, "<:fireflight:703609147718893609>" },
                { Flight.Beastclans, "<:beastclans:703609148058894347>" }
            };

            var domEmojis = new Dictionary<string, string>
            {
                { "dom", "<:discount:703609171676889169>" },
                { "lair", "<:lairspace:703609171844530216>" },
                { "treasure", "<:treasure:703609171492339795>" },
                { "food", "<:food:703609171865763941>" }
            };

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
                                        .WithTitle($"Congratulations to the {(externalEmojis ? (flightEmojis[(Flight)latestDominance.First] + " ") : "")}**{(Flight)latestDominance.First}** flight for claiming 1st place!")
                                        .WithFields(
                                            new EmbedFieldBuilder().WithIsInline(true).WithName("Info").WithValue($"Those who opted in for the {dominanceRole.Mention} role now have it, enjoy your discounts!\r\n\nSecond place: {(externalEmojis ? (flightEmojis[(Flight)latestDominance.Second] + " ") : "")}{(Flight)latestDominance.Second}\r\nThird place: {(externalEmojis ? (flightEmojis[(Flight)latestDominance.Third] + " ") : "")}{(Flight)latestDominance.Third}"),
                                            new EmbedFieldBuilder().WithIsInline(true).WithName("Benefits").WithValue($"{(externalEmojis ? (domEmojis["dom"] + " ") : "")}15% off marketplace treasure items\r\n{(externalEmojis ? (domEmojis["lair"] + " ") : "")}5% off lair expansions\r\n{(externalEmojis ? (domEmojis["treasure"] + " ") : "")}+1500 treasure a day\r\n{(externalEmojis ? (domEmojis["food"] + " ") : "")}+3 gathering turns a day")
                                            );
                                    await annChannel.SendMessageAsync(embed: embed.Build());
                                }
                            }
                        }
                        else
                        {
                            var embed = new EmbedBuilder()
                                .WithTitle($"{(externalEmojis ? (flightEmojis[Flight.Beastclans] + " ") : "")}Beastclans won 1st place this week, somehow, so nobody gets the dominance role : (")
                                .WithFields(
                                    new EmbedFieldBuilder().WithIsInline(true).WithName("Info").WithValue($"How did {(externalEmojis ? (flightEmojis[Flight.Earth] + " ") : "")}Earth not get 1st place? Strange..\r\n\nSecond place: {(externalEmojis ? (flightEmojis[(Flight)latestDominance.Second] + " ") : "")}{(Flight)latestDominance.Second}\r\nThird place: {(externalEmojis ? (flightEmojis[(Flight)latestDominance.Third] + " ") : "")}{(Flight)latestDominance.Third}"),
                                    new EmbedFieldBuilder().WithIsInline(true).WithName("Benefits (for 2nd place)").WithValue($"{(externalEmojis ? (domEmojis["dom"] + " ") : "")}7% off marketplace treasure items\r\n{(externalEmojis ? (domEmojis["lair"] + " ") : "")}1% off lair expansions\r\n{(externalEmojis ? (domEmojis["treasure"] + " ") : "")}+750 treasure a day\r\n{(externalEmojis ? (domEmojis["food"] + " ") : "")}+2 gathering turns a day")
                                    );
                            await annChannel.SendMessageAsync(embed: embed.Build());
                        }
                    }
                }
            }

        }
    }
}
