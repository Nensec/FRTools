using Discord.Commands;
using FRTools.Data;
using FRTools.Discord.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Discord;
using Color = Discord.Color;
using System.Configuration;
using FRTools.Common;
using System.Net;
using FRTools.Data.DataModels.FlightRisingModels;
using System.Text.RegularExpressions;
using FRTools.Data.DataModels;
using System;

namespace FRTools.Discord.Modules
{
    [Name("Skin Tester"), Group("skintester"), Alias("st"), Summary("Skin Tester related commands")]
    [DiscordHelp("LookupModule")]
    public class SkinTesterModule : BaseModule
    {
        public SkinTesterModule(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
        {
        }

        [Command("lookup"), Name("Lookup"), Alias("lu")]
        public async Task Lookup(string skinId)
        {

            var skin = DbContext.Skins.Include(x => x.Creator.FRUser).FirstOrDefault(x => x.GeneratedId == skinId);
            if (skin == null)
                await ReplyAsync(embed: ErrorEmbed("Skin not found.").Build());
            else
            {
                var previewUrl = (await SkinTester.GenerateOrFetchPreview(skinId, skin.Version, "preview", string.Format(FRHelpers.DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null)).Urls[0];

                var embed = new EmbedBuilder();
                embed.WithTitle(skin.Title ?? "_No title_");
                embed.WithImageUrl($"{CDNBasePath}{previewUrl}");
                embed.WithDescription(skin.Description ?? "_No description_");
                embed.WithAuthor(new EmbedAuthorBuilder()
                    .WithName(skin.Creator?.UserName ?? "Anonymous")
                    .WithUrl(skin.Creator != null ? $"{WebsiteBaseUrl}/profile/{skin.Creator.UserName}" : null));
                embed.WithFooter(new EmbedFooterBuilder().WithText($"Version {skin.Version}"));
                embed.WithUrl($"{WebsiteBaseUrl}/skintester/preview/{skinId}");

                await ReplyAsync(embed: embed.Build());
            }

        }

        [Group("preview"), Name("Preview"), Alias("p")]
        public class SkinTesterPreview : BaseModule
        {
            public SkinTesterPreview(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
            {
            }

            [Command]
            public async Task Preview(string skinId, int dragonId, [Remainder]string apparelSwitch = null)
            {
                string dragonUrl = FRHelpers.GetDragonImageUrlFromDragonId(dragonId);
                if (dragonUrl.StartsWith(".."))
                    await ReplyAsync(embed: ErrorEmbed($"**{dragonId}** appears to be an invalid dragon id").Build());
                else
                    await GeneratePreview(skinId, dragonUrl, dragonId, apparelSwitch == "-apparel");
            }

            [Command]
            public async Task Preview(string skinId, string dressingRoomUrl, [Remainder]string apparelSwitch = null)
            {
                await GeneratePreview(skinId, dressingRoomUrl, null, apparelSwitch == "-apparel");
            }

            private async Task<Task> GeneratePreview(string skinId, string dragonUrl, int? dragonId = null, bool showApparel = false)
            {
                DragonCache dragon = null;
                bool isDressingRoomUrl = dragonUrl.Contains("/dgen/dressing-room");
                string dressingRoomUrl = isDressingRoomUrl ? dragonUrl : null;
                if (isDressingRoomUrl)
                {
                    var apparelDragon = FRHelpers.ParseUrlForDragon(dragonUrl);
                    if (dragonUrl.Contains("/dgen/dressing-room/dummy"))
                        dragon = FRHelpers.ParseUrlForDragon(dragonUrl);
                    else if (dragonUrl.Contains("dgen/dressing-room/scry"))
                    {
                        var scryId = int.Parse(Regex.Match(dragonUrl, @"sdid=([\d]*)").Groups[1].Value);
                        var scryUrl = FRHelpers.GetDragonImageUrlFromScryId(scryId);
                        dragon = FRHelpers.ParseUrlForDragon(scryUrl);
                    }
                    else
                    {
                        dragonId = int.Parse(Regex.Match(dragonUrl, @"did=([\d]*)").Groups[1].Value);
                        dragon = FRHelpers.GetDragonFromDragonId(dragonId.Value);
                    }

                    if (FRHelpers.IsAncientBreed(dragon.DragonType))
                        return ReplyAsync(embed: ErrorEmbed("Ancient breeds cannot wear apparal.").Build());

                    dragon.Apparel = apparelDragon.Apparel;
                    if (dragon.GetApparel().Length == 0)
                        return ReplyAsync(embed: ErrorEmbed("This dressing room URL contains no apparel.").Build());
                }
                else
                    dragon = FRHelpers.ParseUrlForDragon(dragonUrl);

                if (dragon.Age == Age.Hatchling)
                    return ReplyAsync(embed: ErrorEmbed("Skins can only be previewed on adult dragons.").Build());

                using (var ctx = new DataContext())
                {
                    var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == skinId);
                    if (skin == null)                    
                        return ReplyAsync(embed: ErrorEmbed("Skin not found.").Build());                    

                    if (skin.DragonType != (int)dragon.DragonType)                    
                        return ReplyAsync(embed: ErrorEmbed($"This skin is meant for a **{(DragonType)skin.DragonType} {(Gender)skin.GenderType}**, the dragon you provided is a **{dragon.DragonType} {dragon.Gender}**.").Build());

                    if (skin.GenderType != (int)dragon.Gender)
                    {
                        return ReplyAsync(embed: ErrorEmbed($"This skin is meant for a **{(Gender)skin.GenderType}**, the dragon you provided is a **{dragon.Gender}**.").Build());
                    }

                    var replyMessage = await ReplyAsync(embed: new EmbedBuilder().WithTitle("Generating your preview..").WithDescription("I'm fetching all the required data to preview your skin, this takes a moment..").Build());

                    var previewResult = await SkinTester.GenerateOrFetchPreview(skinId, skin.Version, dragonId?.ToString(), dragonUrl, dragon, dressingRoomUrl);

                    foreach (var url in previewResult.Urls.Where(url => !skin.Previews.Any(x => x.PreviewImage == url)))
                    {
                        skin.Previews.Add(new Preview
                        {
                            DragonId = dragonId,
                            ScryerUrl = dragonId == null ? dragonUrl : null,
                            PreviewImage = url,
                            DragonData = dragon.ToString(),
                            PreviewTime = DateTime.UtcNow,
                            Requestor = null,
                            Version = skin.Version
                        });
                    }

                    await ctx.SaveChangesAsync();

                    var embed = new EmbedBuilder();
                    embed.WithDescription($"Here is your preview on **{skin.Title ?? skin.GeneratedId}**");
                    if (!showApparel && previewResult.Urls.Length > 1)
                        embed.Description += "\r\n\nYou can show the apparel version of the preview by adding **-apparel** at the end of the preview command.";

                    embed.WithFields(new EmbedFieldBuilder().WithName("Dragon preview").WithValue($"[click here]({CDNBasePath}{previewResult.Urls[0]})"));
                    if (previewResult.Urls.Length > 1)
                        embed.WithFields(new EmbedFieldBuilder().WithName("Apparel preview").WithValue($"[click here]({CDNBasePath}{previewResult.Urls[1]})"));
                    if (!showApparel || previewResult.Urls.Length == 1)
                        embed.WithImageUrl($"{CDNBasePath}{previewResult.Urls[0]}");
                    else
                        embed.WithImageUrl($"{CDNBasePath}{previewResult.Urls[1]}");

                    embed.WithAuthor(new EmbedAuthorBuilder().WithName($"{skin.Title ?? skin.GeneratedId} v{skin.Version}"));
                    embed.WithThumbnailUrl($"{CDNBasePath}/previews/{skinId}/preview.png");
                    return replyMessage.ModifyAsync(x => x.Embed = embed.Build());
                }
            }

        }

        [Command("manage")]
        public override Task ManageModule() => base.ManageModule();
    }
}
