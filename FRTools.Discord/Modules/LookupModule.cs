using Discord;
using Discord.Commands;
using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Discord.Infrastructure;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.Discord.Modules
{
    [Name("Lookup"), Group("lookup"), Alias("lu"), Summary("Lookup related commands")]
    [DiscordHelp("LookupModule")]
    public class LookupModule : BaseModule
    {
        public LookupModule(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
        {
        }

        [Command("dragon"), Name("Dragon"), Alias("d")]
        [DiscordHelp("LookupDragon")]
        public async Task DragonLookup(int id)
        {
            var lookingUpMessage = await ReplyAsync($"Looking up dragon {id}, gimme a moment..");

            var client = new HtmlWeb();
            var dragonProfileDoc = client.Load(string.Format(FRHelpers.DragonProfileUrl, id));

            if (dragonProfileDoc.GetElementbyId("error-404") != null)
            {
                await lookingUpMessage.ModifyAsync(x => x.Content = "This dragon does not exist");
                return;
            }

            var isExalted = dragonProfileDoc.GetElementbyId("exalted-content") != null;

            var embed = new EmbedBuilder()
                .WithUrl(string.Format(FRHelpers.DragonProfileUrl, id))
                .WithThumbnailUrl($"attachment://{id}_350.png")
                .WithFooter(x => x.Text = "Click the image to view a larger version");

            if (isExalted)
            {
                var name = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""content""]/div/div[2]/div/div/div[2]/span").InnerHtml;
                var exaltedToFlight = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""exalted-content""]/div/div[2]/div/p[1]/strong[2]").InnerHtml;

                embed.Title = name;
                embed.Description = $"**{name}** has been exalted to the {FRHelpers.GetFlightFromGodName(exaltedToFlight)} flight.\nThe only info available is lineage.";

                var parentsField = new EmbedFieldBuilder().WithName("Parents").WithIsInline(true);
                var siblingsField = new EmbedFieldBuilder().WithName("Offspring").WithIsInline(true);

                var parentsNodes = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""exalted-content""]/div/div[2]/div/fieldset/div/ul[1]").SelectNodes("li");
                var siblingsNodes = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""exalted-content""]/div/div[2]/div/fieldset/div/ul[2]").SelectNodes("li");

                if (parentsNodes[0].FirstChild.Name == "em")
                    parentsField.Value = "_none_";
                else
                    parentsField.Value = string.Join("\n", parentsNodes.Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})"));

                if (siblingsNodes[0].FirstChild.Name == "em")
                    siblingsField.Value = "_none_";
                else
                    if (siblingsNodes.Count <= 5)
                    siblingsField.Value = string.Join("\n", siblingsNodes.Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})"));
                else
                    siblingsField.Value = string.Join("\n", siblingsNodes.Take(5).Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})")) + $"\n_{siblingsNodes.Count - 5} more.._";

                embed.AddField(parentsField);
                embed.AddField(siblingsField);
            }
            else
            {
                var name = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-header""]/div[1]/span[1]").InnerHtml;
                var ownerNode = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""content""]/div/div[2]/div/div/div[2]/a[1]");
                var level = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-battle""]/div[1]/strong").InnerText.Replace("&infin;", "∞");

                var eggbreakday = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[2]/div[1]/div/div/div[2]/strong").InnerText;
                var dragonAge = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[2]/div[2]/div/div/div[2]").ChildNodes[0].InnerText.Replace("\\n", "").Trim().ToLower();
                var dragonBreed = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[2]/div[2]/div/div/div[2]/strong").InnerText;

                var badges = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-secondary""]/div[2]/div[3]").ChildNodes.Nodes().Where(x => x.Name == "img");
                var gender = badges.First(x => x.ParentNode.Attributes.FirstOrDefault(a => a.Name == "data-tooltip-source")?.Value == "#dragon-profile-icon-sex-tooltip").Attributes["src"].Value.EndsWith("female.png") ? "female" : "male";
                var permaBaby = badges.Any(x => x.Attributes["src"].Value.EndsWith("eternal-youth.png"));

                embed.Title = name;
                embed.Description = $"**{name}** is a level {level}, {(permaBaby ? "**eternal youth**" : dragonAge)} {gender} {dragonBreed} in [{ownerNode.InnerHtml}]({ownerNode.Attributes["href"].Value})'{(ownerNode.InnerHtml.EndsWith("s") ? "" : "s")} lair.";

                var eyeColor = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[2]/div[3]/div/div/div[2]").ChildNodes[0].InnerText.Replace("\\n", "").Trim();
                var eyeType = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[2]/div[3]/div/div/div[2]/strong").InnerText;

                embed.AddField(x => x.WithIsInline(true).WithName("General info").WithValue($"**Eye type:** {eyeColor} {eyeType}\n**Eggday:** {eggbreakday}\n**ID:** {id}"));

                var geneticsNode = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[1]/div[2]");
                var primaryGene = geneticsNode.ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerHtml;
                var primaryColor = geneticsNode.ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[0].InnerHtml.Replace("\\n", "").Trim();
                var secondaryGene = geneticsNode.ChildNodes[3].ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerHtml;
                var secondaryColor = geneticsNode.ChildNodes[3].ChildNodes[1].ChildNodes[3].ChildNodes[0].InnerHtml.Replace("\\n", "").Trim();
                var tertiaryGene = geneticsNode.ChildNodes[5].ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerHtml;
                var tertiaryColor = geneticsNode.ChildNodes[5].ChildNodes[1].ChildNodes[3].ChildNodes[0].InnerHtml.Replace("\\n", "").Trim();

                embed.AddField(x => x.WithIsInline(true).WithName("\u200B").WithValue("\u200B"));
                embed.AddField(x => x.WithIsInline(true).WithName("Genetics").WithValue($"**Primary:** {primaryColor} {primaryGene}\n**Secondary:** {secondaryColor} {secondaryGene}\n**Tertiary:** {tertiaryColor} {tertiaryGene}\n"));

                var parentsField = new EmbedFieldBuilder().WithName("Parents").WithIsInline(true);
                var siblingsField = new EmbedFieldBuilder().WithName("Offspring").WithIsInline(true);

                if (dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-details-lineage""]/div/div") != null)
                {
                    embed.AddField(x => x.WithIsInline(true).WithName("God").WithValue("This is one of the **eleven elemental deities**; it has no parents or offspring."));
                }
                else
                {
                    var parentsNodes = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-details-lineage""]/div/ul[1]").SelectNodes("li");
                    var siblingsNodes = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-details-lineage""]/div/ul[2]").SelectNodes("li");

                    if (parentsNodes[0].FirstChild.Name == "em")
                        parentsField.Value = "_none_";
                    else
                        parentsField.Value = string.Join("\n", parentsNodes.Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})"));

                    if (siblingsNodes[0].FirstChild.Name == "em")
                        siblingsField.Value = "_none_";
                    else
                    {
                        if (siblingsNodes.Count <= 5)
                            siblingsField.Value = string.Join("\n", siblingsNodes.Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})"));
                        else
                            siblingsField.Value = string.Join("\n", siblingsNodes.Take(5).Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})")) + $"\n_{siblingsNodes.Count - 5} more.._";
                    }

                    embed.AddField(parentsField);
                    embed.AddField(x => x.WithIsInline(true).WithName("\u200B").WithValue("\u200B"));
                    embed.AddField(siblingsField);
                }
            }

            using (var webClient = new WebClient())
            {
                var dragonImage = webClient.OpenRead(FRHelpers.GetRenderUrl(id));
                await Context.Channel.SendFileAsync(dragonImage, $"{id}_350.png", embed: embed.Build());
            }
            await lookingUpMessage.DeleteAsync();
        }

        [Group("item"), Name("ItemInfo"), Alias("i")]
        [DiscordHelp("LookupItemInfo")]
        public class LookupItemInfo : BaseModule
        {
            public LookupItemInfo(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
            {
            }

            [Command]
            public async Task ItemLookup([Remainder] string search)
            {
                var searchResult = DbContext.FRItems.Where(x => x.Name.Contains(search) || x.Description.Contains(search)).ToList();

                if (searchResult.Count == 0)
                    await ReplyAsync($"Found no items that match `{search}`, I might not know about any item that match that or they don't exist.");
                else if (searchResult.Count == 1)
                {
                    var embed = await CreateItemEmbed(searchResult[0]);
                    await Context.Channel.SendFilesAsync(embed.Files, embed: embed.Embed.Build());
                }
                else
                {
                    if (searchResult.Count > 5)
                        await ReplyAsync($"Found {searchResult.Count} items that match `{search}`, please refine your search.");
                    else
                    {
                        var embed = new EmbedBuilder()
                            .WithDescription($"Found {searchResult.Count} items that match your query. Please look at the items below and use `{SettingManager.GetSettingValue("GUILDCONFIG_PREFIX", Context.Guild)}lookup item <frid>`")
                            .WithFields(searchResult.Select(x => new EmbedFieldBuilder().WithValue(x.FRId).WithName(x.Name).WithIsInline(true)));
                        await ReplyAsync("", embed: embed.Build());
                    }
                }
            }

            [Command]
            public async Task ItemLookup(int frItemId)
            {
                var item = DbContext.FRItems.FirstOrDefault(x => x.FRId == frItemId);
                if (item != null)
                {
                    var embed = await CreateItemEmbed(item);
                    await Context.Channel.SendFilesAsync(embed.Files, embed: embed.Embed.Build());
                }
                else
                    await ReplyAsync($"Can't find item `{frItemId}`, I might not know about it yet or it does not exist.");
            }

            private async Task<(EmbedBuilder Embed, IEnumerable<KeyValuePair<string, Stream>> Files)> CreateItemEmbed(FRItem item)
            {
                var files = new List<KeyValuePair<string, Stream>>();
                var externalEmojis = Context.Guild.CurrentUser.GuildPermissions.UseExternalEmojis;

                var embed = new EmbedBuilder()
                    .WithTitle(item.Name)
                    .WithDescription(item.Description)
                    .WithThumbnailUrl($"attachment://icon_{item.FRId}.png")
                    .WithFields(new EmbedFieldBuilder().WithName("Game database").WithValue($"[#{item.FRId}]({string.Format(FRHelpers.GameDatabaseUrl, item.FRId)})"));

                using (var client = new WebClient())
                    files.Add(new KeyValuePair<string, Stream>($"icon_{item.FRId}.png", client.OpenRead(string.Format(SiteHelpers.IconProxyUrl, item.FRId))));

                if (item.TreasureValue > 0)
                    embed.AddField(x => x.WithName("Treasure value").WithValue((externalEmojis ? $"{DiscordEmoji.Treasure} " : "") + $"{item.TreasureValue}").WithIsInline(true));

                if(item.FoodValue > 0)
                    embed.AddField(x => x.WithName("Food value").WithValue((externalEmojis ? $"{DiscordEmoji.Food} " : "") + $"{item.FoodValue}").WithIsInline(true));


                if (item.AssetUrl != null)
                {
                    if (item.ItemCategory == FRItemCategory.Equipment)
                    {
                        var random = new Random();
                        var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                        using (var client = new WebClient())
                            files.Add(new KeyValuePair<string, Stream>($"asset_{item.FRId}.png", client.OpenRead(string.Format(SiteHelpers.DummyDragonApparelProxyUrl, (int)modernBreeds[random.Next(1, modernBreeds.Length)], random.Next(0, 2), $"{item.FRId}"))));
                    }
                    else if (item.ItemCategory == FRItemCategory.Skins)
                    {
                        var skinType = item.ItemType.Split(' ');
                        var dragonType = (DragonType)Enum.Parse(typeof(DragonType), skinType[0]);
                        var gender = (Gender)Enum.Parse(typeof(Gender), skinType[1]);
                        using (var client = new WebClient())
                            files.Add(new KeyValuePair<string, Stream>($"asset_{item.FRId}.png", client.OpenRead(string.Format(SiteHelpers.DummyDragonSkinProxyUrl, (int)dragonType, (int)gender, $"{item.FRId}"))));

                        var username = Regex.Match(item.Description, @"Designed by (.+)[\.|\)]");
                        if (username.Success)
                        {
                            var frUser = await FRHelpers.GetOrUpdateFRUser(username.Groups[1].Value, DbContext);
                            if (frUser != null)
                            {
                                embed.AddField(x =>
                                {
                                    x.Name = "Created by";
                                    if (frUser.User != null && frUser.User.ProfileSettings.PublicProfile)
                                        x.Value = $"[FR: {frUser.Username}]({string.Format(FRHelpers.UserProfileUrl, frUser.FRId)}) | [FRTools: {frUser.User.UserName}]({string.Format(SiteHelpers.ProfilePageUrl, frUser.User.UserName)})";
                                    else
                                        x.Value = $"[{frUser.Username}]({string.Format(FRHelpers.UserProfileUrl, frUser.FRId)})";
                                });
                            }
                        }

                    }
                    else
                    {
                        using (var client = new WebClient())
                            files.Add(new KeyValuePair<string, Stream>($"asset_{item.FRId}.png", client.OpenRead($"https://flightrising.com{item.AssetUrl}")));
                    }
                    embed.WithImageUrl($"attachment://asset_{item.FRId}.png");
                }

                return (embed, files);
            }
        }
    }
}