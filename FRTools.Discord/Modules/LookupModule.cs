using Discord;
using Discord.Commands;
using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Discord.Handlers;
using FRTools.Discord.Infrastructure;
using HtmlAgilityPack;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.Discord.Modules
{

    [DiscordSetting("LOOKUP_AUTO_LINK", typeof(bool), "Execute on link", "Execute the corresponding command when a Flight Rising link is posted in chat", DefaultValue = "true")]
    [DiscordSetting("LOOKUP_AUTO_LINK_CHANNELS", typeof(ITextChannel[]), "Allowed channels", "In which channels the bot will auto execute the command when a link is posted", DefaultValue = "ALL", Order = 10)]
    [Name("Lookup"), Group("lookup"), Alias("lu"), Summary("Lookup related commands")]
    [DiscordHelp("LookupModule")]
    public class LookupModule : BaseModule
    {
        public LookupModule(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
        {
        }

        [DiscordSetting("LOOKUP_DRAGON_SHOW_IMAGES", typeof(bool), "Show dragon images", "Display images in the lookup embed, set this to hide if your server members get easily triggered", "Show", "Hide", DefaultValue = "true")]
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
            bool.TryParse(await SettingManager.GetSettingValue("LOOKUP_DRAGON_SHOW_IMAGES", Context.Guild), out var showImages);

            var embed = new EmbedBuilder()
                .WithUrl(string.Format(FRHelpers.DragonProfileUrl, id));

            if (showImages)
            {
                embed.WithThumbnailUrl($"attachment://{id}_350.png")
                    .WithImageUrl($"attachment://{id}_350.png");
            }

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

                if (!showImages)
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
                    if (!showImages)
                        embed.AddField(x => x.WithIsInline(true).WithName("\u200B").WithValue("\u200B"));
                    embed.AddField(siblingsField);
                }
            }

            if (Context.AutomatedCommand)
                embed.WithFooter("This command was executed automatically. Don't want this? Have an administrator change the settings.");

            if (!showImages)
            {
                embed.AddField("Image", FRHelpers.GetRenderUrl(id));
            }

            if (showImages)
            {
                using (var webClient = new WebClient())
                {
                    var dragonImage = webClient.OpenRead(FRHelpers.GetRenderUrl(id));
                    await Context.Channel.SendFileAsync(dragonImage, $"{id}_350.png", embed: embed.Build());
                }
            }
            else
                await Context.Channel.SendMessageAsync(embed: embed.Build());

            await lookingUpMessage.DeleteAsync();
        }

        [Group("item"), Name("Item"), Alias("i")]
        [DiscordHelp("LookupItemInfo")]
        public class LookupItemInfo : LookupItemBase
        {
            public LookupItemInfo(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
            {
            }

            [Command]
            public Task ItemLookup([Remainder] string search) => ItemLookup(x => x.Name.Contains(search) || (x.ItemCategory != FRItemCategory.Skins && x.Description.Contains(search) || x.Creator.Username.Contains(search)), search);

            [Command]
            public Task ItemLookup(int frItemId) => ItemLookup(x => x.FRId == frItemId, frItemId);
        }

        [DiscordSetting("LOOKUP_SKIN_SHOW_IMAGES", typeof(bool), "Show skin images", "Display images in the lookup embed, set this to hide if your server members get easily triggered", "Show", "Hide", DefaultValue = "true")]
        [Group("skin"), Name("Skin"), Alias("accent", "s", "a")]
        [DiscordHelp("LookupSkinInfo")]
        public class LookupSkinInfo : LookupItemBase
        {
            public LookupSkinInfo(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
            {
            }

            [Command]
            public Task ItemLookup([Remainder] string search) => ItemLookup(x => x.ItemCategory == FRItemCategory.Skins && (x.Name.Contains(search) || x.Creator.Username.Contains(search)), search);

            [Command]
            public Task ItemLookup(int frItemId) => ItemLookup(x => x.ItemCategory == FRItemCategory.Skins && x.FRId == frItemId, frItemId);
        }

        [Group("food"), Name("Food"), Alias("f")]
        [DiscordHelp("LookupFoodInfo")]
        public class LookupFoodInfo : LookupItemBase
        {
            public LookupFoodInfo(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
            {
            }

            [Command]
            public Task ItemLookup([Remainder] string search) => ItemLookup(x => x.ItemCategory == FRItemCategory.Food && (x.Name.Contains(search) || x.Description.Contains(search)), search);

            [Command]
            public Task ItemLookup(int frItemId) => ItemLookup(x => x.ItemCategory == FRItemCategory.Food && x.FRId == frItemId, frItemId);
        }

        [DiscordSetting("LOOKUP_GENE_SHOW_IMAGES", typeof(bool), "Show gene images", "Display images in the lookup embed, set this to hide if your server members get easily triggered", "Show", "Hide", DefaultValue = "true")]
        [Group("trinket"), Name("Trinket"), Alias("t", "material", "mat", "m", "chest", "c", "bundle", "vista", "scene", "egg")]
        [DiscordHelp("LookupTrinketInfo")]
        public class LookupTrinketInfo : LookupItemBase
        {
            public LookupTrinketInfo(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
            {
            }

            [Command]
            public Task ItemLookup([Remainder] string search) => ItemLookup(x => x.ItemCategory == FRItemCategory.Trinket && (x.Name.Contains(search) || x.Description.Contains(search)), search);

            [Command]
            public Task ItemLookup(int frItemId) => ItemLookup(x => x.ItemCategory == FRItemCategory.Trinket && x.FRId == frItemId, frItemId);
        }

        [Group("equipment"), Name("Equipment"), Alias("equip", "e", "apparel", "apparal", "a")]
        [DiscordHelp("LookupEquipmentInfo")]
        public class LookupEquipmentInfo : LookupItemBase
        {
            public LookupEquipmentInfo(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
            {
            }

            [Command]
            public Task ItemLookup([Remainder] string search) => ItemLookup(x => x.ItemCategory == FRItemCategory.Equipment && (x.Name.Contains(search) || x.Description.Contains(search)), search);

            [Command]
            public Task ItemLookup(int frItemId) => ItemLookup(x => x.ItemCategory == FRItemCategory.Equipment && x.FRId == frItemId, frItemId);
        }

        [Group("battle_item"), Name("Battle Item"), Alias("battle", "augment", "ability", "energy", "accesory")]
        [DiscordHelp("LookupBattleItemInfo")]
        public class LookupBattleItemInfo : LookupItemBase
        {
            public LookupBattleItemInfo(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
            {
            }

            [Command]
            public Task ItemLookup([Remainder] string search) => ItemLookup(x => x.ItemCategory == FRItemCategory.Battle_Items && (x.Name.Contains(search) || x.Description.Contains(search)), search);

            [Command]
            public Task ItemLookup(int frItemId) => ItemLookup(x => x.ItemCategory == FRItemCategory.Battle_Items && x.FRId == frItemId, frItemId);
        }

        [Group("familiar"), Name("Familiar"), Alias("fam")]
        [DiscordHelp("LookupFamiliarInfo")]
        public class LookupFamiliarInfo : LookupItemBase
        {
            public LookupFamiliarInfo(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
            {
            }

            [Command]
            public Task ItemLookup([Remainder] string search) => ItemLookup(x => x.ItemCategory == FRItemCategory.Familiar && (x.Name.Contains(search) || x.Description.Contains(search)), search);

            [Command]
            public Task ItemLookup(int frItemId) => ItemLookup(x => x.ItemCategory == FRItemCategory.Familiar && x.FRId == frItemId, frItemId);
        }

        public abstract class LookupItemBase : BaseModule
        {
            public LookupItemBase(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
            {
            }

            protected async Task ItemLookup(Expression<Func<FRItem, bool>> query, string searchTerm)
            {
                var plsWait = await Context.Channel.SendMessageAsync("Searching for your item, this takes a moment..");

                var searchResult = DbContext.FRItems.Where(query).ToList();

                if (searchResult.Count == 0)
                    await ReplyAsync($"Found no items that match `{searchTerm}`, I might not know about any item that match that or they don't exist.");
                else if (searchResult.Count == 1)
                {
                    await plsWait.ModifyAsync(x => x.Content = "I found your item! Please give me a moment while I fetch the data..");
                    var embed = await ItemHandler.CreateItemEmbed(searchResult[0], Context, SettingManager);
                    await Context.Channel.SendFilesAsync(embed.Files, embed: embed.Embed.Build());
                }
                else
                {
                    if (searchResult.Count > 25)
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine($"Found {searchResult.Count} items that match `{searchTerm}`, but I can only display a preview of 25 items.");
                        var cats = searchResult.GroupBy(x => x.ItemCategory);
                        if (cats.Count() > 1)
                        {
                            foreach (var cat in searchResult.GroupBy(x => x.ItemCategory))
                                sb.AppendLine($"- {cat.Count()} in the category `{cat.Key}`");
                            sb.AppendLine();
                            sb.AppendLine($"Please refine your search, perhaps use a category filter such as `{await SettingManager.GetSettingValue("GUILDCONFIG_PREFIX", Context.Guild)}lookup {cats.FirstOrDefault().Key.ToString().ToLower()} {searchTerm}`");
                        }
                        else
                        {
                            sb.AppendLine();
                            sb.AppendLine($"All of the items were part of the category **{cats.FirstOrDefault().Key}**");
                            sb.AppendLine("Please refine your search.");
                        }
                        var embed = new EmbedBuilder()
                            .WithDescription(sb.ToString());
                        await ReplyAsync(embed: embed.Build());
                    }
                    else
                    {
                        var embed = new EmbedBuilder()
                            .WithDescription($"Found {searchResult.Count} items that match your query. Please look at the items below and use `{await SettingManager.GetSettingValue("GUILDCONFIG_PREFIX", Context.Guild)}lookup item <frid>` to view it's details.")
                            .WithFields(searchResult.Select(x => new EmbedFieldBuilder().WithValue($"{x.FRId} ({x.ItemCategory.ToString().ToLower()})").WithName(x.Name).WithIsInline(true)));
                        await ReplyAsync("", embed: embed.Build());
                    }
                }
                await plsWait.DeleteAsync();
            }

            protected async Task ItemLookup(Expression<Func<FRItem, bool>> query, int frItemId)
            {
                var item = DbContext.FRItems.FirstOrDefault(query);
                if (item != null)
                {
                    var plsWait = await Context.Channel.SendMessageAsync("I found your item! Please give me a moment while I fetch the data..");
                    var embed = await ItemHandler.CreateItemEmbed(item, Context, SettingManager);
                    await Context.Channel.SendFilesAsync(embed.Files, embed: embed.Embed.Build());
                    await plsWait.DeleteAsync();
                }
                else
                    await ReplyAsync($"Can't find item `{frItemId}`, I might not know about it yet or it does not exist.");
            }
        }
    }
}