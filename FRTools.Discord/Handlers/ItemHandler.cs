using Discord;
using Discord.WebSocket;
using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Discord.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.Discord.Handlers
{
    public static class ItemHandler
    {
        public static async Task<(EmbedBuilder Embed, IEnumerable<KeyValuePair<string, Stream>> Files)> CreateItemEmbed(FRItem item, FRToolsSocketCommandContext context, SettingManager settingManager, DataContext ctx, bool flashSale = false)
        {
            var embedResult = await CreateItemEmbed(item, context.Guild, settingManager, ctx, flashSale);

            if (context.AutomatedCommand)
                embedResult.Embed.WithFooter("This command was executed automatically. Don't want this? Have an administrator change the settings.");

            return embedResult;
        }

        public static async Task<(EmbedBuilder Embed, IEnumerable<KeyValuePair<string, Stream>> Files)> CreateItemEmbed(FRItem item, SocketGuild guild, SettingManager settingManager, DataContext ctx, bool flashSale = false)
        {
            var files = new List<KeyValuePair<string, Stream>>();
            var externalEmojis = guild.CurrentUser.GuildPermissions.UseExternalEmojis;

            var embed = new EmbedBuilder()
                .WithTitle(item.Name)
                .WithDescription(item.Description)
                .WithThumbnailUrl($"attachment://icon_{item.FRId}.png")
                .WithFields(new EmbedFieldBuilder().WithName("Game database").WithValue($"[#{item.FRId}]({string.Format(FRHelpers.GameDatabaseUrl, item.FRId)})").WithIsInline(true));

            if (item.TreasureValue > 0)
                embed.AddField(x => x.WithName("Treasure value").WithValue((externalEmojis ? $"{DiscordHelpers.DiscordEmojis[DiscordEmoji.Treasure]} " : "") + $"{(flashSale ? $"~~{item.TreasureValue * 10}~~ ***{item.TreasureValue * .8 * 10}***" : $"{item.TreasureValue}")}").WithIsInline(true));

            if (item.FoodValue > 0)
                embed.AddField(x => x.WithName("Food value").WithValue((externalEmojis ? $"{DiscordHelpers.DiscordEmojis[DiscordEmoji.Food]} " : "") + $"{item.FoodValue}").WithIsInline(true));

            bool showImages = true;
            Stream assetStream = null;
            var random = new Random();
            switch (item.ItemCategory)
            {
                case FRItemCategory.Equipment:
                    {
                        var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                        using (var client = new WebClient())
                            assetStream = await client.OpenReadTaskAsync(string.Format(SiteHelpers.DummyDragonApparelProxyUrl, (int)modernBreeds[random.Next(1, modernBreeds.Length)], random.Next(0, 2), $"{item.FRId}"));

                        break;
                    }
                case FRItemCategory.Skins:
                    {
                        showImages = bool.TryParse(await settingManager.GetSettingValue("LOOKUP_SKIN_SHOW_IMAGES", guild), out var showSkinImages) && showSkinImages;

                        var skinType = item.ItemType.Split(' ');
                        var dragonType = (DragonType)Enum.Parse(typeof(DragonType), skinType[0]);
                        var gender = (Gender)Enum.Parse(typeof(Gender), skinType[1]);

                        if (showImages)
                            using (var client = new WebClient())
                                assetStream = await client.OpenReadTaskAsync(string.Format(SiteHelpers.DummyDragonSkinProxyUrl, (int)dragonType, (int)gender, $"{item.FRId}"));

                        embed.AddField(x =>
                        {
                            x.Name = "For";
                            x.Value = $"{dragonType} {gender}";
                            x.IsInline = true;
                        });

                        var username = Regex.Match(item.Description, @"Designed by ([^.]+)[.|\)]");
                        if (username.Success)
                        {
                            var frUser = await FRHelpers.GetOrUpdateFRUser(username.Groups[1].Value, ctx);
                            if (frUser != null)
                            {
                                embed.AddField(x =>
                                {
                                    x.Name = "Created by";
                                    x.IsInline = true;
                                    if (frUser.User != null && frUser.User.ProfileSettings.PublicProfile)
                                        x.Value = $"[FR: {frUser.Username}]({string.Format(FRHelpers.UserProfileUrl, frUser.FRId)}) | [FRTools: {frUser.User.UserName}]({string.Format(SiteHelpers.ProfilePageUrl, frUser.User.UserName)})";
                                    else
                                        x.Value = $"[{frUser.Username}]({string.Format(FRHelpers.UserProfileUrl, frUser.FRId)})";
                                });
                            }
                        }

                        break;
                    }
                case FRItemCategory.Familiar:
                    {
                        using (var client = new WebClient())
                            assetStream = await client.OpenReadTaskAsync(string.Format(FRHelpers.FamiliarArtUrl, item.FRId));

                        break;
                    }
                case FRItemCategory.Trinket when item.ItemType == "Specialty Item" && (item.Name.StartsWith("Primary") || item.Name.StartsWith("Secondary") || item.Name.StartsWith("Tertiary")):
                    {
                        showImages = bool.TryParse(await settingManager.GetSettingValue("LOOKUP_GENE_SHOW_IMAGES", guild), out var showSkinImages) && showSkinImages;

                        if (showImages)
                        {
                            var ancientBreed = GeneratedFRHelpers.GetAncientBreeds().Where(x => item.Name.EndsWith($"({x})"));
                            if (ancientBreed.Any())
                            {
                                using (var client = new WebClient())
                                    assetStream = await client.OpenReadTaskAsync(string.Format(SiteHelpers.DummyDragonGeneProxyUrl, (int)ancientBreed.First(), random.Next(0, 2), $"{item.FRId}"));
                            }
                            else
                            {
                                var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                                using (var client = new WebClient())
                                    assetStream = await client.OpenReadTaskAsync(string.Format(SiteHelpers.DummyDragonGeneProxyUrl, (int)modernBreeds[random.Next(1, modernBreeds.Length)], random.Next(0, 2), $"{item.FRId}"));
                            }
                        }
                        break;
                    }
                default:
                    {
                        if (item.ItemType == "Scene")
                        {
                            using (var client = new WebClient())
                                assetStream = await client.OpenReadTaskAsync(string.Format(FRHelpers.SceneArtUrl, item.FRId));
                        }
                        else
                        {
                            if (item.AssetUrl != null)
                            {
                                Console.WriteLine("Unknown art type, attempting AssetURL");
                                using (var client = new WebClient())
                                    assetStream = await client.OpenReadTaskAsync($"https://www1.flightrising.com{item.AssetUrl}");
                            }
                        }

                        break;
                    }
            }
            if (showImages && assetStream != null)
            {
                files.Add(new KeyValuePair<string, Stream>($"asset_{item.FRId}.png", assetStream));
                embed.WithImageUrl($"attachment://asset_{item.FRId}.png");
            }

            using (var client = new WebClient())
            {
                var iconStream = await client.OpenReadTaskAsync(string.Format(SiteHelpers.IconProxyUrl, item.FRId));
                files.Add(new KeyValuePair<string, Stream>($"icon_{item.FRId}.png", iconStream));
            }

            return (embed, files);
        }
    }
}
