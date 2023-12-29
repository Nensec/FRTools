using System;
using System.Collections.Generic;
using System.Linq;
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
                // First check if the breed is known in case of an ancient breed
                if (item.Name.Contains("("))
                {
                    var breed = item.Name.Split('(', ')')[1];
                    if (!CheckIfDragonTypeIsKnown(breed))
                    {
                        return true;
                    }
                }

                // Check if the gene itself is known
                var geneName = item.Name.Split(':', '(')[0].Trim();
                try
                {
                    if (item.Name.StartsWith("Primary"))
                        GetBodyGene(geneName);
                    if (item.Name.StartsWith("Secondary"))
                        GetWingGene(geneName);
                    if (item.Name.StartsWith("Tertiary"))
                        GetTertiaryGene(geneName);
                }
                catch (ArgumentException)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetDragonType(string dragontype, out DragonType result)
        {
            try
            {
                result = GetDragonType(dragontype);
                return true;
            }
            catch
            {
                result = DragonType.Unknown;
                return false;
            }
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
    }
}
