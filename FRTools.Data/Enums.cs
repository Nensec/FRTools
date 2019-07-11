﻿using System.ComponentModel;

namespace FRTools.Data
{
    public enum DragonType
    {
        Fae = 1,
        Guardian,
        Mirror,
        Pearlcatcher,
        Ridgeback,
        Tundra,
        Spiral,
        Imperial,
        Snapper,
        Wildclaw,
        Nocturne,
        Coatl,
        Skydancer,
        Bogsneak,
        Gaoler = 17
    }

    public enum GaolerBodyGene
    {
        Basic = 0,
        Giraffe = 27,
        Wasp,
        Shaggy,
        Falcon,
        Piebald,
        Pinstripe,
        Jaguar,
        Bar,
        Tapir,
        Tiger,
        Crystal,
        Mosaic,
        Phantom,
    }

    public enum GaolerWingGene
    {
        Basic = 0,
        Hex = 27,
        Bee,
        Streak,
        Peregrine,
        Paint,
        Trail,
        Rosette,
        Daub,
        Striation,
        Stripes,
        Facet,
        Breakup,
        Spirit,
    }

    public enum GaolerTertGene
    {
        Basic = 0,
        Ghost = 25,
        Shardflank,
        Gnarlhorns,
        Smoke,
        Thylacine,
        Ringlets,
        Underbelly,
        Runes,
        Scorpion,
        Wintercoat,
        Weathered,
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum Element
    {
        Earth = 1,
        Plague,
        Wind,
        Water,
        Lightning,
        Ice,
        Shadow,
        Light,
        Arcane,
        Nature,
        Fire
    }

    public enum EyeType
    {
        Common,
        Uncommon,
        Unusual,
        Rare,
        Faceted,
        [Description("Multi-Gaze")]
        MultiGaze,
        Primal,
        Glowing,
        [Description("Dark Sclera")]
        DarkSclera,
        Goat,
        Swirl
    }

    public enum BodyGene
    {
        Basic,
        Iridescent,
        Tiger,
        Clown,
        Speckle,
        Ripple,
        Bar,
        Crystal,
        Vipera,
        Piebald,
        Cherub,
        Poison,
        Giraffe,
        Petals,
        Jupiter,
        Skink,
        Falcon,
        Metallic,
        Savannah,
        Jaguar,
        Wasp,
        Tapir,
        Pinstripe,
        Python,
        Starmap,
        Lionfish,
        Laced
    }

    public enum TertiaryGene
    {
        Basic,
        Circuit,
        Gembond = 4,
        Underbelly,
        Crackle,
        Smoke,
        Spines,
        Okapi,
        Glimmer,
        Thylacine,
        Stained,
        Contour,
        Runes,
        Scales,
        Lace,
        Opal,
        Capsule,
        Smirch,
        Ghost,
        Filigree,
        Firefly,
        Ringlets,
        Peacock
    }

    public enum WingGene
    {
        Basic,
        Shimmer,
        Stripes,
        EyeSpots,
        Freckle,
        Seraph,
        Current,
        Daub,
        Facet,
        Hypnotic,
        Paint,
        Peregrine,
        Toxin,
        Butterfly,
        Hex,
        Saturn,
        Spinner,
        Alloy,
        Safari,
        Rosette,
        Bee,
        Striation,
        Trail,
        Morph,
        Noxtide,
        Constellation,
        Edged
    }

    public enum Color
    {
        Maize = 1,
        White,
        Ice,
        Platinum,
        Silver,
        Grey,
        Charcoal,
        Coal,
        Black,
        Obsidian,
        Midnight,
        Shadow,
        Mulberry,
        Thistle,
        Lavender,
        Purple,
        Violet,
        Royal,
        Storm,
        Navy,
        Blue,
        Splash,
        Sky,
        Stonewash,
        Steel,
        Denim,
        Azure,
        Caribbean,
        Teal,
        Aqua,
        Seafoam,
        Jade,
        Emerald,
        Jungle,
        Forest,
        Swamp,
        Avocado,
        Green,
        Leaf,
        Spring,
        Goldenrod,
        Lemon,
        Banana,
        Ivory,
        Gold,
        Sunshine,
        Orange,
        Fire,
        Tangerine,
        Sand,
        Beige,
        Stone,
        Slate,
        Soil,
        Brown,
        Chocolate,
        Rust,
        Tomato,
        Crimson,
        Blood,
        Maroon,
        Red,
        Carmine,
        Coral,
        Magenta,
        Pink,
        Rose,
        Heather,
        Orchid,
        Oilslick,
        Sapphire,
        Wine,
        Mauve,
        Moon,
        Marigold,
        Tan,
        Cinnamon,
        Spearmint,
        Mantis,
        Shamrock,
        Hunter,
        Iris,
        Bronze,
        Saffron,
        Pearl,
        Ruby,
        Berry,
        Hickory,
        Cyan,
        Ultramarine,
        Smoke,
        Plum,
        Honey,
        Copper,
        Taupe,
        Abyss,
        Antique,
        Gloom,
        Robin,
        Spruce,
        Pear,
        Honeydew,
        Amber,
        Yellow,
        Peach,
        Clay,
        Brick,
        Terracotta,
        Bubblegum,
        Sanddollar,
        Eggplant,
        Indigo,
        Fern,
        Amethyst,
        Moss,
        Cherry,
        Cerulean,
        Lead,
        Wisteria,
        Watermelon,
        Sanguine,
        Ginger,
        Olive,
        Tarnish,
        Pistachio,
        Overcast,
        Blackberry,
        Grapefruit,
        Flint,
        Radioactiv,
        Orca,
        Cerise,
        Carrot,
        Peacock,
        Periwinkle,
        Cobalt,
        Fog,
        Sable,
        Flaxen,
        Metals,
        Thicket,
        Murk,
        Latte,
        Peridot,
        Cornflower,
        Dust,
        Grape,
        Lapis,
        Turquoise,
        Mist,
        Phthalo,
        Mint,
        Algae,
        Camo,
        Chartreuse,
        Caramel,
        Umber,
        Pumpkin,
        Blush,
        Raspberry,
        Garnet,
        Dirt,
        Cream,
        Cottoncandy,
        Driftwood,
        Auburn,
        Buttercup,
        Strawberry,
        Vermilion,
        Fuchsia,
        Cantaloupe,
        Sunset,
        Crocodile,
        Twilight,
        Nightshade,
        Eldritch,
        Shale
    }

    public enum Age
    {
        Hatchling,
        Adult
    }
}