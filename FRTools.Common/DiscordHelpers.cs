using System.Collections.Generic;

namespace FRTools.Common
{
    public static class DiscordHelpers
    {
        public static Dictionary<DiscordEmoji, string> DiscordEmojis = new Dictionary<DiscordEmoji, string>
        {
            { DiscordEmoji.Earth, "<:earthflight:703609147987329044>" },
            { DiscordEmoji.Plague, "<:plagueflight:703609147907637258>" },
            { DiscordEmoji.Wind, "<:windflight:703609147815362621>" },
            { DiscordEmoji.Water, "<:waterflight:703609147958100039>" },
            { DiscordEmoji.Lightning, "<:lightningflight:703609147899248681>" },
            { DiscordEmoji.Ice, "<:iceflight:703609148142649504>" },
            { DiscordEmoji.Shadow, "<:shadowflight:703609148130066562>" },
            { DiscordEmoji.Light, "<:lightflight:703609148054700113>" },
            { DiscordEmoji.Arcane, "<:arcaneflight:703609147962425424>" },
            { DiscordEmoji.Nature, "<:natureflight:703609148109226014>" },
            { DiscordEmoji.Fire, "<:fireflight:703609147718893609>" },
            { DiscordEmoji.Beastclans, "<:beastclans:703609148058894347>" },
            { DiscordEmoji.Dominance, "<:discount:703609171676889169>" },
            { DiscordEmoji.Lair, "<:lairspace:703609171844530216>" },
            { DiscordEmoji.Treasure, "<:treasure:703609171492339795>" },
            { DiscordEmoji.Food, "<:food:703609171865763941>" }
        };
    }

    public enum DiscordEmoji
    {
        Earth,
        Plague,
        Wind,
        Water,
        Lightning,
        Ice,
        Shadow,
        Light,
        Arcane,
        Nature,
        Fire,
        Beastclans,
        Dominance,
        Lair,
        Treasure,
        Food
    }
}
