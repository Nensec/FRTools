using System.Text.RegularExpressions;
using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services
{
    public static class Extentions
    {
        public static async Task<byte[]?> ParseItemForEmbed(this DiscordEmbed embed, Random random, FRItem item, IFRUserService userService, ILogger logger)
        {
            byte[]? itemAsset = null;

            if (item.FoodValue > 0)
                embed.Fields.Add(new DiscordEmbedField { Name = "Food value", Value = $"{item.FoodValue}", Inline = true });

            switch (item.ItemCategory)
            {
                case FRItemCategory.Equipment:
                    {
                        var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                        using (var client = new HttpClient())
                            itemAsset = await client.GetByteArrayAsync($"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/apparel/{(int)modernBreeds[random.Next(1, modernBreeds.Length)]}/{random.Next(0, 2)}/{item.FRId}");

                        break;
                    }
                case FRItemCategory.Skins:
                    {
                        var breed = item.ItemType.Split(' ');
                        var dragonType = FRHelpers.GetDragonType(breed[0]);
                        var gender = (Gender)Enum.Parse(typeof(Gender), breed[1]);

                        using (var client = new HttpClient())
                            itemAsset = await client.GetByteArrayAsync($"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/skin/{(int)dragonType}/{(int)gender}/{item.FRId}");

                        embed.Fields.Add(new DiscordEmbedField { Name = "For", Value = $"{dragonType} {gender}", Inline = true });

                        var username = Regex.Match(item.Description, @"Designed by ([^.]+)[.|\)]");
                        if (username.Success)
                        {
                            var frUser = await userService.GetOrUpdateFRUser(username.Groups[1].Value);
                            if (frUser != null)
                                embed.Fields.Add(new DiscordEmbedField { Name = "Created by", Value = $"[FR: {frUser.Username}]({string.Format(FRHelpers.UserProfileUrl, frUser.FRId)})" });
                        }

                        break;
                    }
                case FRItemCategory.Familiar:
                    {
                        using (var client = new HttpClient())
                            itemAsset = await client.GetByteArrayAsync(string.Format(FRHelpers.FamiliarArtUrl, item.FRId));

                        break;
                    }
                case FRItemCategory.Trinket when item.ItemType == "Specialty Item" && (item.Name.StartsWith("Primary") || item.Name.StartsWith("Secondary") || item.Name.StartsWith("Tertiary")):
                    {
                        if (item.Name.Contains('('))
                        {
                            if (FRHelpers.TryGetDragonType(item.Name.Split('(', ')')[1], out var dragonType))
                            {
                                using (var client = new HttpClient())
                                    itemAsset = await client.GetByteArrayAsync($"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/gene/{(int)dragonType}/{random.Next(0, 2)}/{item.FRId}");
                            }
                            else
                                embed.Description += "\n\r\nBreed is not (yet) found in my data so the image is unavailable!";
                        }
                        else
                        {
                            var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                            using (var client = new HttpClient())
                                itemAsset = await client.GetByteArrayAsync($"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/gene/{(int)modernBreeds[random.Next(1, modernBreeds.Length)]}/{random.Next(0, 2)}/{item.FRId}");
                        }

                        break;
                    }
                default:
                    {
                        if (item.ItemType == "Scene")
                        {
                            using (var client = new HttpClient())
                                itemAsset = await client.GetByteArrayAsync(string.Format(FRHelpers.SceneArtUrl, item.FRId));
                        }
                        else
                        {
                            if (item.AssetUrl != null)
                            {
                                logger.LogDebug("Unknown art type, attempting AssetURL");
                                using (var client = new HttpClient())
                                    itemAsset = await client.GetByteArrayAsync($"https://www1.flightrising.com{item.AssetUrl}");
                            }
                        }

                        break;
                    }
            }

            return itemAsset;
        }
    }
}
