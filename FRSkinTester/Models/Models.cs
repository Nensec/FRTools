using FRSkinTester.Infrastructure.DataModels;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FRSkinTester.Models
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
        MultiGaze,
        Primal,
        Glowing,
        DarkSclera,
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
        Lionfish
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

    public class UploadModelPost
    {
        [Display(Name = "Name of your skin")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Dragon type")]
        public DragonType DragonType { get; set; }
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        [Display(Name = "Skin file")]
        [Required]
        public HttpPostedFileBase Skin { get; set; }
    }

    public class UploadModelPostViewModel
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }

    public class PreviewModelGet
    {
        public string SkinId { get; set; }
    }

    public class PreviewModelBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string SkinId { get; set; }
        public string PreviewUrl { get; set; }
        public double? Coverage { get; set; }
    }

    public class PreviewModelPost : PreviewModelBase
    {
        [Display(Name = "Your dragon id")]
        [Required]
        public int DragonId { get; set; }
    }

    public class PreviewScryerModelPost : PreviewModelBase
    {
        [Display(Name = "Scry image URL")]
        [Required]
        public string ScryerUrl { get; set; }
    }

    public class PreviewModelPostViewModel
    {
        public string ImageResultUrl { get; set; }
        public string SkinId { get; set; }
        public Dragon Dragon { get; set; }
    }

    public class DeleteSkinPost
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }

    public class Dragon
    {
        public Gender Gender { get; set; }
        public DragonType DragonType { get; set; }
        public Element Element { get; set; }
        public EyeType EyeType { get; set; }
        public BodyGene BodyGene { get; set; }
        public Color BodyColor { get; set; }
        public WingGene WingGene { get; set; }
        public Color WingColor { get; set; }
        public TertiaryGene TertiaryGene { get; set; }
        public Color TertiaryColor { get; set; }

        public static Dragon FromString(string data)
        {
            var split = data.Split('_');
            var dragon = new Dragon
            {
                Gender = (Gender)int.Parse(split[0]),
                DragonType = (DragonType)int.Parse(split[1])
            };

            if (split.Length > 2)
                dragon.Element = (Element)int.Parse(split[2]);
            if (split.Length > 3)
                dragon.EyeType = (EyeType)int.Parse(split[3]);
            if (split.Length > 4)
                dragon.BodyGene = (BodyGene)int.Parse(split[4]);
            if (split.Length > 5)
                dragon.WingGene = (WingGene)int.Parse(split[5]);
            if (split.Length > 6)
                dragon.TertiaryGene = (TertiaryGene)int.Parse(split[6]);
            if (split.Length > 7)
                dragon.BodyColor = (Color)int.Parse(split[7]);
            if (split.Length > 8)
                dragon.WingColor = (Color)int.Parse(split[8]);
            if (split.Length > 9)
                dragon.TertiaryColor = (Color)int.Parse(split[9]);

            return dragon;
        }

        public override string ToString() => $"{(int)Gender}_{(int)DragonType}_{(int)Element}_{(int)EyeType}_{(int)BodyGene}_{(int)WingGene}_{(int)TertiaryGene}_{(int)BodyColor}_{(int)WingColor}_{(int)TertiaryColor}";
    }

    public class ManageModelGet
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }

    public class ManageModelViewModel
    {
        public Skin Skin { get; set; }
        public string PreviewUrl { get; set; }
    }

    public class ManageModelPost
    {
        [Display(Name = "Name of your skin")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Dragon type")]
        public DragonType DragonType { get; set; }
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }
}