namespace FRTools.Core.Helpers
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
    }
}
