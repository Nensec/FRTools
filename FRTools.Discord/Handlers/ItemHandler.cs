using Discord;
using Discord.WebSocket;
using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.Discord.Handlers
{
    public static class ItemHandler
    {
        public static async Task<(EmbedBuilder Embed, IEnumerable<KeyValuePair<string, Stream>> Files)> CreateItemEmbed(FRItem item, SocketGuild guild)
        {
            var files = new List<KeyValuePair<string, Stream>>();
            var externalEmojis = guild.CurrentUser.GuildPermissions.UseExternalEmojis;

            var embed = new EmbedBuilder()
                .WithTitle(item.Name)
                .WithDescription(item.Description)
                .WithThumbnailUrl($"attachment://icon_{item.FRId}.png")
                .WithFields(new EmbedFieldBuilder().WithName("Game database").WithValue($"[#{item.FRId}]({string.Format(FRHelpers.GameDatabaseUrl, item.FRId)})").WithIsInline(true));

            if (item.TreasureValue > 0)
                embed.AddField(x => x.WithName("Treasure value").WithValue((externalEmojis ? $"{DiscordHelpers.DiscordEmojis[DiscordEmoji.Treasure]} " : "") + $"{item.TreasureValue}").WithIsInline(true));

            if (item.FoodValue > 0)
                embed.AddField(x => x.WithName("Food value").WithValue((externalEmojis ? $"{DiscordHelpers.DiscordEmojis[DiscordEmoji.Food]} " : "") + $"{item.FoodValue}").WithIsInline(true));


            if (item.AssetUrl != null)
            {
                if (item.ItemCategory == FRItemCategory.Equipment)
                {
                    var random = new Random();
                    var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                    using (var client = new WebClient())
                    {
                        var assetStream = await client.OpenReadTaskAsync(string.Format(SiteHelpers.DummyDragonApparelProxyUrl, (int)modernBreeds[random.Next(1, modernBreeds.Length)], random.Next(0, 2), $"{item.FRId}"));
                        files.Add(new KeyValuePair<string, Stream>($"asset_{item.FRId}.png", assetStream));
                    }
                }
                else if (item.ItemCategory == FRItemCategory.Skins)
                {
                    var skinType = item.ItemType.Split(' ');
                    var dragonType = (DragonType)Enum.Parse(typeof(DragonType), skinType[0]);
                    var gender = (Gender)Enum.Parse(typeof(Gender), skinType[1]);
                    using (var client = new WebClient())
                    {
                        var assetStream = await client.OpenReadTaskAsync(string.Format(SiteHelpers.DummyDragonSkinProxyUrl, (int)dragonType, (int)gender, $"{item.FRId}"));
                        files.Add(new KeyValuePair<string, Stream>($"asset_{item.FRId}.png", assetStream));
                    }

                    var username = Regex.Match(item.Description, @"Designed by ([^.]+)[.|\)]");
                    if (username.Success)
                    {
                        var frUser = await FRHelpers.GetOrUpdateFRUser(username.Groups[1].Value);
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

                }
                else
                {
                    using (var client = new WebClient())
                    {
                        var assetStream = await client.OpenReadTaskAsync($"https://flightrising.com{item.AssetUrl}");
                        files.Add(new KeyValuePair<string, Stream>($"asset_{item.FRId}.png", assetStream));
                    }
                }
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
