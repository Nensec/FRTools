using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.MS.ItemFetcher
{
    class Program
    {
        static int _requestsMade = 0;

        static async Task Main()
        {
            using (var ctx = new DataContext())
            {
                var highestItemId = ctx.FRItems.Any() ? ctx.FRItems.Max(x => x.FRId) : 0;

                while(_requestsMade <= 20)
                {
                    var item = FetchItem(++highestItemId);
                    ctx.FRItems.Add(item);
                    await Task.Delay(10);
                }
                await ctx.SaveChangesAsync();
            }
        }

        static FRItem FetchItem(int itemId, string category = "food")
        {
            _requestsMade++;

            var client = new HtmlWeb();
            var itemDoc = client.Load(string.Format(FRHelpers.ItemFetchUrl, itemId, category));
            var iconUrl = itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/img[2]").GetAttributeValue("src", "/images/cms//.png");

            if (iconUrl == "/images/cms//.png")
            {
                // Item does not exist
                return null;
            }

            var categoryMatch = Regex.Match(iconUrl, @"/images/cms/(?<Category>.*)/(\d*).png");
            if (categoryMatch.Groups["Category"].Value != category)
                return FetchItem(itemId, categoryMatch.Groups["Category"].Value);

            var item = new FRItem { FRId = itemId, IconUrl = iconUrl, ItemCategory = (FRItemCategory)Enum.Parse(typeof(FRItemCategory), category, true) };
            item.Name = itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/div[1]").InnerText;
            item.Description = itemDoc.DocumentNode.SelectSingleNode("/div/div[2]").InnerText;
            item.ItemType = itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/div[2]").InnerText;
            var rarityUrl = itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/img[1]").GetAttributeValue("src", "");
            var rarityMatch = Regex.Match(rarityUrl, @"../images/layout/tooltips/star-(?<Rarity>\d).png");
            if (rarityMatch.Success)
                item.Rarity = int.Parse(rarityMatch.Groups["Rarity"].Value);

            switch (item.ItemCategory)
            {
                case FRItemCategory.Food:
                    item.FoodValue = int.Parse(itemDoc.DocumentNode.SelectSingleNode("/div/div[4]").InnerText);
                    item.FoodType = (FRFoodType)Enum.Parse(typeof(FRFoodType), item.ItemType, true);
                    break;
                case FRItemCategory.Skins:
                    item.AssetUrl = itemDoc.DocumentNode.SelectSingleNode("/div/div[2]/div/img").GetAttributeValue("src", "");
                    break;
                case FRItemCategory.Equipment:
                    break;
                case FRItemCategory.Familiar:
                    break;
                case FRItemCategory.Battle_Items:
                    break;
                case FRItemCategory.Trinket:
                    break;
                default:
                    break;
            }

            return item;
        }
    }
}
