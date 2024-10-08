﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;

namespace FRTools.Core.Common
{
    public static class FRHelpers
    {
        public const string DressingRoomDummyUrl = "https://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}";
        public const string ScryerUrl = "https://flightrising.com/includes/scryer_getdragon.php?zord={0}";
        public const string DragonProfileUrl = "https://www1.flightrising.com/dragon/{0}";
        public const string DragonProfileUrlNoScenic = "https://www1.flightrising.com/dragon/{0}?view=default";
        public const string DressingRoomDragonApparalUrl = "https://www1.flightrising.com/dgen/dressing-room/dragon?did={0}&apparel={1}";
        public const string DressingRoomDummyApparalUrl = "https://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}&apparel={2}";
        public const string DressingRoomDummySkinUrl = "https://www1.flightrising.com/hoard/preview-image?gender={1}&breed={0}&item={2}";
        public const string ItemFetchUrl = "https://flightrising.com/includes/itemajax.php?id={0}&tab={1}";
        public const string FamiliarArtUrl = "https://www1.flightrising.com/static/cms/familiar/art/{0}.png";
        public const string SceneArtUrl = "https://www1.flightrising.com/static/cms/scene/{0}.png";
        public const string UserProfileUrl = "https://www1.flightrising.com/clan-profile/{0}";
        public const string GameDatabaseUrl = "https://www1.flightrising.com/game-database/item/{0}";
        public const string MarketplaceUrl = "https://www1.flightrising.com/market/treasure/";
        public const string MarketplaceFetchUrl = "https://www1.flightrising.com/market/ajax/get-items?mode=treasure&tab={0}";

        public static string CleanupFRHtmlText(string input)
        {
            return input.Replace('\u000A', '\u0020')
                .Replace('\u0009', '\u0020')
                .Replace('\u000D', '\u0020')
                .Trim();
        }

        public static DragonType GetDragonType(string dragonType) => (DragonType)Enum.Parse(typeof(DragonType), dragonType, true);
        public static AllBodyGene GetBodyGene(string bodyGene) => (AllBodyGene)Enum.Parse(typeof(AllBodyGene), bodyGene, true);
        public static AllWingGene GetWingGene(string wingGene) => (AllWingGene)Enum.Parse(typeof(AllWingGene), wingGene, true);
        public static AllTertiaryGene GetTertiaryGene(string tertiaryGene) => (AllTertiaryGene)Enum.Parse(typeof(AllTertiaryGene), tertiaryGene, true);

        public static bool CheckForUnknownGenesOrBreed(List<FRItem> items)
        {
            bool CheckIfDragonTypeIsKnown(string breed)
            {
                try
                {
                    var _ = GetDragonType(breed);
                }
                catch (ArgumentException)
                {
                    return false;
                }
                return true;
            }

            // Checking skins is more a sanity check, but it could be the case if a new modern is introduced and no genes were added in the same newspost
            foreach (var item in items.Where(x => x.ItemCategory == FRItemCategory.Skins))
            {
                var breed = item.ItemType.Split(' ')[0];
                return !CheckIfDragonTypeIsKnown(breed);
            }

            foreach (var item in items.Where(x => x.ItemType == "Specialty Item" && (x.Name.StartsWith("Primary") || x.Name.StartsWith("Secondary") || x.Name.StartsWith("Tertiary"))))
            {
                // FR seems to have two flavors now
                // Tertiary Gene: Rockbreaker (Sandsurge) - This is the old style
                // Tertiary Auraboa Gene: Crystalline - This is new style

                // First check if the breed is known in case of an ancient breed 
                if (item.Name.Contains("(") && !CheckIfDragonTypeIsKnown(item.Name.Split('(', ')')[1])) // old style
                    return true;
                else if (!item.Name.StartsWith("Primary Gene") && !item.Name.StartsWith("Secondary Gene") && !item.Name.StartsWith("Tertiary Gene") && !CheckIfDragonTypeIsKnown(item.Name.Split(" ")[1].Trim())) // new style
                    return true;

                // Check if the gene itself is known
                try
                {
                    var nameSplit = item.Name.Split(':', '(', ')');

                    string enumName = item.Name.Contains('(')
                        ? $"{nameSplit[2].Trim()}_{nameSplit[1].Trim()}"
                        : $"{nameSplit[0].Split(" ")[1].Trim()}_{nameSplit[1].Trim()}";

                    if (item.Name.StartsWith("Primary"))
                        GetBodyGene(enumName);
                    if (item.Name.StartsWith("Secondary"))
                        GetWingGene(enumName);
                    if (item.Name.StartsWith("Tertiary"))
                        GetTertiaryGene(enumName);
                }
                catch (ArgumentException)
                {
                    return true;
                }
            }

            return false;
        }

        public static Flight GetFlightFromGodName(string godName)
        {
            switch (godName.ToLower())
            {
                default:
                case "earthshaker":
                    return Flight.Earth;
                case "plaguebringer":
                    return Flight.Plague;
                case "windsinger":
                    return Flight.Wind;
                case "tidelord":
                    return Flight.Water;
                case "stormcatcher":
                    return Flight.Lightning;
                case "icewarden":
                    return Flight.Ice;
                case "shadowbinder":
                    return Flight.Shadow;
                case "lightweaver":
                    return Flight.Light;
                case "arcanist":
                    return Flight.Arcane;
                case "gladekeeper":
                    return Flight.Nature;
                case "flamecaller":
                    return Flight.Fire;
            }
        }

        public static string GetRenderUrl(long dragonId) => $"https://www1.flightrising.com/rendern/350/{(Math.Floor(dragonId / 100d) + 1)}/{dragonId}_350.png";

        public static bool IsAncientGene(FRItem item, out DragonType? dragonType)
        {
            dragonType = GeneratedFRHelpers.GetAncientBreeds().Cast<DragonType?>().FirstOrDefault(x =>
            {
                if (item.Name.EndsWith($"({x})"))
                    return true;

                var regex = GeneRegex.Match(item.Name);
                return regex.Success && regex.Groups["Ancient"].Value == x.ToString();
            });

            return dragonType != null;
        }

        public static int GetGeneId(FRItem item)
        {
            if (item.ItemCategory != FRItemCategory.Trinket && item.ItemType != "Specialty Items")
                throw new Exception("Definitely not a gene.");

            Type geneType;
            if (!IsAncientGene(item, out var dragonType))
                dragonType = DragonType.Fae;

            if (item.Name.StartsWith("Primary"))
                geneType = dragonType!.Value.PrimaryGeneType();
            else if (item.Name.StartsWith("Secondary"))
                geneType = dragonType!.Value.SecondaryGeneType();
            else if (item.Name.StartsWith("Tertiary"))
                geneType = dragonType!.Value.TertiaryGeneType();
            else
                throw new Exception("Something went poopy parsing name for genetype!");

            var regex = GeneRegex.Match(item.Name);
            var geneName = regex.Groups["Name"].Value;

            return (int)Enum.Parse(geneType, geneName.Replace(" ", ""));
        }

        private static Regex GeneRegex => new(@"(?<Type>Primary|Secondary|Tertiary) ((?<Ancient>.+) )?Gene: (?<Name>.+)");
    }
}
