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
        public static async Task<byte[]?> ParseItemForEmbed(this DiscordEmbed embed, Random random, FRItem item, IFRUserService userService, ILogger logger, DragonType? dragonType = null, Gender? gender = null)
        {
            byte[]? itemAsset = null;

            embed.Title ??= item.Name;
            embed.Description ??= item.Description;
            embed.Thumbnail ??= new DiscordEmbedThumbnail { Url = $"attachment://icon_{item.FRId}.png" };
            embed.Fields.Add(new DiscordEmbedField { Name = "Game database", Value = $"[#{item.FRId}]({string.Format(FRHelpers.GameDatabaseUrl, item.FRId)})", Inline = true });

            if (item.TreasureValue > 0)
                embed.Fields.Add(new DiscordEmbedField { Name = "Treasure value", Value = $"{item.TreasureValue}", Inline = true });

            if (item.FoodValue > 0)
                embed.Fields.Add(new DiscordEmbedField { Name = "Food value", Value = $"{item.FoodValue}", Inline = true });

            switch (item.ItemCategory)
            {
                case FRItemCategory.Equipment:
                    {
                        var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                        if (dragonType == null || !modernBreeds.Contains(dragonType.Value))
                            dragonType = modernBreeds[random.Next(1, modernBreeds.Length)];
                        
                        using (var client = new HttpClient())
                            itemAsset = await client.GetByteArrayAsync(Helpers.GetProxyDummyDragonApparelUrl((int)dragonType, (int?)gender ?? random.Next(0, 2), item.FRId));

                        break;
                    }
                case FRItemCategory.Skins:
                    {
                        var breed = item.ItemType.Split(' ');
                        dragonType = FRHelpers.GetDragonType(breed[0]);
                        gender = (Gender)Enum.Parse(typeof(Gender), breed[1]);

                        using (var client = new HttpClient())
                            itemAsset = await client.GetByteArrayAsync(Helpers.GetProxyDummyDragonSkinUrl((int)dragonType, (int)gender, item.FRId));

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
                            if (FRHelpers.TryGetDragonType(item.Name.Split('(', ')')[1], out dragonType) && dragonType != null)
                            {
                                using (var client = new HttpClient())
                                    itemAsset = await client.GetByteArrayAsync(Helpers.GetProxyDummyDragonGeneUrl((int)dragonType, (int?)gender ?? random.Next(0, 2), item.FRId));
                            }
                            else
                                embed.Description += "\n\r\nBreed is not (yet) found in my data so the image is unavailable! If this breed was announced very recently it can take up to an hour or so for my systems to be auto-updated.";
                        }
                        else
                        {
                            var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                            if (dragonType == null || !modernBreeds.Contains(dragonType.Value))
                                dragonType = modernBreeds[random.Next(1, modernBreeds.Length)];

                            using (var client = new HttpClient())
                                itemAsset = await client.GetByteArrayAsync(Helpers.GetProxyDummyDragonGeneUrl((int)dragonType, (int?)gender ?? random.Next(0, 2), item.FRId));
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
