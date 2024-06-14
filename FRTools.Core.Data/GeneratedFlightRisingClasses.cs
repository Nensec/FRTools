using System.ComponentModel;
using Newtonsoft.Json;

namespace FRTools.Core.Data
{
	public enum DragonType
	{
		[Description("Fae")]
		Fae = 1,
		[Description("Guardian")]
		Guardian = 2,
		[Description("Mirror")]
		Mirror = 3,
		[Description("Pearlcatcher")]
		Pearlcatcher = 4,
		[Description("Ridgeback")]
		Ridgeback = 5,
		[Description("Tundra")]
		Tundra = 6,
		[Description("Spiral")]
		Spiral = 7,
		[Description("Imperial")]
		Imperial = 8,
		[Description("Snapper")]
		Snapper = 9,
		[Description("Wildclaw")]
		Wildclaw = 10,
		[Description("Nocturne")]
		Nocturne = 11,
		[Description("Coatl")]
		Coatl = 12,
		[Description("Skydancer")]
		Skydancer = 13,
		[Description("Bogsneak")]
		Bogsneak = 14,
		[Description("Obelisk")]
		Obelisk = 15,
		[Description("Fathom")]
		Fathom = 16,
		[Description("Gaoler")]
		Gaoler = 17,
		[Description("Banescale")]
		Banescale = 18,
		[Description("Veilspun")]
		Veilspun = 19,
		[Description("Aberration")]
		Aberration = 20,
		[Description("Undertide")]
		Undertide = 21,
		[Description("Aether")]
		Aether = 22,
		[Description("Sandsurge")]
		Sandsurge = 23,
		[Description("Auraboa")]
		Auraboa = 24,
		[Description("Dusthide")]
		Dusthide = 25,
	}

	public enum Gender
	{
		[Description("Male")]
		Male = 0,
		[Description("Female")]
		Female = 1,
	}

	public enum Age
	{
		[Description("Hatchling")]
		Hatchling = 0,
		[Description("Dragon")]
		Dragon = 1,
	}

	public enum Element
	{
		[Description("Earth")]
		Earth = 1,
		[Description("Plague")]
		Plague = 2,
		[Description("Wind")]
		Wind = 3,
		[Description("Water")]
		Water = 4,
		[Description("Lightning")]
		Lightning = 5,
		[Description("Ice")]
		Ice = 6,
		[Description("Shadow")]
		Shadow = 7,
		[Description("Light")]
		Light = 8,
		[Description("Arcane")]
		Arcane = 9,
		[Description("Nature")]
		Nature = 10,
		[Description("Fire")]
		Fire = 11,
	}

	public enum Flight
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
	}

	public enum EyeType
	{
		[Description("Common")]
		Common = 0,
		[Description("Uncommon")]
		Uncommon = 1,
		[Description("Unusual")]
		Unusual = 2,
		[Description("Rare")]
		Rare = 3,
		[Description("Faceted")]
		Faceted = 4,
		[Description("Multi-Gaze")]
		MultiGaze = 5,
		[Description("Primal")]
		Primal = 6,
		[Description("Glowing")]
		Glowing = 7,
		[Description("Dark Sclera")]
		DarkSclera = 8,
		[Description("Goat")]
		Goat = 9,
		[Description("Swirl")]
		Swirl = 10,
		[Description("Innocent")]
		Innocent = 11,
		[Description("Pastel")]
		Pastel = 12,
		[Description("Bright")]
		Bright = 13,
		[Description("Button")]
		Button = 14,
		[Description("Faded")]
		Faded = 15,
		[Description("Dark")]
		Dark = 16,
	}

	public enum Color
	{
		[Color("#FFFDEA", 0)]
		Maize = 1,
		[Color("#FFFFFF", 3)]
		White = 2,
		[Color("#EBEFFF", 5)]
		Ice = 3,
		[Color("#C8BECE", 7)]
		Platinum = 4,
		[Color("#BBBABF", 8)]
		Silver = 5,
		[Color("#808080", 10)]
		Grey = 6,
		[Color("#545454", 16)]
		Charcoal = 7,
		[Color("#4B4946", 17)]
		Coal = 8,
		[Color("#333333", 19)]
		Black = 9,
		[Color("#1D2224", 20)]
		Obsidian = 10,
		[Color("#252735", 22)]
		Midnight = 11,
		[Color("#3A2E44", 23)]
		Shadow = 12,
		[Color("#6E235D", 25)]
		Mulberry = 13,
		[Color("#8F7C8B", 28)]
		Thistle = 14,
		[Color("#CCA4E0", 31)]
		Lavender = 15,
		[Color("#A261CF", 33)]
		Purple = 16,
		[Color("#643F9C", 37)]
		Violet = 17,
		[Color("#4D2C89", 39)]
		Royal = 18,
		[Color("#757ADB", 42)]
		Storm = 19,
		[Color("#212B5F", 46)]
		Navy = 20,
		[Color("#324BA9", 49)]
		Blue = 21,
		[Color("#6392DF", 52)]
		Splash = 22,
		[Color("#AEC8FF", 54)]
		Sky = 23,
		[Color("#7895C1", 55)]
		Stonewash = 24,
		[Color("#556979", 57)]
		Steel = 25,
		[Color("#2F4557", 58)]
		Denim = 26,
		[Color("#0A3D67", 61)]
		Azure = 27,
		[Color("#0086CE", 62)]
		Caribbean = 28,
		[Color("#2B768F", 63)]
		Teal = 29,
		[Color("#72C4C4", 67)]
		Aqua = 30,
		[Color("#B2E2BD", 71)]
		Seafoam = 31,
		[Color("#61AB89", 73)]
		Jade = 32,
		[Color("#20603F", 77)]
		Emerald = 33,
		[Color("#1E361A", 79)]
		Jungle = 34,
		[Color("#425035", 81)]
		Forest = 35,
		[Color("#687F67", 84)]
		Swamp = 36,
		[Color("#567C34", 85)]
		Avocado = 37,
		[Color("#629C3F", 86)]
		Green = 38,
		[Color("#A5E32D", 90)]
		Leaf = 39,
		[Color("#A9A832", 95)]
		Spring = 40,
		[Color("#BEA55D", 100)]
		Goldenrod = 41,
		[Color("#FFE63B", 103)]
		Lemon = 42,
		[Color("#FFEC80", 106)]
		Banana = 43,
		[Color("#FFD297", 109)]
		Ivory = 44,
		[Color("#E8AF49", 111)]
		Gold = 45,
		[Color("#FA912B", 114)]
		Sunshine = 46,
		[Color("#D5602B", 119)]
		Orange = 47,
		[Color("#EF5C23", 123)]
		Fire = 48,
		[Color("#FF7360", 125)]
		Tangerine = 49,
		[Color("#B27749", 128)]
		Sand = 50,
		[Color("#CABBA2", 130)]
		Beige = 51,
		[Color("#827A64", 131)]
		Stone = 52,
		[Color("#564D48", 133)]
		Slate = 53,
		[Color("#5A4534", 140)]
		Soil = 54,
		[Color("#8E5B3F", 144)]
		Brown = 55,
		[Color("#563012", 145)]
		Chocolate = 56,
		[Color("#8B3220", 148)]
		Rust = 57,
		[Color("#BA311C", 149)]
		Tomato = 58,
		[Color("#850012", 153)]
		Crimson = 59,
		[Color("#451717", 156)]
		Blood = 60,
		[Color("#652127", 157)]
		Maroon = 61,
		[Color("#C1272D", 159)]
		Red = 62,
		[Color("#B13A3A", 162)]
		Carmine = 63,
		[Color("#CC6F6F", 164)]
		Coral = 64,
		[Color("#E934AA", 168)]
		Magenta = 65,
		[Color("#E77FBF", 173)]
		Pink = 66,
		[Color("#FFD6F6", 175)]
		Rose = 67,
		[Color("#9777BD", 32)]
		Heather = 68,
		[Color("#D950FF", 34)]
		Orchid = 69,
		[Color("#342B25", 18)]
		Oilslick = 70,
		[Color("#0D095B", 45)]
		Sapphire = 71,
		[Color("#4D0F28", 171)]
		Wine = 72,
		[Color("#9C4875", 172)]
		Mauve = 73,
		[Color("#D8D7D8", 4)]
		Moon = 74,
		[Color("#FFB43B", 113)]
		Marigold = 75,
		[Color("#C49A70", 129)]
		Tan = 76,
		[Color("#C05A39", 126)]
		Cinnamon = 77,
		[Color("#148E67", 74)]
		Spearmint = 78,
		[Color("#99FF9C", 88)]
		Mantis = 79,
		[Color("#236925", 78)]
		Shamrock = 80,
		[Color("#1D2715", 80)]
		Hunter = 81,
		[Color("#535195", 41)]
		Iris = 82,
		[Color("#B2560D", 120)]
		Bronze = 83,
		[Color("#FF8400", 115)]
		Saffron = 84,
		[Color("#FBE9F8", 176)]
		Pearl = 85,
		[Color("#CD000E", 151)]
		Ruby = 86,
		[Color("#8B272C", 158)]
		Berry = 87,
		[Color("#725639", 141)]
		Hickory = 88,
		[Color("#00FFF0", 65)]
		Cyan = 89,
		[Color("#1C51E7", 48)]
		Ultramarine = 90,
		[Color("#9494A9", 11)]
		Smoke = 91,
		[Color("#853390", 26)]
		Plum = 92,
		[Color("#D1B300", 102)]
		Honey = 93,
		[Color("#A44B28", 147)]
		Copper = 94,
		[Color("#6D665A", 132)]
		Taupe = 95,
		[Color("#0D1E24", 59)]
		Abyss = 96,
		[Color("#D8D6CD", 2)]
		Antique = 97,
		[Color("#535264", 12)]
		Gloom = 98,
		[Color("#9AEAEF", 66)]
		Robin = 99,
		[Color("#8BBBB2", 69)]
		Spruce = 100,
		[Color("#8ECD55", 89)]
		Pear = 101,
		[Color("#D0E672", 92)]
		Honeydew = 102,
		[Color("#C18E1B", 101)]
		Amber = 103,
		[Color("#F9E255", 104)]
		Yellow = 104,
		[Color("#FFB576", 117)]
		Peach = 105,
		[Color("#603F3D", 137)]
		Clay = 106,
		[Color("#9A534D", 163)]
		Brick = 107,
		[Color("#B23B07", 121)]
		Terracotta = 108,
		[Color("#EAA9FF", 174)]
		Bubblegum = 109,
		[Color("#EBE7AE", 107)]
		Sanddollar = 110,
		[Color("#332B65", 40)]
		Eggplant = 111,
		[Color("#2D237A", 44)]
		Indigo = 112,
		[Color("#7ECE73", 87)]
		Fern = 113,
		[Color("#993BD0", 35)]
		Amethyst = 114,
		[Color("#7E7745", 99)]
		Moss = 115,
		[Color("#AA0024", 152)]
		Cherry = 116,
		[Color("#00B4D6", 64)]
		Cerulean = 117,
		[Color("#413C3F", 13)]
		Lead = 118,
		[Color("#724E7B", 27)]
		Wisteria = 119,
		[Color("#DB518D", 167)]
		Watermelon = 120,
		[Color("#2E0002", 155)]
		Sanguine = 121,
		[Color("#90532B", 143)]
		Ginger = 122,
		[Color("#697135", 97)]
		Olive = 123,
		[Color("#855C32", 142)]
		Tarnish = 124,
		[Color("#E2FFE6", 70)]
		Pistachio = 125,
		[Color("#444F69", 56)]
		Overcast = 126,
		[Color("#4B294F", 24)]
		Blackberry = 127,
		[Color("#F7FF6F", 105)]
		Grapefruit = 128,
		[Color("#626268", 15)]
		Flint = 129,
		[Color("#C6FF00", 91)]
		Radioactive = 130,
		[Color("#E0DFFF", 6)]
		Orca = 131,
		[Color("#A22929", 161)]
		Cerise = 132,
		[Color("#FF5500", 122)]
		Carrot = 133,
		[Color("#1F4739", 76)]
		Peacock = 134,
		[Color("#4866D5", 50)]
		Periwinkle = 135,
		[Color("#003484", 47)]
		Cobalt = 136,
		[Color("#A593B0", 29)]
		Fog = 137,
		[Color("#57372C", 138)]
		Sable = 138,
		[Color("#FDE9AE", 108)]
		Flaxen = 139,
		[Color("#D1B046", 112)]
		Metals = 140,
		[Color("#005E48", 75)]
		Thicket = 141,
		[Color("#4B4420", 98)]
		Murk = 142,
		[Color("#977B6C", 135)]
		Latte = 143,
		[Color("#E8FFB5", 93)]
		Peridot = 144,
		[Color("#75A8FF", 53)]
		Cornflower = 145,
		[Color("#9C9C9E", 9)]
		Dust = 146,
		[Color("#570FC0", 38)]
		Grape = 147,
		[Color("#2B84FF", 51)]
		Lapis = 148,
		[Color("#3AA0A1", 68)]
		Turquoise = 149,
		[Color("#E1CEFF", 30)]
		Mist = 150,
		[Color("#0B2D46", 60)]
		Phthalo = 151,
		[Color("#9AFFC7", 72)]
		Mint = 152,
		[Color("#97AF8B", 83)]
		Algae = 153,
		[Color("#51684C", 82)]
		Camo = 154,
		[Color("#B4CD3C", 94)]
		Chartreuse = 155,
		[Color("#C67047", 127)]
		Caramel = 156,
		[Color("#2F1E1A", 139)]
		Umber = 157,
		[Color("#FF6840", 124)]
		Pumpkin = 158,
		[Color("#FFA2A2", 165)]
		Blush = 159,
		[Color("#8A0249", 170)]
		Raspberry = 160,
		[Color("#5B0F14", 154)]
		Garnet = 161,
		[Color("#76483F", 136)]
		Dirt = 162,
		[Color("#FFEFDC", 1)]
		Cream = 163,
		[Color("#EB7997", 166)]
		Cottoncandy = 164,
		[Color("#766359", 134)]
		Driftwood = 165,
		[Color("#7B3C1D", 146)]
		Auburn = 166,
		[Color("#F6BF6B", 110)]
		Buttercup = 167,
		[Color("#DE3235", 160)]
		Strawberry = 168,
		[Color("#E22D17", 150)]
		Vermilion = 169,
		[Color("#EC0089", 169)]
		Fuchsia = 170,
		[Color("#FF984F", 118)]
		Cantaloupe = 171,
		[Color("#FFA248", 116)]
		Sunset = 172,
		[Color("#828335", 96)]
		Crocodile = 173,
		[Color("#474AA0", 43)]
		Twilight = 174,
		[Color("#782EB2", 36)]
		Nightshade = 175,
		[Color("#252A25", 21)]
		Eldritch = 176,
		[Color("#4D4850", 14)]
		Shale = 177,
	}

	public enum BodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Iridescent")]
		Iridescent = 1,
		[Description("Tiger")]
		Tiger = 2,
		[Description("Clown")]
		Clown = 3,
		[Description("Speckle")]
		Speckle = 4,
		[Description("Ripple")]
		Ripple = 5,
		[Description("Bar")]
		Bar = 6,
		[Description("Crystal")]
		Crystal = 7,
		[Description("Vipera")]
		Vipera = 8,
		[Description("Piebald")]
		Piebald = 9,
		[Description("Cherub")]
		Cherub = 10,
		[Description("Poison")]
		Poison = 11,
		[Description("Giraffe")]
		Giraffe = 12,
		[Description("Petals")]
		Petals = 13,
		[Description("Jupiter")]
		Jupiter = 14,
		[Description("Skink")]
		Skink = 15,
		[Description("Falcon")]
		Falcon = 16,
		[Description("Metallic")]
		Metallic = 17,
		[Description("Savannah")]
		Savannah = 18,
		[Description("Jaguar")]
		Jaguar = 19,
		[Description("Wasp")]
		Wasp = 20,
		[Description("Tapir")]
		Tapir = 21,
		[Description("Pinstripe")]
		Pinstripe = 22,
		[Description("Python")]
		Python = 23,
		[Description("Starmap")]
		Starmap = 24,
		[Description("Lionfish")]
		Lionfish = 25,
		[Description("Laced")]
		Laced = 26,
		[Description("Leopard")]
		Leopard = 40,
		[Description("Slime")]
		Slime = 41,
		[Description("Fade")]
		Fade = 42,
		[Description("Swirl")]
		Swirl = 57,
		[Description("Mosaic")]
		Mosaic = 58,
		[Description("Stitched")]
		Stitched = 59,
		[Description("Flaunt")]
		Flaunt = 82,
		[Description("Ribbon")]
		Ribbon = 84,
		[Description("Pharaoh")]
		Pharaoh = 87,
		[Description("Ground")]
		Ground = 88,
		[Description("Boulder")]
		Boulder = 110,
		[Description("Tide")]
		Tide = 114,
		[Description("Fern")]
		Fern = 136,
		[Description("Harlequin")]
		Harlequin = 170,
		[Description("Cinder")]
		Cinder = 213,
		[Description("Boa")]
		Boa = 232,
		[Description("Chrysocolla")]
		Chrysocolla = 237,
		[Description("Orb")]
		Orb = 283,
	}
	
	public enum SandsurgeBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Arapaima (Sandsurge)")]
		Arapaima = 194,
		[Description("Boa (Sandsurge)")]
		Boa = 189,
		[Description("Boulder (Sandsurge)")]
		Boulder = 188,
		[Description("Cherub (Sandsurge)")]
		Cherub = 190,
		[Description("Chrysocolla (Sandsurge)")]
		Chrysocolla = 195,
		[Description("Cinder (Sandsurge)")]
		Cinder = 217,
		[Description("Clown (Sandsurge)")]
		Clown = 196,
		[Description("Fade (Sandsurge)")]
		Fade = 191,
		[Description("Flaunt (Sandsurge)")]
		Flaunt = 193,
		[Description("Ground (Sandsurge)")]
		Ground = 186,
		[Description("Harlequin (Sandsurge)")]
		Harlequin = 192,
		[Description("Jaguar (Sandsurge)")]
		Jaguar = 198,
		[Description("Jupiter (Sandsurge)")]
		Jupiter = 197,
		[Description("Lionfish (Sandsurge)")]
		Lionfish = 199,
		[Description("Mosaic (Sandsurge)")]
		Mosaic = 200,
		[Description("Orb (Sandsurge)")]
		Orb = 284,
		[Description("Piebald (Sandsurge)")]
		Piebald = 201,
		[Description("Pinstripe (Sandsurge)")]
		Pinstripe = 202,
		[Description("Python (Sandsurge)")]
		Python = 203,
		[Description("Rattlesnake (Sandsurge)")]
		Rattlesnake = 206,
		[Description("Sailfish (Sandsurge)")]
		Sailfish = 212,
		[Description("Savannah (Sandsurge)")]
		Savannah = 204,
		[Description("Slime (Sandsurge)")]
		Slime = 208,
		[Description("Swirl (Sandsurge)")]
		Swirl = 209,
		[Description("Tapir (Sandsurge)")]
		Tapir = 210,
		[Description("Tiger (Sandsurge)")]
		Tiger = 211,
		[Description("Wasp (Sandsurge)")]
		Wasp = 207,
		[Description("Wrought (Sandsurge)")]
		Wrought = 187,
	}
	
	public enum DusthideBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Arc (Dusthide)")]
		Arc = 306,
		[Description("Bar (Dusthide)")]
		Bar = 292,
		[Description("Checkers (Dusthide)")]
		Checkers = 293,
		[Description("Cinder (Dusthide)")]
		Cinder = 295,
		[Description("Display (Dusthide)")]
		Display = 317,
		[Description("Fade (Dusthide)")]
		Fade = 296,
		[Description("Falcon (Dusthide)")]
		Falcon = 301,
		[Description("Giraffe (Dusthide)")]
		Giraffe = 297,
		[Description("Ground (Dusthide)")]
		Ground = 299,
		[Description("Harlequin (Dusthide)")]
		Harlequin = 298,
		[Description("Jupiter (Dusthide)")]
		Jupiter = 300,
		[Description("Laced (Dusthide)")]
		Laced = 302,
		[Description("Orb (Dusthide)")]
		Orb = 303,
		[Description("Petals (Dusthide)")]
		Petals = 304,
		[Description("Petrified (Dusthide)")]
		Petrified = 309,
		[Description("Piebald (Dusthide)")]
		Piebald = 305,
		[Description("Pinstripe (Dusthide)")]
		Pinstripe = 308,
		[Description("Ribbon (Dusthide)")]
		Ribbon = 311,
		[Description("Ripple (Dusthide)")]
		Ripple = 312,
		[Description("Sailfish (Dusthide)")]
		Sailfish = 313,
		[Description("Savannah (Dusthide)")]
		Savannah = 314,
		[Description("Skink (Dusthide)")]
		Skink = 315,
		[Description("Speckle (Dusthide)")]
		Speckle = 294,
		[Description("Strike (Dusthide)")]
		Strike = 205,
		[Description("Varnish (Dusthide)")]
		Varnish = 316,
		[Description("Wasp (Dusthide)")]
		Wasp = 310,
		[Description("Wrought (Dusthide)")]
		Wrought = 307,
	}
	
	public enum VeilspunBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Arc (Veilspun)")]
		Arc = 70,
		[Description("Bar (Veilspun)")]
		Bar = 145,
		[Description("Boa (Veilspun)")]
		Boa = 236,
		[Description("Boulder (Veilspun)")]
		Boulder = 290,
		[Description("Bright (Veilspun)")]
		Bright = 69,
		[Description("Cinder (Veilspun)")]
		Cinder = 219,
		[Description("Clown (Veilspun)")]
		Clown = 76,
		[Description("Crystal (Veilspun)")]
		Crystal = 146,
		[Description("Fade (Veilspun)")]
		Fade = 60,
		[Description("Falcon (Veilspun)")]
		Falcon = 139,
		[Description("Fern (Veilspun)")]
		Fern = 138,
		[Description("Giraffe (Veilspun)")]
		Giraffe = 83,
		[Description("Jupiter (Veilspun)")]
		Jupiter = 64,
		[Description("Laced (Veilspun)")]
		Laced = 61,
		[Description("Leopard (Veilspun)")]
		Leopard = 142,
		[Description("Orb (Veilspun)")]
		Orb = 285,
		[Description("Petals (Veilspun)")]
		Petals = 143,
		[Description("Poison (Veilspun)")]
		Poison = 140,
		[Description("Shell (Veilspun)")]
		Shell = 71,
		[Description("Skink (Veilspun)")]
		Skink = 67,
		[Description("Slime (Veilspun)")]
		Slime = 141,
		[Description("Speckle (Veilspun)")]
		Speckle = 144,
		[Description("Sphinxmoth (Veilspun)")]
		Sphinxmoth = 72,
		[Description("Starmap (Veilspun)")]
		Starmap = 65,
		[Description("Stitched (Veilspun)")]
		Stitched = 66,
		[Description("Tapir (Veilspun)")]
		Tapir = 62,
		[Description("Vipera (Veilspun)")]
		Vipera = 63,
		[Description("Wasp (Veilspun)")]
		Wasp = 68,
	}
	
	public enum AberrationBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bar (Aberration)")]
		Bar = 89,
		[Description("Boa (Aberration)")]
		Boa = 233,
		[Description("Boulder (Aberration)")]
		Boulder = 220,
		[Description("Cherub (Aberration)")]
		Cherub = 221,
		[Description("Cinder (Aberration)")]
		Cinder = 214,
		[Description("Clown (Aberration)")]
		Clown = 222,
		[Description("Crystal (Aberration)")]
		Crystal = 91,
		[Description("Diamond (Aberration)")]
		Diamond = 93,
		[Description("Fade (Aberration)")]
		Fade = 90,
		[Description("Falcon (Aberration)")]
		Falcon = 92,
		[Description("Fern (Aberration)")]
		Fern = 223,
		[Description("Flaunt (Aberration)")]
		Flaunt = 112,
		[Description("Giraffe (Aberration)")]
		Giraffe = 94,
		[Description("Ground (Aberration)")]
		Ground = 97,
		[Description("Harlequin (Aberration)")]
		Harlequin = 224,
		[Description("Jaguar (Aberration)")]
		Jaguar = 99,
		[Description("Leopard (Aberration)")]
		Leopard = 225,
		[Description("Lionfish (Aberration)")]
		Lionfish = 100,
		[Description("Orb (Aberration)")]
		Orb = 102,
		[Description("Pharaoh (Aberration)")]
		Pharaoh = 101,
		[Description("Pinstripe (Aberration)")]
		Pinstripe = 226,
		[Description("Poison (Aberration)")]
		Poison = 227,
		[Description("Ribbon (Aberration)")]
		Ribbon = 105,
		[Description("Ripple (Aberration)")]
		Ripple = 228,
		[Description("Savannah (Aberration)")]
		Savannah = 103,
		[Description("Skink (Aberration)")]
		Skink = 229,
		[Description("Slime (Aberration)")]
		Slime = 106,
		[Description("Speckle (Aberration)")]
		Speckle = 98,
		[Description("Starmap (Aberration)")]
		Starmap = 230,
		[Description("Stitched (Aberration)")]
		Stitched = 107,
		[Description("Swirl (Aberration)")]
		Swirl = 104,
		[Description("Tapir (Aberration)")]
		Tapir = 95,
		[Description("Tide (Aberration)")]
		Tide = 231,
		[Description("Vipera (Aberration)")]
		Vipera = 96,
		[Description("Wasp (Aberration)")]
		Wasp = 108,
	}
	
	public enum AetherBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bar (Aether)")]
		Bar = 150,
		[Description("Boa (Aether)")]
		Boa = 234,
		[Description("Boulder (Aether)")]
		Boulder = 151,
		[Description("Candy (Aether)")]
		Candy = 167,
		[Description("Cinder (Aether)")]
		Cinder = 163,
		[Description("Clown (Aether)")]
		Clown = 169,
		[Description("Fade (Aether)")]
		Fade = 148,
		[Description("Flaunt (Aether)")]
		Flaunt = 149,
		[Description("Jaguar (Aether)")]
		Jaguar = 152,
		[Description("Jupiter (Aether)")]
		Jupiter = 153,
		[Description("Laced (Aether)")]
		Laced = 156,
		[Description("Lionfish (Aether)")]
		Lionfish = 158,
		[Description("Metallic (Aether)")]
		Metallic = 157,
		[Description("Mosaic (Aether)")]
		Mosaic = 155,
		[Description("Orb (Aether)")]
		Orb = 288,
		[Description("Petals (Aether)")]
		Petals = 154,
		[Description("Piebald (Aether)")]
		Piebald = 159,
		[Description("Python (Aether)")]
		Python = 160,
		[Description("Skink (Aether)")]
		Skink = 161,
		[Description("Spool (Aether)")]
		Spool = 162,
		[Description("Starmap (Aether)")]
		Starmap = 168,
		[Description("Stitched (Aether)")]
		Stitched = 165,
		[Description("Tide (Aether)")]
		Tide = 164,
		[Description("Twinkle (Aether)")]
		Twinkle = 166,
	}
	
	public enum AuraboaBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bar (Auraboa)")]
		Bar = 238,
		[Description("Boa (Auraboa)")]
		Boa = 239,
		[Description("Boulder (Auraboa)")]
		Boulder = 240,
		[Description("Caterpillar (Auraboa)")]
		Caterpillar = 241,
		[Description("Fade (Auraboa)")]
		Fade = 242,
		[Description("Falcon (Auraboa)")]
		Falcon = 243,
		[Description("Fern (Auraboa)")]
		Fern = 244,
		[Description("Flaunt (Auraboa)")]
		Flaunt = 245,
		[Description("Giraffe (Auraboa)")]
		Giraffe = 246,
		[Description("Harlequin (Auraboa)")]
		Harlequin = 247,
		[Description("Jaguar (Auraboa)")]
		Jaguar = 248,
		[Description("Laced (Auraboa)")]
		Laced = 249,
		[Description("Love (Auraboa)")]
		Love = 279,
		[Description("Metallic (Auraboa)")]
		Metallic = 250,
		[Description("Mochlus (Auraboa)")]
		Mochlus = 251,
		[Description("Mosaic (Auraboa)")]
		Mosaic = 252,
		[Description("Orb (Auraboa)")]
		Orb = 253,
		[Description("Piebald (Auraboa)")]
		Piebald = 254,
		[Description("Python (Auraboa)")]
		Python = 255,
		[Description("Rattlesnake (Auraboa)")]
		Rattlesnake = 256,
		[Description("Ripple (Auraboa)")]
		Ripple = 257,
		[Description("Starmap (Auraboa)")]
		Starmap = 263,
		[Description("Tapir (Auraboa)")]
		Tapir = 258,
		[Description("Tiger (Auraboa)")]
		Tiger = 259,
		[Description("Varnish (Auraboa)")]
		Varnish = 261,
		[Description("Vipera (Auraboa)")]
		Vipera = 260,
		[Description("Wicker (Auraboa)")]
		Wicker = 262,
	}
	
	public enum BanescaleBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bar (Banescale)")]
		Bar = 181,
		[Description("Boa (Banescale)")]
		Boa = 235,
		[Description("Boulder (Banescale)")]
		Boulder = 291,
		[Description("Candycane (Banescale)")]
		Candycane = 55,
		[Description("Cherub (Banescale)")]
		Cherub = 43,
		[Description("Chevron (Banescale)")]
		Chevron = 54,
		[Description("Cinder (Banescale)")]
		Cinder = 215,
		[Description("Clown (Banescale)")]
		Clown = 75,
		[Description("Crystal (Banescale)")]
		Crystal = 182,
		[Description("Fade (Banescale)")]
		Fade = 85,
		[Description("Falcon (Banescale)")]
		Falcon = 80,
		[Description("Fern (Banescale)")]
		Fern = 137,
		[Description("Giraffe (Banescale)")]
		Giraffe = 81,
		[Description("Jaguar (Banescale)")]
		Jaguar = 44,
		[Description("Laced (Banescale)")]
		Laced = 48,
		[Description("Leopard (Banescale)")]
		Leopard = 109,
		[Description("Marble (Banescale)")]
		Marble = 47,
		[Description("Metallic (Banescale)")]
		Metallic = 49,
		[Description("Orb (Banescale)")]
		Orb = 287,
		[Description("Petals (Banescale)")]
		Petals = 51,
		[Description("Pharaoh (Banescale)")]
		Pharaoh = 185,
		[Description("Pinstripe (Banescale)")]
		Pinstripe = 45,
		[Description("Poison (Banescale)")]
		Poison = 53,
		[Description("Ragged (Banescale)")]
		Ragged = 56,
		[Description("Ribbon (Banescale)")]
		Ribbon = 183,
		[Description("Ripple (Banescale)")]
		Ripple = 79,
		[Description("Savannah (Banescale)")]
		Savannah = 50,
		[Description("Skink (Banescale)")]
		Skink = 52,
		[Description("Speckle (Banescale)")]
		Speckle = 147,
		[Description("Tapir (Banescale)")]
		Tapir = 74,
		[Description("Tide (Banescale)")]
		Tide = 184,
		[Description("Tiger (Banescale)")]
		Tiger = 46,
	}
	
	public enum GaolerBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bar (Gaoler)")]
		Bar = 34,
		[Description("Boa (Gaoler)")]
		Boa = 171,
		[Description("Boulder (Gaoler)")]
		Boulder = 289,
		[Description("Cinder (Gaoler)")]
		Cinder = 216,
		[Description("Clown (Gaoler)")]
		Clown = 77,
		[Description("Crystal (Gaoler)")]
		Crystal = 37,
		[Description("Fade (Gaoler)")]
		Fade = 86,
		[Description("Falcon (Gaoler)")]
		Falcon = 30,
		[Description("Flaunt (Gaoler)")]
		Flaunt = 111,
		[Description("Giraffe (Gaoler)")]
		Giraffe = 27,
		[Description("Jaguar (Gaoler)")]
		Jaguar = 33,
		[Description("Laced (Gaoler)")]
		Laced = 73,
		[Description("Leopard (Gaoler)")]
		Leopard = 173,
		[Description("Mosaic (Gaoler)")]
		Mosaic = 38,
		[Description("Orb (Gaoler)")]
		Orb = 286,
		[Description("Phantom (Gaoler)")]
		Phantom = 39,
		[Description("Piebald (Gaoler)")]
		Piebald = 31,
		[Description("Pinstripe (Gaoler)")]
		Pinstripe = 32,
		[Description("Poison (Gaoler)")]
		Poison = 174,
		[Description("Ribbon (Gaoler)")]
		Ribbon = 175,
		[Description("Ripple (Gaoler)")]
		Ripple = 78,
		[Description("Savannah (Gaoler)")]
		Savannah = 176,
		[Description("Shaggy (Gaoler)")]
		Shaggy = 29,
		[Description("Skink (Gaoler)")]
		Skink = 177,
		[Description("Slime (Gaoler)")]
		Slime = 178,
		[Description("Stitched (Gaoler)")]
		Stitched = 180,
		[Description("Swirl (Gaoler)")]
		Swirl = 179,
		[Description("Tapir (Gaoler)")]
		Tapir = 35,
		[Description("Tide (Gaoler)")]
		Tide = 172,
		[Description("Tiger (Gaoler)")]
		Tiger = 36,
		[Description("Wasp (Gaoler)")]
		Wasp = 28,
	}
	
	public enum UndertideBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bar (Undertide)")]
		Bar = 117,
		[Description("Boa (Undertide)")]
		Boa = 128,
		[Description("Boulder (Undertide)")]
		Boulder = 135,
		[Description("Checkers (Undertide)")]
		Checkers = 127,
		[Description("Cherub (Undertide)")]
		Cherub = 119,
		[Description("Chrysocolla (Undertide)")]
		Chrysocolla = 264,
		[Description("Cinder (Undertide)")]
		Cinder = 218,
		[Description("Clown (Undertide)")]
		Clown = 265,
		[Description("Crystal (Undertide)")]
		Crystal = 118,
		[Description("Fade (Undertide)")]
		Fade = 115,
		[Description("Falcon (Undertide)")]
		Falcon = 122,
		[Description("Fern (Undertide)")]
		Fern = 266,
		[Description("Flaunt (Undertide)")]
		Flaunt = 267,
		[Description("Giraffe (Undertide)")]
		Giraffe = 123,
		[Description("Ground (Undertide)")]
		Ground = 268,
		[Description("Harlequin (Undertide)")]
		Harlequin = 280,
		[Description("Jaguar (Undertide)")]
		Jaguar = 269,
		[Description("Jupiter (Undertide)")]
		Jupiter = 270,
		[Description("Lionfish (Undertide)")]
		Lionfish = 126,
		[Description("Metallic (Undertide)")]
		Metallic = 278,
		[Description("Octopus (Undertide)")]
		Octopus = 133,
		[Description("Orb (Undertide)")]
		Orb = 271,
		[Description("Petals (Undertide)")]
		Petals = 281,
		[Description("Pharaoh (Undertide)")]
		Pharaoh = 120,
		[Description("Piebald (Undertide)")]
		Piebald = 272,
		[Description("Pinstripe (Undertide)")]
		Pinstripe = 121,
		[Description("Poison (Undertide)")]
		Poison = 131,
		[Description("Ribbon (Undertide)")]
		Ribbon = 124,
		[Description("Ripple (Undertide)")]
		Ripple = 130,
		[Description("Savannah (Undertide)")]
		Savannah = 129,
		[Description("Speckle (Undertide)")]
		Speckle = 132,
		[Description("Starmap (Undertide)")]
		Starmap = 273,
		[Description("Stitched (Undertide)")]
		Stitched = 282,
		[Description("Swirl (Undertide)")]
		Swirl = 134,
		[Description("Tapir (Undertide)")]
		Tapir = 274,
		[Description("Tide (Undertide)")]
		Tide = 113,
		[Description("Tiger (Undertide)")]
		Tiger = 275,
		[Description("Varnish (Undertide)")]
		Varnish = 276,
		[Description("Wasp (Undertide)")]
		Wasp = 125,
		[Description("Wolf (Undertide)")]
		Wolf = 116,
		[Description("Wrought (Undertide)")]
		Wrought = 277,
	}
	
	public enum AllBodyGene
	{
		[Description("Basic")]
		[Order(0)]
		[Gene(DragonType.Aberration, DragonType.Aether, DragonType.Auraboa, DragonType.Banescale, DragonType.Bogsneak, DragonType.Coatl, DragonType.Dusthide, DragonType.Fae, DragonType.Fathom, DragonType.Gaoler, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Sandsurge, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Undertide, DragonType.Veilspun, DragonType.Wildclaw)]
		Basic = 0,		
		[Description("Iridescent")]
		[Order(1)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Iridescent = 1,		
		[Description("Tiger")]
		[Order(2)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Tiger = 2,		
		[Description("Clown")]
		[Order(3)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Clown = 3,		
		[Description("Speckle")]
		[Order(4)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Speckle = 4,		
		[Description("Ripple")]
		[Order(5)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Ripple = 5,		
		[Description("Bar")]
		[Order(6)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Bar = 6,		
		[Description("Crystal")]
		[Order(7)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Crystal = 7,		
		[Description("Vipera")]
		[Order(8)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Vipera = 8,		
		[Description("Piebald")]
		[Order(9)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Piebald = 9,		
		[Description("Cherub")]
		[Order(10)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Cherub = 10,		
		[Description("Poison")]
		[Order(11)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Poison = 11,		
		[Description("Giraffe")]
		[Order(12)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Giraffe = 12,		
		[Description("Petals")]
		[Order(13)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Petals = 13,		
		[Description("Jupiter")]
		[Order(14)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Jupiter = 14,		
		[Description("Skink")]
		[Order(15)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Skink = 15,		
		[Description("Falcon")]
		[Order(16)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Falcon = 16,		
		[Description("Metallic")]
		[Order(17)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Metallic = 17,		
		[Description("Savannah")]
		[Order(18)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Savannah = 18,		
		[Description("Jaguar")]
		[Order(19)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Jaguar = 19,		
		[Description("Wasp")]
		[Order(20)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Wasp = 20,		
		[Description("Tapir")]
		[Order(21)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Tapir = 21,		
		[Description("Pinstripe")]
		[Order(22)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Pinstripe = 22,		
		[Description("Python")]
		[Order(23)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Python = 23,		
		[Description("Starmap")]
		[Order(24)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Starmap = 24,		
		[Description("Lionfish")]
		[Order(25)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Lionfish = 25,		
		[Description("Laced")]
		[Order(26)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Laced = 26,		
		[Description("Leopard")]
		[Order(27)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Leopard = 40,		
		[Description("Slime")]
		[Order(28)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Slime = 41,		
		[Description("Fade")]
		[Order(29)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Fade = 42,		
		[Description("Swirl")]
		[Order(30)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Swirl = 57,		
		[Description("Mosaic")]
		[Order(31)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Mosaic = 58,		
		[Description("Stitched")]
		[Order(32)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Stitched = 59,		
		[Description("Flaunt")]
		[Order(33)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Flaunt = 82,		
		[Description("Ribbon")]
		[Order(34)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Ribbon = 84,		
		[Description("Pharaoh")]
		[Order(35)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Pharaoh = 87,		
		[Description("Ground")]
		[Order(36)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Ground = 88,		
		[Description("Boulder")]
		[Order(37)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Boulder = 110,		
		[Description("Tide")]
		[Order(38)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Tide = 114,		
		[Description("Fern")]
		[Order(39)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Fern = 136,		
		[Description("Harlequin")]
		[Order(40)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Harlequin = 170,		
		[Description("Cinder")]
		[Order(41)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Cinder = 213,		
		[Description("Boa")]
		[Order(42)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Boa = 232,		
		[Description("Chrysocolla")]
		[Order(43)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Chrysocolla = 237,		
		[Description("Orb")]
		[Order(44)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Orb = 283,		
		[Description("Arapaima (Sandsurge)")]
		[Order(45)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Arapaima = 194,
		[Description("Boa (Sandsurge)")]
		[Order(46)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Boa = 189,
		[Description("Boulder (Sandsurge)")]
		[Order(47)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Boulder = 188,
		[Description("Cherub (Sandsurge)")]
		[Order(48)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Cherub = 190,
		[Description("Chrysocolla (Sandsurge)")]
		[Order(49)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Chrysocolla = 195,
		[Description("Cinder (Sandsurge)")]
		[Order(50)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Cinder = 217,
		[Description("Clown (Sandsurge)")]
		[Order(51)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Clown = 196,
		[Description("Fade (Sandsurge)")]
		[Order(52)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Fade = 191,
		[Description("Flaunt (Sandsurge)")]
		[Order(53)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Flaunt = 193,
		[Description("Ground (Sandsurge)")]
		[Order(54)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Ground = 186,
		[Description("Harlequin (Sandsurge)")]
		[Order(55)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Harlequin = 192,
		[Description("Jaguar (Sandsurge)")]
		[Order(56)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Jaguar = 198,
		[Description("Jupiter (Sandsurge)")]
		[Order(57)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Jupiter = 197,
		[Description("Lionfish (Sandsurge)")]
		[Order(58)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Lionfish = 199,
		[Description("Mosaic (Sandsurge)")]
		[Order(59)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Mosaic = 200,
		[Description("Orb (Sandsurge)")]
		[Order(60)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Orb = 284,
		[Description("Piebald (Sandsurge)")]
		[Order(61)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Piebald = 201,
		[Description("Pinstripe (Sandsurge)")]
		[Order(62)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Pinstripe = 202,
		[Description("Python (Sandsurge)")]
		[Order(63)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Python = 203,
		[Description("Rattlesnake (Sandsurge)")]
		[Order(64)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Rattlesnake = 206,
		[Description("Sailfish (Sandsurge)")]
		[Order(65)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Sailfish = 212,
		[Description("Savannah (Sandsurge)")]
		[Order(66)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Savannah = 204,
		[Description("Slime (Sandsurge)")]
		[Order(67)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Slime = 208,
		[Description("Swirl (Sandsurge)")]
		[Order(68)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Swirl = 209,
		[Description("Tapir (Sandsurge)")]
		[Order(69)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Tapir = 210,
		[Description("Tiger (Sandsurge)")]
		[Order(70)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Tiger = 211,
		[Description("Wasp (Sandsurge)")]
		[Order(71)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Wasp = 207,
		[Description("Wrought (Sandsurge)")]
		[Order(72)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Wrought = 187,
		[Description("Arc (Dusthide)")]
		[Order(73)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Arc = 306,
		[Description("Bar (Dusthide)")]
		[Order(74)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Bar = 292,
		[Description("Checkers (Dusthide)")]
		[Order(75)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Checkers = 293,
		[Description("Cinder (Dusthide)")]
		[Order(76)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Cinder = 295,
		[Description("Display (Dusthide)")]
		[Order(77)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Display = 317,
		[Description("Fade (Dusthide)")]
		[Order(78)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Fade = 296,
		[Description("Falcon (Dusthide)")]
		[Order(79)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Falcon = 301,
		[Description("Giraffe (Dusthide)")]
		[Order(80)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Giraffe = 297,
		[Description("Ground (Dusthide)")]
		[Order(81)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Ground = 299,
		[Description("Harlequin (Dusthide)")]
		[Order(82)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Harlequin = 298,
		[Description("Jupiter (Dusthide)")]
		[Order(83)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Jupiter = 300,
		[Description("Laced (Dusthide)")]
		[Order(84)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Laced = 302,
		[Description("Orb (Dusthide)")]
		[Order(85)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Orb = 303,
		[Description("Petals (Dusthide)")]
		[Order(86)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Petals = 304,
		[Description("Petrified (Dusthide)")]
		[Order(87)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Petrified = 309,
		[Description("Piebald (Dusthide)")]
		[Order(88)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Piebald = 305,
		[Description("Pinstripe (Dusthide)")]
		[Order(89)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Pinstripe = 308,
		[Description("Ribbon (Dusthide)")]
		[Order(90)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Ribbon = 311,
		[Description("Ripple (Dusthide)")]
		[Order(91)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Ripple = 312,
		[Description("Sailfish (Dusthide)")]
		[Order(92)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Sailfish = 313,
		[Description("Savannah (Dusthide)")]
		[Order(93)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Savannah = 314,
		[Description("Skink (Dusthide)")]
		[Order(94)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Skink = 315,
		[Description("Speckle (Dusthide)")]
		[Order(95)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Speckle = 294,
		[Description("Strike (Dusthide)")]
		[Order(96)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Strike = 205,
		[Description("Varnish (Dusthide)")]
		[Order(97)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Varnish = 316,
		[Description("Wasp (Dusthide)")]
		[Order(98)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Wasp = 310,
		[Description("Wrought (Dusthide)")]
		[Order(99)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Wrought = 307,
		[Description("Arc (Veilspun)")]
		[Order(100)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Arc = 70,
		[Description("Bar (Veilspun)")]
		[Order(101)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Bar = 145,
		[Description("Boa (Veilspun)")]
		[Order(102)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Boa = 236,
		[Description("Boulder (Veilspun)")]
		[Order(103)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Boulder = 290,
		[Description("Bright (Veilspun)")]
		[Order(104)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Bright = 69,
		[Description("Cinder (Veilspun)")]
		[Order(105)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Cinder = 219,
		[Description("Clown (Veilspun)")]
		[Order(106)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Clown = 76,
		[Description("Crystal (Veilspun)")]
		[Order(107)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Crystal = 146,
		[Description("Fade (Veilspun)")]
		[Order(108)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Fade = 60,
		[Description("Falcon (Veilspun)")]
		[Order(109)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Falcon = 139,
		[Description("Fern (Veilspun)")]
		[Order(110)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Fern = 138,
		[Description("Giraffe (Veilspun)")]
		[Order(111)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Giraffe = 83,
		[Description("Jupiter (Veilspun)")]
		[Order(112)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Jupiter = 64,
		[Description("Laced (Veilspun)")]
		[Order(113)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Laced = 61,
		[Description("Leopard (Veilspun)")]
		[Order(114)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Leopard = 142,
		[Description("Orb (Veilspun)")]
		[Order(115)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Orb = 285,
		[Description("Petals (Veilspun)")]
		[Order(116)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Petals = 143,
		[Description("Poison (Veilspun)")]
		[Order(117)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Poison = 140,
		[Description("Shell (Veilspun)")]
		[Order(118)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Shell = 71,
		[Description("Skink (Veilspun)")]
		[Order(119)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Skink = 67,
		[Description("Slime (Veilspun)")]
		[Order(120)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Slime = 141,
		[Description("Speckle (Veilspun)")]
		[Order(121)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Speckle = 144,
		[Description("Sphinxmoth (Veilspun)")]
		[Order(122)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Sphinxmoth = 72,
		[Description("Starmap (Veilspun)")]
		[Order(123)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Starmap = 65,
		[Description("Stitched (Veilspun)")]
		[Order(124)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Stitched = 66,
		[Description("Tapir (Veilspun)")]
		[Order(125)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Tapir = 62,
		[Description("Vipera (Veilspun)")]
		[Order(126)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Vipera = 63,
		[Description("Wasp (Veilspun)")]
		[Order(127)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Wasp = 68,
		[Description("Bar (Aberration)")]
		[Order(128)]
		[Gene(DragonType.Aberration)]
		Aberration_Bar = 89,
		[Description("Boa (Aberration)")]
		[Order(129)]
		[Gene(DragonType.Aberration)]
		Aberration_Boa = 233,
		[Description("Boulder (Aberration)")]
		[Order(130)]
		[Gene(DragonType.Aberration)]
		Aberration_Boulder = 220,
		[Description("Cherub (Aberration)")]
		[Order(131)]
		[Gene(DragonType.Aberration)]
		Aberration_Cherub = 221,
		[Description("Cinder (Aberration)")]
		[Order(132)]
		[Gene(DragonType.Aberration)]
		Aberration_Cinder = 214,
		[Description("Clown (Aberration)")]
		[Order(133)]
		[Gene(DragonType.Aberration)]
		Aberration_Clown = 222,
		[Description("Crystal (Aberration)")]
		[Order(134)]
		[Gene(DragonType.Aberration)]
		Aberration_Crystal = 91,
		[Description("Diamond (Aberration)")]
		[Order(135)]
		[Gene(DragonType.Aberration)]
		Aberration_Diamond = 93,
		[Description("Fade (Aberration)")]
		[Order(136)]
		[Gene(DragonType.Aberration)]
		Aberration_Fade = 90,
		[Description("Falcon (Aberration)")]
		[Order(137)]
		[Gene(DragonType.Aberration)]
		Aberration_Falcon = 92,
		[Description("Fern (Aberration)")]
		[Order(138)]
		[Gene(DragonType.Aberration)]
		Aberration_Fern = 223,
		[Description("Flaunt (Aberration)")]
		[Order(139)]
		[Gene(DragonType.Aberration)]
		Aberration_Flaunt = 112,
		[Description("Giraffe (Aberration)")]
		[Order(140)]
		[Gene(DragonType.Aberration)]
		Aberration_Giraffe = 94,
		[Description("Ground (Aberration)")]
		[Order(141)]
		[Gene(DragonType.Aberration)]
		Aberration_Ground = 97,
		[Description("Harlequin (Aberration)")]
		[Order(142)]
		[Gene(DragonType.Aberration)]
		Aberration_Harlequin = 224,
		[Description("Jaguar (Aberration)")]
		[Order(143)]
		[Gene(DragonType.Aberration)]
		Aberration_Jaguar = 99,
		[Description("Leopard (Aberration)")]
		[Order(144)]
		[Gene(DragonType.Aberration)]
		Aberration_Leopard = 225,
		[Description("Lionfish (Aberration)")]
		[Order(145)]
		[Gene(DragonType.Aberration)]
		Aberration_Lionfish = 100,
		[Description("Orb (Aberration)")]
		[Order(146)]
		[Gene(DragonType.Aberration)]
		Aberration_Orb = 102,
		[Description("Pharaoh (Aberration)")]
		[Order(147)]
		[Gene(DragonType.Aberration)]
		Aberration_Pharaoh = 101,
		[Description("Pinstripe (Aberration)")]
		[Order(148)]
		[Gene(DragonType.Aberration)]
		Aberration_Pinstripe = 226,
		[Description("Poison (Aberration)")]
		[Order(149)]
		[Gene(DragonType.Aberration)]
		Aberration_Poison = 227,
		[Description("Ribbon (Aberration)")]
		[Order(150)]
		[Gene(DragonType.Aberration)]
		Aberration_Ribbon = 105,
		[Description("Ripple (Aberration)")]
		[Order(151)]
		[Gene(DragonType.Aberration)]
		Aberration_Ripple = 228,
		[Description("Savannah (Aberration)")]
		[Order(152)]
		[Gene(DragonType.Aberration)]
		Aberration_Savannah = 103,
		[Description("Skink (Aberration)")]
		[Order(153)]
		[Gene(DragonType.Aberration)]
		Aberration_Skink = 229,
		[Description("Slime (Aberration)")]
		[Order(154)]
		[Gene(DragonType.Aberration)]
		Aberration_Slime = 106,
		[Description("Speckle (Aberration)")]
		[Order(155)]
		[Gene(DragonType.Aberration)]
		Aberration_Speckle = 98,
		[Description("Starmap (Aberration)")]
		[Order(156)]
		[Gene(DragonType.Aberration)]
		Aberration_Starmap = 230,
		[Description("Stitched (Aberration)")]
		[Order(157)]
		[Gene(DragonType.Aberration)]
		Aberration_Stitched = 107,
		[Description("Swirl (Aberration)")]
		[Order(158)]
		[Gene(DragonType.Aberration)]
		Aberration_Swirl = 104,
		[Description("Tapir (Aberration)")]
		[Order(159)]
		[Gene(DragonType.Aberration)]
		Aberration_Tapir = 95,
		[Description("Tide (Aberration)")]
		[Order(160)]
		[Gene(DragonType.Aberration)]
		Aberration_Tide = 231,
		[Description("Vipera (Aberration)")]
		[Order(161)]
		[Gene(DragonType.Aberration)]
		Aberration_Vipera = 96,
		[Description("Wasp (Aberration)")]
		[Order(162)]
		[Gene(DragonType.Aberration)]
		Aberration_Wasp = 108,
		[Description("Bar (Aether)")]
		[Order(163)]
		[Gene(DragonType.Aether)]
		Aether_Bar = 150,
		[Description("Boa (Aether)")]
		[Order(164)]
		[Gene(DragonType.Aether)]
		Aether_Boa = 234,
		[Description("Boulder (Aether)")]
		[Order(165)]
		[Gene(DragonType.Aether)]
		Aether_Boulder = 151,
		[Description("Candy (Aether)")]
		[Order(166)]
		[Gene(DragonType.Aether)]
		Aether_Candy = 167,
		[Description("Cinder (Aether)")]
		[Order(167)]
		[Gene(DragonType.Aether)]
		Aether_Cinder = 163,
		[Description("Clown (Aether)")]
		[Order(168)]
		[Gene(DragonType.Aether)]
		Aether_Clown = 169,
		[Description("Fade (Aether)")]
		[Order(169)]
		[Gene(DragonType.Aether)]
		Aether_Fade = 148,
		[Description("Flaunt (Aether)")]
		[Order(170)]
		[Gene(DragonType.Aether)]
		Aether_Flaunt = 149,
		[Description("Jaguar (Aether)")]
		[Order(171)]
		[Gene(DragonType.Aether)]
		Aether_Jaguar = 152,
		[Description("Jupiter (Aether)")]
		[Order(172)]
		[Gene(DragonType.Aether)]
		Aether_Jupiter = 153,
		[Description("Laced (Aether)")]
		[Order(173)]
		[Gene(DragonType.Aether)]
		Aether_Laced = 156,
		[Description("Lionfish (Aether)")]
		[Order(174)]
		[Gene(DragonType.Aether)]
		Aether_Lionfish = 158,
		[Description("Metallic (Aether)")]
		[Order(175)]
		[Gene(DragonType.Aether)]
		Aether_Metallic = 157,
		[Description("Mosaic (Aether)")]
		[Order(176)]
		[Gene(DragonType.Aether)]
		Aether_Mosaic = 155,
		[Description("Orb (Aether)")]
		[Order(177)]
		[Gene(DragonType.Aether)]
		Aether_Orb = 288,
		[Description("Petals (Aether)")]
		[Order(178)]
		[Gene(DragonType.Aether)]
		Aether_Petals = 154,
		[Description("Piebald (Aether)")]
		[Order(179)]
		[Gene(DragonType.Aether)]
		Aether_Piebald = 159,
		[Description("Python (Aether)")]
		[Order(180)]
		[Gene(DragonType.Aether)]
		Aether_Python = 160,
		[Description("Skink (Aether)")]
		[Order(181)]
		[Gene(DragonType.Aether)]
		Aether_Skink = 161,
		[Description("Spool (Aether)")]
		[Order(182)]
		[Gene(DragonType.Aether)]
		Aether_Spool = 162,
		[Description("Starmap (Aether)")]
		[Order(183)]
		[Gene(DragonType.Aether)]
		Aether_Starmap = 168,
		[Description("Stitched (Aether)")]
		[Order(184)]
		[Gene(DragonType.Aether)]
		Aether_Stitched = 165,
		[Description("Tide (Aether)")]
		[Order(185)]
		[Gene(DragonType.Aether)]
		Aether_Tide = 164,
		[Description("Twinkle (Aether)")]
		[Order(186)]
		[Gene(DragonType.Aether)]
		Aether_Twinkle = 166,
		[Description("Bar (Auraboa)")]
		[Order(187)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Bar = 238,
		[Description("Boa (Auraboa)")]
		[Order(188)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Boa = 239,
		[Description("Boulder (Auraboa)")]
		[Order(189)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Boulder = 240,
		[Description("Caterpillar (Auraboa)")]
		[Order(190)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Caterpillar = 241,
		[Description("Fade (Auraboa)")]
		[Order(191)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Fade = 242,
		[Description("Falcon (Auraboa)")]
		[Order(192)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Falcon = 243,
		[Description("Fern (Auraboa)")]
		[Order(193)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Fern = 244,
		[Description("Flaunt (Auraboa)")]
		[Order(194)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Flaunt = 245,
		[Description("Giraffe (Auraboa)")]
		[Order(195)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Giraffe = 246,
		[Description("Harlequin (Auraboa)")]
		[Order(196)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Harlequin = 247,
		[Description("Jaguar (Auraboa)")]
		[Order(197)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Jaguar = 248,
		[Description("Laced (Auraboa)")]
		[Order(198)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Laced = 249,
		[Description("Love (Auraboa)")]
		[Order(199)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Love = 279,
		[Description("Metallic (Auraboa)")]
		[Order(200)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Metallic = 250,
		[Description("Mochlus (Auraboa)")]
		[Order(201)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Mochlus = 251,
		[Description("Mosaic (Auraboa)")]
		[Order(202)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Mosaic = 252,
		[Description("Orb (Auraboa)")]
		[Order(203)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Orb = 253,
		[Description("Piebald (Auraboa)")]
		[Order(204)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Piebald = 254,
		[Description("Python (Auraboa)")]
		[Order(205)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Python = 255,
		[Description("Rattlesnake (Auraboa)")]
		[Order(206)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Rattlesnake = 256,
		[Description("Ripple (Auraboa)")]
		[Order(207)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Ripple = 257,
		[Description("Starmap (Auraboa)")]
		[Order(208)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Starmap = 263,
		[Description("Tapir (Auraboa)")]
		[Order(209)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Tapir = 258,
		[Description("Tiger (Auraboa)")]
		[Order(210)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Tiger = 259,
		[Description("Varnish (Auraboa)")]
		[Order(211)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Varnish = 261,
		[Description("Vipera (Auraboa)")]
		[Order(212)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Vipera = 260,
		[Description("Wicker (Auraboa)")]
		[Order(213)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Wicker = 262,
		[Description("Bar (Banescale)")]
		[Order(214)]
		[Gene(DragonType.Banescale)]
		Banescale_Bar = 181,
		[Description("Boa (Banescale)")]
		[Order(215)]
		[Gene(DragonType.Banescale)]
		Banescale_Boa = 235,
		[Description("Boulder (Banescale)")]
		[Order(216)]
		[Gene(DragonType.Banescale)]
		Banescale_Boulder = 291,
		[Description("Candycane (Banescale)")]
		[Order(217)]
		[Gene(DragonType.Banescale)]
		Banescale_Candycane = 55,
		[Description("Cherub (Banescale)")]
		[Order(218)]
		[Gene(DragonType.Banescale)]
		Banescale_Cherub = 43,
		[Description("Chevron (Banescale)")]
		[Order(219)]
		[Gene(DragonType.Banescale)]
		Banescale_Chevron = 54,
		[Description("Cinder (Banescale)")]
		[Order(220)]
		[Gene(DragonType.Banescale)]
		Banescale_Cinder = 215,
		[Description("Clown (Banescale)")]
		[Order(221)]
		[Gene(DragonType.Banescale)]
		Banescale_Clown = 75,
		[Description("Crystal (Banescale)")]
		[Order(222)]
		[Gene(DragonType.Banescale)]
		Banescale_Crystal = 182,
		[Description("Fade (Banescale)")]
		[Order(223)]
		[Gene(DragonType.Banescale)]
		Banescale_Fade = 85,
		[Description("Falcon (Banescale)")]
		[Order(224)]
		[Gene(DragonType.Banescale)]
		Banescale_Falcon = 80,
		[Description("Fern (Banescale)")]
		[Order(225)]
		[Gene(DragonType.Banescale)]
		Banescale_Fern = 137,
		[Description("Giraffe (Banescale)")]
		[Order(226)]
		[Gene(DragonType.Banescale)]
		Banescale_Giraffe = 81,
		[Description("Jaguar (Banescale)")]
		[Order(227)]
		[Gene(DragonType.Banescale)]
		Banescale_Jaguar = 44,
		[Description("Laced (Banescale)")]
		[Order(228)]
		[Gene(DragonType.Banescale)]
		Banescale_Laced = 48,
		[Description("Leopard (Banescale)")]
		[Order(229)]
		[Gene(DragonType.Banescale)]
		Banescale_Leopard = 109,
		[Description("Marble (Banescale)")]
		[Order(230)]
		[Gene(DragonType.Banescale)]
		Banescale_Marble = 47,
		[Description("Metallic (Banescale)")]
		[Order(231)]
		[Gene(DragonType.Banescale)]
		Banescale_Metallic = 49,
		[Description("Orb (Banescale)")]
		[Order(232)]
		[Gene(DragonType.Banescale)]
		Banescale_Orb = 287,
		[Description("Petals (Banescale)")]
		[Order(233)]
		[Gene(DragonType.Banescale)]
		Banescale_Petals = 51,
		[Description("Pharaoh (Banescale)")]
		[Order(234)]
		[Gene(DragonType.Banescale)]
		Banescale_Pharaoh = 185,
		[Description("Pinstripe (Banescale)")]
		[Order(235)]
		[Gene(DragonType.Banescale)]
		Banescale_Pinstripe = 45,
		[Description("Poison (Banescale)")]
		[Order(236)]
		[Gene(DragonType.Banescale)]
		Banescale_Poison = 53,
		[Description("Ragged (Banescale)")]
		[Order(237)]
		[Gene(DragonType.Banescale)]
		Banescale_Ragged = 56,
		[Description("Ribbon (Banescale)")]
		[Order(238)]
		[Gene(DragonType.Banescale)]
		Banescale_Ribbon = 183,
		[Description("Ripple (Banescale)")]
		[Order(239)]
		[Gene(DragonType.Banescale)]
		Banescale_Ripple = 79,
		[Description("Savannah (Banescale)")]
		[Order(240)]
		[Gene(DragonType.Banescale)]
		Banescale_Savannah = 50,
		[Description("Skink (Banescale)")]
		[Order(241)]
		[Gene(DragonType.Banescale)]
		Banescale_Skink = 52,
		[Description("Speckle (Banescale)")]
		[Order(242)]
		[Gene(DragonType.Banescale)]
		Banescale_Speckle = 147,
		[Description("Tapir (Banescale)")]
		[Order(243)]
		[Gene(DragonType.Banescale)]
		Banescale_Tapir = 74,
		[Description("Tide (Banescale)")]
		[Order(244)]
		[Gene(DragonType.Banescale)]
		Banescale_Tide = 184,
		[Description("Tiger (Banescale)")]
		[Order(245)]
		[Gene(DragonType.Banescale)]
		Banescale_Tiger = 46,
		[Description("Bar (Gaoler)")]
		[Order(246)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Bar = 34,
		[Description("Boa (Gaoler)")]
		[Order(247)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Boa = 171,
		[Description("Boulder (Gaoler)")]
		[Order(248)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Boulder = 289,
		[Description("Cinder (Gaoler)")]
		[Order(249)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Cinder = 216,
		[Description("Clown (Gaoler)")]
		[Order(250)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Clown = 77,
		[Description("Crystal (Gaoler)")]
		[Order(251)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Crystal = 37,
		[Description("Fade (Gaoler)")]
		[Order(252)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Fade = 86,
		[Description("Falcon (Gaoler)")]
		[Order(253)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Falcon = 30,
		[Description("Flaunt (Gaoler)")]
		[Order(254)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Flaunt = 111,
		[Description("Giraffe (Gaoler)")]
		[Order(255)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Giraffe = 27,
		[Description("Jaguar (Gaoler)")]
		[Order(256)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Jaguar = 33,
		[Description("Laced (Gaoler)")]
		[Order(257)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Laced = 73,
		[Description("Leopard (Gaoler)")]
		[Order(258)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Leopard = 173,
		[Description("Mosaic (Gaoler)")]
		[Order(259)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Mosaic = 38,
		[Description("Orb (Gaoler)")]
		[Order(260)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Orb = 286,
		[Description("Phantom (Gaoler)")]
		[Order(261)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Phantom = 39,
		[Description("Piebald (Gaoler)")]
		[Order(262)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Piebald = 31,
		[Description("Pinstripe (Gaoler)")]
		[Order(263)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Pinstripe = 32,
		[Description("Poison (Gaoler)")]
		[Order(264)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Poison = 174,
		[Description("Ribbon (Gaoler)")]
		[Order(265)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Ribbon = 175,
		[Description("Ripple (Gaoler)")]
		[Order(266)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Ripple = 78,
		[Description("Savannah (Gaoler)")]
		[Order(267)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Savannah = 176,
		[Description("Shaggy (Gaoler)")]
		[Order(268)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Shaggy = 29,
		[Description("Skink (Gaoler)")]
		[Order(269)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Skink = 177,
		[Description("Slime (Gaoler)")]
		[Order(270)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Slime = 178,
		[Description("Stitched (Gaoler)")]
		[Order(271)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Stitched = 180,
		[Description("Swirl (Gaoler)")]
		[Order(272)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Swirl = 179,
		[Description("Tapir (Gaoler)")]
		[Order(273)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Tapir = 35,
		[Description("Tide (Gaoler)")]
		[Order(274)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Tide = 172,
		[Description("Tiger (Gaoler)")]
		[Order(275)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Tiger = 36,
		[Description("Wasp (Gaoler)")]
		[Order(276)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Wasp = 28,
		[Description("Bar (Undertide)")]
		[Order(277)]
		[Gene(DragonType.Undertide)]
		Undertide_Bar = 117,
		[Description("Boa (Undertide)")]
		[Order(278)]
		[Gene(DragonType.Undertide)]
		Undertide_Boa = 128,
		[Description("Boulder (Undertide)")]
		[Order(279)]
		[Gene(DragonType.Undertide)]
		Undertide_Boulder = 135,
		[Description("Checkers (Undertide)")]
		[Order(280)]
		[Gene(DragonType.Undertide)]
		Undertide_Checkers = 127,
		[Description("Cherub (Undertide)")]
		[Order(281)]
		[Gene(DragonType.Undertide)]
		Undertide_Cherub = 119,
		[Description("Chrysocolla (Undertide)")]
		[Order(282)]
		[Gene(DragonType.Undertide)]
		Undertide_Chrysocolla = 264,
		[Description("Cinder (Undertide)")]
		[Order(283)]
		[Gene(DragonType.Undertide)]
		Undertide_Cinder = 218,
		[Description("Clown (Undertide)")]
		[Order(284)]
		[Gene(DragonType.Undertide)]
		Undertide_Clown = 265,
		[Description("Crystal (Undertide)")]
		[Order(285)]
		[Gene(DragonType.Undertide)]
		Undertide_Crystal = 118,
		[Description("Fade (Undertide)")]
		[Order(286)]
		[Gene(DragonType.Undertide)]
		Undertide_Fade = 115,
		[Description("Falcon (Undertide)")]
		[Order(287)]
		[Gene(DragonType.Undertide)]
		Undertide_Falcon = 122,
		[Description("Fern (Undertide)")]
		[Order(288)]
		[Gene(DragonType.Undertide)]
		Undertide_Fern = 266,
		[Description("Flaunt (Undertide)")]
		[Order(289)]
		[Gene(DragonType.Undertide)]
		Undertide_Flaunt = 267,
		[Description("Giraffe (Undertide)")]
		[Order(290)]
		[Gene(DragonType.Undertide)]
		Undertide_Giraffe = 123,
		[Description("Ground (Undertide)")]
		[Order(291)]
		[Gene(DragonType.Undertide)]
		Undertide_Ground = 268,
		[Description("Harlequin (Undertide)")]
		[Order(292)]
		[Gene(DragonType.Undertide)]
		Undertide_Harlequin = 280,
		[Description("Jaguar (Undertide)")]
		[Order(293)]
		[Gene(DragonType.Undertide)]
		Undertide_Jaguar = 269,
		[Description("Jupiter (Undertide)")]
		[Order(294)]
		[Gene(DragonType.Undertide)]
		Undertide_Jupiter = 270,
		[Description("Lionfish (Undertide)")]
		[Order(295)]
		[Gene(DragonType.Undertide)]
		Undertide_Lionfish = 126,
		[Description("Metallic (Undertide)")]
		[Order(296)]
		[Gene(DragonType.Undertide)]
		Undertide_Metallic = 278,
		[Description("Octopus (Undertide)")]
		[Order(297)]
		[Gene(DragonType.Undertide)]
		Undertide_Octopus = 133,
		[Description("Orb (Undertide)")]
		[Order(298)]
		[Gene(DragonType.Undertide)]
		Undertide_Orb = 271,
		[Description("Petals (Undertide)")]
		[Order(299)]
		[Gene(DragonType.Undertide)]
		Undertide_Petals = 281,
		[Description("Pharaoh (Undertide)")]
		[Order(300)]
		[Gene(DragonType.Undertide)]
		Undertide_Pharaoh = 120,
		[Description("Piebald (Undertide)")]
		[Order(301)]
		[Gene(DragonType.Undertide)]
		Undertide_Piebald = 272,
		[Description("Pinstripe (Undertide)")]
		[Order(302)]
		[Gene(DragonType.Undertide)]
		Undertide_Pinstripe = 121,
		[Description("Poison (Undertide)")]
		[Order(303)]
		[Gene(DragonType.Undertide)]
		Undertide_Poison = 131,
		[Description("Ribbon (Undertide)")]
		[Order(304)]
		[Gene(DragonType.Undertide)]
		Undertide_Ribbon = 124,
		[Description("Ripple (Undertide)")]
		[Order(305)]
		[Gene(DragonType.Undertide)]
		Undertide_Ripple = 130,
		[Description("Savannah (Undertide)")]
		[Order(306)]
		[Gene(DragonType.Undertide)]
		Undertide_Savannah = 129,
		[Description("Speckle (Undertide)")]
		[Order(307)]
		[Gene(DragonType.Undertide)]
		Undertide_Speckle = 132,
		[Description("Starmap (Undertide)")]
		[Order(308)]
		[Gene(DragonType.Undertide)]
		Undertide_Starmap = 273,
		[Description("Stitched (Undertide)")]
		[Order(309)]
		[Gene(DragonType.Undertide)]
		Undertide_Stitched = 282,
		[Description("Swirl (Undertide)")]
		[Order(310)]
		[Gene(DragonType.Undertide)]
		Undertide_Swirl = 134,
		[Description("Tapir (Undertide)")]
		[Order(311)]
		[Gene(DragonType.Undertide)]
		Undertide_Tapir = 274,
		[Description("Tide (Undertide)")]
		[Order(312)]
		[Gene(DragonType.Undertide)]
		Undertide_Tide = 113,
		[Description("Tiger (Undertide)")]
		[Order(313)]
		[Gene(DragonType.Undertide)]
		Undertide_Tiger = 275,
		[Description("Varnish (Undertide)")]
		[Order(314)]
		[Gene(DragonType.Undertide)]
		Undertide_Varnish = 276,
		[Description("Wasp (Undertide)")]
		[Order(315)]
		[Gene(DragonType.Undertide)]
		Undertide_Wasp = 125,
		[Description("Wolf (Undertide)")]
		[Order(316)]
		[Gene(DragonType.Undertide)]
		Undertide_Wolf = 116,
		[Description("Wrought (Undertide)")]
		[Order(317)]
		[Gene(DragonType.Undertide)]
		Undertide_Wrought = 277,
	}

	public enum WingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Shimmer")]
		Shimmer = 1,
		[Description("Stripes")]
		Stripes = 2,
		[Description("Eye Spots")]
		EyeSpots = 3,
		[Description("Freckle")]
		Freckle = 4,
		[Description("Seraph")]
		Seraph = 5,
		[Description("Current")]
		Current = 6,
		[Description("Daub")]
		Daub = 7,
		[Description("Facet")]
		Facet = 8,
		[Description("Hypnotic")]
		Hypnotic = 9,
		[Description("Paint")]
		Paint = 10,
		[Description("Peregrine")]
		Peregrine = 11,
		[Description("Toxin")]
		Toxin = 12,
		[Description("Butterfly")]
		Butterfly = 13,
		[Description("Hex")]
		Hex = 14,
		[Description("Saturn")]
		Saturn = 15,
		[Description("Spinner")]
		Spinner = 16,
		[Description("Alloy")]
		Alloy = 17,
		[Description("Safari")]
		Safari = 18,
		[Description("Rosette")]
		Rosette = 19,
		[Description("Bee")]
		Bee = 20,
		[Description("Striation")]
		Striation = 21,
		[Description("Trail")]
		Trail = 22,
		[Description("Morph")]
		Morph = 23,
		[Description("Noxtide")]
		Noxtide = 24,
		[Description("Constellation")]
		Constellation = 25,
		[Description("Edged")]
		Edged = 26,
		[Description("Clouded")]
		Clouded = 40,
		[Description("Sludge")]
		Sludge = 41,
		[Description("Blend")]
		Blend = 42,
		[Description("Marbled")]
		Marbled = 57,
		[Description("Breakup")]
		Breakup = 58,
		[Description("Patchwork")]
		Patchwork = 59,
		[Description("Flair")]
		Flair = 82,
		[Description("Eel")]
		Eel = 84,
		[Description("Sarcophagus")]
		Sarcophagus = 87,
		[Description("Fissure")]
		Fissure = 88,
		[Description("Myrid")]
		Myrid = 110,
		[Description("Foam")]
		Foam = 113,
		[Description("Paisley")]
		Paisley = 136,
		[Description("Jester")]
		Jester = 170,
		[Description("Blaze")]
		Blaze = 213,
		[Description("Saddle")]
		Saddle = 232,
		[Description("Malachite")]
		Malachite = 237,
		[Description("Weaver")]
		Weaver = 283,
	}
	
	public enum AuraboaWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Affection (Auraboa)")]
		Affection = 279,
		[Description("Alloy (Auraboa)")]
		Alloy = 250,
		[Description("Blend (Auraboa)")]
		Blend = 242,
		[Description("Breakup (Auraboa)")]
		Breakup = 252,
		[Description("Constellation (Auraboa)")]
		Constellation = 263,
		[Description("Current (Auraboa)")]
		Current = 257,
		[Description("Daub (Auraboa)")]
		Daub = 238,
		[Description("Diamondback (Auraboa)")]
		Diamondback = 256,
		[Description("Edged (Auraboa)")]
		Edged = 249,
		[Description("Flair (Auraboa)")]
		Flair = 245,
		[Description("Hex (Auraboa)")]
		Hex = 246,
		[Description("Hypnotic (Auraboa)")]
		Hypnotic = 260,
		[Description("Jester (Auraboa)")]
		Jester = 247,
		[Description("Lacquer (Auraboa)")]
		Lacquer = 261,
		[Description("Larvae (Auraboa)")]
		Larvae = 241,
		[Description("Morph (Auraboa)")]
		Morph = 255,
		[Description("Myrid (Auraboa)")]
		Myrid = 240,
		[Description("Paint (Auraboa)")]
		Paint = 254,
		[Description("Paisley (Auraboa)")]
		Paisley = 244,
		[Description("Peregrine (Auraboa)")]
		Peregrine = 243,
		[Description("Riopa (Auraboa)")]
		Riopa = 251,
		[Description("Rosette (Auraboa)")]
		Rosette = 248,
		[Description("Saddle (Auraboa)")]
		Saddle = 239,
		[Description("Striation (Auraboa)")]
		Striation = 258,
		[Description("Stripes (Auraboa)")]
		Stripes = 259,
		[Description("Weaver (Auraboa)")]
		Weaver = 253,
		[Description("Woven (Auraboa)")]
		Woven = 262,
	}
	
	public enum AetherWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Alloy (Aether)")]
		Alloy = 157,
		[Description("Blaze (Aether)")]
		Blaze = 163,
		[Description("Blend (Aether)")]
		Blend = 148,
		[Description("Breakup (Aether)")]
		Breakup = 155,
		[Description("Butterfly (Aether)")]
		Butterfly = 154,
		[Description("Constellation (Aether)")]
		Constellation = 168,
		[Description("Daub (Aether)")]
		Daub = 150,
		[Description("Edged (Aether)")]
		Edged = 156,
		[Description("Eye Spots (Aether)")]
		EyeSpots = 169,
		[Description("Flair (Aether)")]
		Flair = 149,
		[Description("Flicker (Aether)")]
		Flicker = 166,
		[Description("Foam (Aether)")]
		Foam = 164,
		[Description("Icing (Aether)")]
		Icing = 167,
		[Description("Morph (Aether)")]
		Morph = 160,
		[Description("Myrid (Aether)")]
		Myrid = 151,
		[Description("Noxtide (Aether)")]
		Noxtide = 158,
		[Description("Paint (Aether)")]
		Paint = 159,
		[Description("Patchwork (Aether)")]
		Patchwork = 165,
		[Description("Rosette (Aether)")]
		Rosette = 152,
		[Description("Saddle (Aether)")]
		Saddle = 234,
		[Description("Saturn (Aether)")]
		Saturn = 153,
		[Description("Spinner (Aether)")]
		Spinner = 161,
		[Description("Thread (Aether)")]
		Thread = 162,
		[Description("Weaver (Aether)")]
		Weaver = 284,
	}
	
	public enum BanescaleWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Alloy (Banescale)")]
		Alloy = 49,
		[Description("Arrow (Banescale)")]
		Arrow = 54,
		[Description("Blaze (Banescale)")]
		Blaze = 215,
		[Description("Blend (Banescale)")]
		Blend = 85,
		[Description("Butterfly (Banescale)")]
		Butterfly = 51,
		[Description("Clouded (Banescale)")]
		Clouded = 109,
		[Description("Current (Banescale)")]
		Current = 79,
		[Description("Daub (Banescale)")]
		Daub = 181,
		[Description("Edged (Banescale)")]
		Edged = 48,
		[Description("Eel (Banescale)")]
		Eel = 183,
		[Description("Eye Spots (Banescale)")]
		EyeSpots = 75,
		[Description("Facet (Banescale)")]
		Facet = 182,
		[Description("Foam (Banescale)")]
		Foam = 184,
		[Description("Freckle (Banescale)")]
		Freckle = 147,
		[Description("Hex (Banescale)")]
		Hex = 81,
		[Description("Mottle (Banescale)")]
		Mottle = 47,
		[Description("Myrid (Banescale)")]
		Myrid = 289,
		[Description("Paisley (Banescale)")]
		Paisley = 137,
		[Description("Peregrine (Banescale)")]
		Peregrine = 80,
		[Description("Rosette (Banescale)")]
		Rosette = 44,
		[Description("Saddle (Banescale)")]
		Saddle = 235,
		[Description("Safari (Banescale)")]
		Safari = 50,
		[Description("Sarcophagus (Banescale)")]
		Sarcophagus = 185,
		[Description("Seraph (Banescale)")]
		Seraph = 43,
		[Description("Spinner (Banescale)")]
		Spinner = 52,
		[Description("Striation (Banescale)")]
		Striation = 74,
		[Description("Stripes (Banescale)")]
		Stripes = 46,
		[Description("Sugarplum (Banescale)")]
		Sugarplum = 55,
		[Description("Tear (Banescale)")]
		Tear = 56,
		[Description("Toxin (Banescale)")]
		Toxin = 53,
		[Description("Trail (Banescale)")]
		Trail = 45,
		[Description("Weaver (Banescale)")]
		Weaver = 285,
	}
	
	public enum UndertideWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Alloy (Undertide)")]
		Alloy = 278,
		[Description("Bee (Undertide)")]
		Bee = 125,
		[Description("Blaze (Undertide)")]
		Blaze = 218,
		[Description("Blend (Undertide)")]
		Blend = 115,
		[Description("Butterfly (Undertide)")]
		Butterfly = 281,
		[Description("Chess (Undertide)")]
		Chess = 127,
		[Description("Constellation (Undertide)")]
		Constellation = 273,
		[Description("Current (Undertide)")]
		Current = 130,
		[Description("Daub (Undertide)")]
		Daub = 117,
		[Description("Eel (Undertide)")]
		Eel = 124,
		[Description("Eye Spots (Undertide)")]
		EyeSpots = 265,
		[Description("Facet (Undertide)")]
		Facet = 118,
		[Description("Fissure (Undertide)")]
		Fissure = 268,
		[Description("Flair (Undertide)")]
		Flair = 267,
		[Description("Foam (Undertide)")]
		Foam = 114,
		[Description("Freckle (Undertide)")]
		Freckle = 132,
		[Description("Hex (Undertide)")]
		Hex = 123,
		[Description("Jester (Undertide)")]
		Jester = 282,
		[Description("Lacquer (Undertide)")]
		Lacquer = 276,
		[Description("Malachite (Undertide)")]
		Malachite = 264,
		[Description("Marbled (Undertide)")]
		Marbled = 134,
		[Description("Myrid (Undertide)")]
		Myrid = 135,
		[Description("Noxtide (Undertide)")]
		Noxtide = 126,
		[Description("Pack (Undertide)")]
		Pack = 116,
		[Description("Paint (Undertide)")]
		Paint = 272,
		[Description("Paisley (Undertide)")]
		Paisley = 266,
		[Description("Patchwork (Undertide)")]
		Patchwork = 280,
		[Description("Peregrine (Undertide)")]
		Peregrine = 122,
		[Description("Rings (Undertide)")]
		Rings = 133,
		[Description("Rosette (Undertide)")]
		Rosette = 269,
		[Description("Saddle (Undertide)")]
		Saddle = 128,
		[Description("Safari (Undertide)")]
		Safari = 129,
		[Description("Sarcophagus (Undertide)")]
		Sarcophagus = 120,
		[Description("Saturn (Undertide)")]
		Saturn = 270,
		[Description("Seraph (Undertide)")]
		Seraph = 119,
		[Description("Spire (Undertide)")]
		Spire = 277,
		[Description("Striation (Undertide)")]
		Striation = 274,
		[Description("Stripes (Undertide)")]
		Stripes = 275,
		[Description("Toxin (Undertide)")]
		Toxin = 131,
		[Description("Trail (Undertide)")]
		Trail = 121,
		[Description("Weaver (Undertide)")]
		Weaver = 271,
	}
	
	public enum SandsurgeWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Arowana (Sandsurge)")]
		Arowana = 194,
		[Description("Bee (Sandsurge)")]
		Bee = 207,
		[Description("Blaze (Sandsurge)")]
		Blaze = 217,
		[Description("Blend (Sandsurge)")]
		Blend = 191,
		[Description("Breakup (Sandsurge)")]
		Breakup = 200,
		[Description("Diamondback (Sandsurge)")]
		Diamondback = 206,
		[Description("Eye Spots (Sandsurge)")]
		EyeSpots = 196,
		[Description("Fissure (Sandsurge)")]
		Fissure = 186,
		[Description("Flair (Sandsurge)")]
		Flair = 193,
		[Description("Jester (Sandsurge)")]
		Jester = 192,
		[Description("Malachite (Sandsurge)")]
		Malachite = 195,
		[Description("Marbled (Sandsurge)")]
		Marbled = 209,
		[Description("Marlin (Sandsurge)")]
		Marlin = 212,
		[Description("Morph (Sandsurge)")]
		Morph = 203,
		[Description("Myrid (Sandsurge)")]
		Myrid = 188,
		[Description("Noxtide (Sandsurge)")]
		Noxtide = 199,
		[Description("Paint (Sandsurge)")]
		Paint = 201,
		[Description("Rosette (Sandsurge)")]
		Rosette = 198,
		[Description("Saddle (Sandsurge)")]
		Saddle = 189,
		[Description("Safari (Sandsurge)")]
		Safari = 204,
		[Description("Saturn (Sandsurge)")]
		Saturn = 197,
		[Description("Seraph (Sandsurge)")]
		Seraph = 190,
		[Description("Sludge (Sandsurge)")]
		Sludge = 208,
		[Description("Spire (Sandsurge)")]
		Spire = 187,
		[Description("Striation (Sandsurge)")]
		Striation = 210,
		[Description("Stripes (Sandsurge)")]
		Stripes = 211,
		[Description("Trail (Sandsurge)")]
		Trail = 202,
		[Description("Weaver (Sandsurge)")]
		Weaver = 287,
	}
	
	public enum AberrationWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bee (Aberration)")]
		Bee = 108,
		[Description("Blaze (Aberration)")]
		Blaze = 214,
		[Description("Blend (Aberration)")]
		Blend = 91,
		[Description("Clouded (Aberration)")]
		Clouded = 220,
		[Description("Constellation (Aberration)")]
		Constellation = 221,
		[Description("Current (Aberration)")]
		Current = 222,
		[Description("Daub (Aberration)")]
		Daub = 89,
		[Description("Eel (Aberration)")]
		Eel = 105,
		[Description("Eye Spots (Aberration)")]
		EyeSpots = 223,
		[Description("Facet (Aberration)")]
		Facet = 90,
		[Description("Fissure (Aberration)")]
		Fissure = 97,
		[Description("Flair (Aberration)")]
		Flair = 112,
		[Description("Foam (Aberration)")]
		Foam = 224,
		[Description("Freckle (Aberration)")]
		Freckle = 98,
		[Description("Hex (Aberration)")]
		Hex = 94,
		[Description("Hypnotic (Aberration)")]
		Hypnotic = 96,
		[Description("Jester (Aberration)")]
		Jester = 225,
		[Description("Marbled (Aberration)")]
		Marbled = 103,
		[Description("Myrid (Aberration)")]
		Myrid = 226,
		[Description("Noxtide (Aberration)")]
		Noxtide = 100,
		[Description("Paisley (Aberration)")]
		Paisley = 227,
		[Description("Patchwork (Aberration)")]
		Patchwork = 107,
		[Description("Peregrine (Aberration)")]
		Peregrine = 92,
		[Description("Rosette (Aberration)")]
		Rosette = 99,
		[Description("Saddle (Aberration)")]
		Saddle = 233,
		[Description("Safari (Aberration)")]
		Safari = 104,
		[Description("Sarcophagus (Aberration)")]
		Sarcophagus = 101,
		[Description("Seraph (Aberration)")]
		Seraph = 228,
		[Description("Sludge (Aberration)")]
		Sludge = 106,
		[Description("Spade (Aberration)")]
		Spade = 93,
		[Description("Spinner (Aberration)")]
		Spinner = 229,
		[Description("Striation (Aberration)")]
		Striation = 95,
		[Description("Toxin (Aberration)")]
		Toxin = 230,
		[Description("Trail (Aberration)")]
		Trail = 231,
		[Description("Weaver (Aberration)")]
		Weaver = 102,
	}
	
	public enum DusthideWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bee (Dusthide)")]
		Bee = 310,
		[Description("Blaze (Dusthide)")]
		Blaze = 295,
		[Description("Blend (Dusthide)")]
		Blend = 296,
		[Description("Butterfly (Dusthide)")]
		Butterfly = 304,
		[Description("Chess (Dusthide)")]
		Chess = 293,
		[Description("Coil (Dusthide)")]
		Coil = 205,
		[Description("Current (Dusthide)")]
		Current = 312,
		[Description("Daub (Dusthide)")]
		Daub = 292,
		[Description("Edged (Dusthide)")]
		Edged = 302,
		[Description("Eel (Dusthide)")]
		Eel = 311,
		[Description("Fissure (Dusthide)")]
		Fissure = 298,
		[Description("Freckle (Dusthide)")]
		Freckle = 294,
		[Description("Hex (Dusthide)")]
		Hex = 297,
		[Description("Jester (Dusthide)")]
		Jester = 299,
		[Description("Lacquer (Dusthide)")]
		Lacquer = 316,
		[Description("Lode (Dusthide)")]
		Lode = 309,
		[Description("Loop (Dusthide)")]
		Loop = 306,
		[Description("Marlin (Dusthide)")]
		Marlin = 313,
		[Description("Paint (Dusthide)")]
		Paint = 305,
		[Description("Parade (Dusthide)")]
		Parade = 317,
		[Description("Peregrine (Dusthide)")]
		Peregrine = 301,
		[Description("Safari (Dusthide)")]
		Safari = 314,
		[Description("Saturn (Dusthide)")]
		Saturn = 300,
		[Description("Spinner (Dusthide)")]
		Spinner = 315,
		[Description("Spire (Dusthide)")]
		Spire = 307,
		[Description("Trail (Dusthide)")]
		Trail = 308,
		[Description("Weaver (Dusthide)")]
		Weaver = 303,
	}
	
	public enum GaolerWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bee (Gaoler)")]
		Bee = 28,
		[Description("Blaze (Gaoler)")]
		Blaze = 216,
		[Description("Blend (Gaoler)")]
		Blend = 86,
		[Description("Breakup (Gaoler)")]
		Breakup = 38,
		[Description("Clouded (Gaoler)")]
		Clouded = 180,
		[Description("Current (Gaoler)")]
		Current = 78,
		[Description("Daub (Gaoler)")]
		Daub = 34,
		[Description("Edged (Gaoler)")]
		Edged = 73,
		[Description("Eel (Gaoler)")]
		Eel = 174,
		[Description("Eye Spots (Gaoler)")]
		EyeSpots = 77,
		[Description("Facet (Gaoler)")]
		Facet = 37,
		[Description("Flair (Gaoler)")]
		Flair = 111,
		[Description("Foam (Gaoler)")]
		Foam = 172,
		[Description("Hex (Gaoler)")]
		Hex = 27,
		[Description("Marbled (Gaoler)")]
		Marbled = 178,
		[Description("Myrid (Gaoler)")]
		Myrid = 290,
		[Description("Paint (Gaoler)")]
		Paint = 31,
		[Description("Patchwork (Gaoler)")]
		Patchwork = 179,
		[Description("Peregrine (Gaoler)")]
		Peregrine = 30,
		[Description("Rosette (Gaoler)")]
		Rosette = 33,
		[Description("Saddle (Gaoler)")]
		Saddle = 171,
		[Description("Safari (Gaoler)")]
		Safari = 175,
		[Description("Sludge (Gaoler)")]
		Sludge = 177,
		[Description("Spinner (Gaoler)")]
		Spinner = 176,
		[Description("Spirit (Gaoler)")]
		Spirit = 39,
		[Description("Streak (Gaoler)")]
		Streak = 29,
		[Description("Striation (Gaoler)")]
		Striation = 35,
		[Description("Stripes (Gaoler)")]
		Stripes = 36,
		[Description("Toxin (Gaoler)")]
		Toxin = 173,
		[Description("Trail (Gaoler)")]
		Trail = 32,
		[Description("Weaver (Gaoler)")]
		Weaver = 286,
	}
	
	public enum VeilspunWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bee (Veilspun)")]
		Bee = 60,
		[Description("Blaze (Veilspun)")]
		Blaze = 219,
		[Description("Blend (Veilspun)")]
		Blend = 61,
		[Description("Butterfly (Veilspun)")]
		Butterfly = 144,
		[Description("Clouded (Veilspun)")]
		Clouded = 142,
		[Description("Constellation (Veilspun)")]
		Constellation = 66,
		[Description("Daub (Veilspun)")]
		Daub = 145,
		[Description("Edged (Veilspun)")]
		Edged = 62,
		[Description("Eye Spots (Veilspun)")]
		EyeSpots = 76,
		[Description("Facet (Veilspun)")]
		Facet = 146,
		[Description("Freckle (Veilspun)")]
		Freckle = 143,
		[Description("Hawkmoth (Veilspun)")]
		Hawkmoth = 72,
		[Description("Hex (Veilspun)")]
		Hex = 83,
		[Description("Hypnotic (Veilspun)")]
		Hypnotic = 64,
		[Description("Loop (Veilspun)")]
		Loop = 70,
		[Description("Myrid (Veilspun)")]
		Myrid = 291,
		[Description("Paisley (Veilspun)")]
		Paisley = 138,
		[Description("Patchwork (Veilspun)")]
		Patchwork = 67,
		[Description("Peregrine (Veilspun)")]
		Peregrine = 139,
		[Description("Saddle (Veilspun)")]
		Saddle = 236,
		[Description("Saturn (Veilspun)")]
		Saturn = 65,
		[Description("Sludge (Veilspun)")]
		Sludge = 141,
		[Description("Spinner (Veilspun)")]
		Spinner = 68,
		[Description("Striation (Veilspun)")]
		Striation = 63,
		[Description("Toxin (Veilspun)")]
		Toxin = 140,
		[Description("Vivid (Veilspun)")]
		Vivid = 69,
		[Description("Weaver (Veilspun)")]
		Weaver = 288,
		[Description("Web (Veilspun)")]
		Web = 71,
	}
	
	public enum AllWingGene
	{
		[Description("Basic")]
		[Order(0)]
		[Gene(DragonType.Aberration, DragonType.Aether, DragonType.Auraboa, DragonType.Banescale, DragonType.Bogsneak, DragonType.Coatl, DragonType.Dusthide, DragonType.Fae, DragonType.Fathom, DragonType.Gaoler, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Sandsurge, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Undertide, DragonType.Veilspun, DragonType.Wildclaw)]
		Basic = 0,
		[Description("Shimmer")]
		[Order(1)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Shimmer = 1,
		[Description("Stripes")]
		[Order(2)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Stripes = 2,
		[Description("Eye Spots")]
		[Order(3)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		EyeSpots = 3,
		[Description("Freckle")]
		[Order(4)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Freckle = 4,
		[Description("Seraph")]
		[Order(5)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Seraph = 5,
		[Description("Current")]
		[Order(6)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Current = 6,
		[Description("Daub")]
		[Order(7)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Daub = 7,
		[Description("Facet")]
		[Order(8)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Facet = 8,
		[Description("Hypnotic")]
		[Order(9)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Hypnotic = 9,
		[Description("Paint")]
		[Order(10)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Paint = 10,
		[Description("Peregrine")]
		[Order(11)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Peregrine = 11,
		[Description("Toxin")]
		[Order(12)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Toxin = 12,
		[Description("Butterfly")]
		[Order(13)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Butterfly = 13,
		[Description("Hex")]
		[Order(14)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Hex = 14,
		[Description("Saturn")]
		[Order(15)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Saturn = 15,
		[Description("Spinner")]
		[Order(16)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Spinner = 16,
		[Description("Alloy")]
		[Order(17)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Alloy = 17,
		[Description("Safari")]
		[Order(18)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Safari = 18,
		[Description("Rosette")]
		[Order(19)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Rosette = 19,
		[Description("Bee")]
		[Order(20)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Bee = 20,
		[Description("Striation")]
		[Order(21)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Striation = 21,
		[Description("Trail")]
		[Order(22)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Trail = 22,
		[Description("Morph")]
		[Order(23)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Morph = 23,
		[Description("Noxtide")]
		[Order(24)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Noxtide = 24,
		[Description("Constellation")]
		[Order(25)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Constellation = 25,
		[Description("Edged")]
		[Order(26)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Edged = 26,
		[Description("Clouded")]
		[Order(27)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Clouded = 40,
		[Description("Sludge")]
		[Order(28)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Sludge = 41,
		[Description("Blend")]
		[Order(29)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Blend = 42,
		[Description("Marbled")]
		[Order(30)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Marbled = 57,
		[Description("Breakup")]
		[Order(31)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Breakup = 58,
		[Description("Patchwork")]
		[Order(32)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Patchwork = 59,
		[Description("Flair")]
		[Order(33)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Flair = 82,
		[Description("Eel")]
		[Order(34)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Eel = 84,
		[Description("Sarcophagus")]
		[Order(35)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Sarcophagus = 87,
		[Description("Fissure")]
		[Order(36)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Fissure = 88,
		[Description("Myrid")]
		[Order(37)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Myrid = 110,
		[Description("Foam")]
		[Order(38)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Foam = 113,
		[Description("Paisley")]
		[Order(39)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Paisley = 136,
		[Description("Jester")]
		[Order(40)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Jester = 170,
		[Description("Blaze")]
		[Order(41)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Blaze = 213,
		[Description("Saddle")]
		[Order(42)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Saddle = 232,
		[Description("Malachite")]
		[Order(43)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Malachite = 237,
		[Description("Weaver")]
		[Order(44)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Weaver = 283,
		[Description("Affection (Auraboa)")]
		[Order(45)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Affection = 279,
		[Description("Alloy (Auraboa)")]
		[Order(46)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Alloy = 250,
		[Description("Blend (Auraboa)")]
		[Order(47)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Blend = 242,
		[Description("Breakup (Auraboa)")]
		[Order(48)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Breakup = 252,
		[Description("Constellation (Auraboa)")]
		[Order(49)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Constellation = 263,
		[Description("Current (Auraboa)")]
		[Order(50)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Current = 257,
		[Description("Daub (Auraboa)")]
		[Order(51)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Daub = 238,
		[Description("Diamondback (Auraboa)")]
		[Order(52)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Diamondback = 256,
		[Description("Edged (Auraboa)")]
		[Order(53)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Edged = 249,
		[Description("Flair (Auraboa)")]
		[Order(54)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Flair = 245,
		[Description("Hex (Auraboa)")]
		[Order(55)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Hex = 246,
		[Description("Hypnotic (Auraboa)")]
		[Order(56)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Hypnotic = 260,
		[Description("Jester (Auraboa)")]
		[Order(57)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Jester = 247,
		[Description("Lacquer (Auraboa)")]
		[Order(58)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Lacquer = 261,
		[Description("Larvae (Auraboa)")]
		[Order(59)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Larvae = 241,
		[Description("Morph (Auraboa)")]
		[Order(60)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Morph = 255,
		[Description("Myrid (Auraboa)")]
		[Order(61)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Myrid = 240,
		[Description("Paint (Auraboa)")]
		[Order(62)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Paint = 254,
		[Description("Paisley (Auraboa)")]
		[Order(63)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Paisley = 244,
		[Description("Peregrine (Auraboa)")]
		[Order(64)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Peregrine = 243,
		[Description("Riopa (Auraboa)")]
		[Order(65)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Riopa = 251,
		[Description("Rosette (Auraboa)")]
		[Order(66)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Rosette = 248,
		[Description("Saddle (Auraboa)")]
		[Order(67)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Saddle = 239,
		[Description("Striation (Auraboa)")]
		[Order(68)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Striation = 258,
		[Description("Stripes (Auraboa)")]
		[Order(69)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Stripes = 259,
		[Description("Weaver (Auraboa)")]
		[Order(70)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Weaver = 253,
		[Description("Woven (Auraboa)")]
		[Order(71)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Woven = 262,
		[Description("Alloy (Aether)")]
		[Order(72)]
		[Gene(DragonType.Aether)]
		Aether_Alloy = 157,
		[Description("Blaze (Aether)")]
		[Order(73)]
		[Gene(DragonType.Aether)]
		Aether_Blaze = 163,
		[Description("Blend (Aether)")]
		[Order(74)]
		[Gene(DragonType.Aether)]
		Aether_Blend = 148,
		[Description("Breakup (Aether)")]
		[Order(75)]
		[Gene(DragonType.Aether)]
		Aether_Breakup = 155,
		[Description("Butterfly (Aether)")]
		[Order(76)]
		[Gene(DragonType.Aether)]
		Aether_Butterfly = 154,
		[Description("Constellation (Aether)")]
		[Order(77)]
		[Gene(DragonType.Aether)]
		Aether_Constellation = 168,
		[Description("Daub (Aether)")]
		[Order(78)]
		[Gene(DragonType.Aether)]
		Aether_Daub = 150,
		[Description("Edged (Aether)")]
		[Order(79)]
		[Gene(DragonType.Aether)]
		Aether_Edged = 156,
		[Description("Eye Spots (Aether)")]
		[Order(80)]
		[Gene(DragonType.Aether)]
		Aether_EyeSpots = 169,
		[Description("Flair (Aether)")]
		[Order(81)]
		[Gene(DragonType.Aether)]
		Aether_Flair = 149,
		[Description("Flicker (Aether)")]
		[Order(82)]
		[Gene(DragonType.Aether)]
		Aether_Flicker = 166,
		[Description("Foam (Aether)")]
		[Order(83)]
		[Gene(DragonType.Aether)]
		Aether_Foam = 164,
		[Description("Icing (Aether)")]
		[Order(84)]
		[Gene(DragonType.Aether)]
		Aether_Icing = 167,
		[Description("Morph (Aether)")]
		[Order(85)]
		[Gene(DragonType.Aether)]
		Aether_Morph = 160,
		[Description("Myrid (Aether)")]
		[Order(86)]
		[Gene(DragonType.Aether)]
		Aether_Myrid = 151,
		[Description("Noxtide (Aether)")]
		[Order(87)]
		[Gene(DragonType.Aether)]
		Aether_Noxtide = 158,
		[Description("Paint (Aether)")]
		[Order(88)]
		[Gene(DragonType.Aether)]
		Aether_Paint = 159,
		[Description("Patchwork (Aether)")]
		[Order(89)]
		[Gene(DragonType.Aether)]
		Aether_Patchwork = 165,
		[Description("Rosette (Aether)")]
		[Order(90)]
		[Gene(DragonType.Aether)]
		Aether_Rosette = 152,
		[Description("Saddle (Aether)")]
		[Order(91)]
		[Gene(DragonType.Aether)]
		Aether_Saddle = 234,
		[Description("Saturn (Aether)")]
		[Order(92)]
		[Gene(DragonType.Aether)]
		Aether_Saturn = 153,
		[Description("Spinner (Aether)")]
		[Order(93)]
		[Gene(DragonType.Aether)]
		Aether_Spinner = 161,
		[Description("Thread (Aether)")]
		[Order(94)]
		[Gene(DragonType.Aether)]
		Aether_Thread = 162,
		[Description("Weaver (Aether)")]
		[Order(95)]
		[Gene(DragonType.Aether)]
		Aether_Weaver = 284,
		[Description("Alloy (Banescale)")]
		[Order(96)]
		[Gene(DragonType.Banescale)]
		Banescale_Alloy = 49,
		[Description("Arrow (Banescale)")]
		[Order(97)]
		[Gene(DragonType.Banescale)]
		Banescale_Arrow = 54,
		[Description("Blaze (Banescale)")]
		[Order(98)]
		[Gene(DragonType.Banescale)]
		Banescale_Blaze = 215,
		[Description("Blend (Banescale)")]
		[Order(99)]
		[Gene(DragonType.Banescale)]
		Banescale_Blend = 85,
		[Description("Butterfly (Banescale)")]
		[Order(100)]
		[Gene(DragonType.Banescale)]
		Banescale_Butterfly = 51,
		[Description("Clouded (Banescale)")]
		[Order(101)]
		[Gene(DragonType.Banescale)]
		Banescale_Clouded = 109,
		[Description("Current (Banescale)")]
		[Order(102)]
		[Gene(DragonType.Banescale)]
		Banescale_Current = 79,
		[Description("Daub (Banescale)")]
		[Order(103)]
		[Gene(DragonType.Banescale)]
		Banescale_Daub = 181,
		[Description("Edged (Banescale)")]
		[Order(104)]
		[Gene(DragonType.Banescale)]
		Banescale_Edged = 48,
		[Description("Eel (Banescale)")]
		[Order(105)]
		[Gene(DragonType.Banescale)]
		Banescale_Eel = 183,
		[Description("Eye Spots (Banescale)")]
		[Order(106)]
		[Gene(DragonType.Banescale)]
		Banescale_EyeSpots = 75,
		[Description("Facet (Banescale)")]
		[Order(107)]
		[Gene(DragonType.Banescale)]
		Banescale_Facet = 182,
		[Description("Foam (Banescale)")]
		[Order(108)]
		[Gene(DragonType.Banescale)]
		Banescale_Foam = 184,
		[Description("Freckle (Banescale)")]
		[Order(109)]
		[Gene(DragonType.Banescale)]
		Banescale_Freckle = 147,
		[Description("Hex (Banescale)")]
		[Order(110)]
		[Gene(DragonType.Banescale)]
		Banescale_Hex = 81,
		[Description("Mottle (Banescale)")]
		[Order(111)]
		[Gene(DragonType.Banescale)]
		Banescale_Mottle = 47,
		[Description("Myrid (Banescale)")]
		[Order(112)]
		[Gene(DragonType.Banescale)]
		Banescale_Myrid = 289,
		[Description("Paisley (Banescale)")]
		[Order(113)]
		[Gene(DragonType.Banescale)]
		Banescale_Paisley = 137,
		[Description("Peregrine (Banescale)")]
		[Order(114)]
		[Gene(DragonType.Banescale)]
		Banescale_Peregrine = 80,
		[Description("Rosette (Banescale)")]
		[Order(115)]
		[Gene(DragonType.Banescale)]
		Banescale_Rosette = 44,
		[Description("Saddle (Banescale)")]
		[Order(116)]
		[Gene(DragonType.Banescale)]
		Banescale_Saddle = 235,
		[Description("Safari (Banescale)")]
		[Order(117)]
		[Gene(DragonType.Banescale)]
		Banescale_Safari = 50,
		[Description("Sarcophagus (Banescale)")]
		[Order(118)]
		[Gene(DragonType.Banescale)]
		Banescale_Sarcophagus = 185,
		[Description("Seraph (Banescale)")]
		[Order(119)]
		[Gene(DragonType.Banescale)]
		Banescale_Seraph = 43,
		[Description("Spinner (Banescale)")]
		[Order(120)]
		[Gene(DragonType.Banescale)]
		Banescale_Spinner = 52,
		[Description("Striation (Banescale)")]
		[Order(121)]
		[Gene(DragonType.Banescale)]
		Banescale_Striation = 74,
		[Description("Stripes (Banescale)")]
		[Order(122)]
		[Gene(DragonType.Banescale)]
		Banescale_Stripes = 46,
		[Description("Sugarplum (Banescale)")]
		[Order(123)]
		[Gene(DragonType.Banescale)]
		Banescale_Sugarplum = 55,
		[Description("Tear (Banescale)")]
		[Order(124)]
		[Gene(DragonType.Banescale)]
		Banescale_Tear = 56,
		[Description("Toxin (Banescale)")]
		[Order(125)]
		[Gene(DragonType.Banescale)]
		Banescale_Toxin = 53,
		[Description("Trail (Banescale)")]
		[Order(126)]
		[Gene(DragonType.Banescale)]
		Banescale_Trail = 45,
		[Description("Weaver (Banescale)")]
		[Order(127)]
		[Gene(DragonType.Banescale)]
		Banescale_Weaver = 285,
		[Description("Alloy (Undertide)")]
		[Order(128)]
		[Gene(DragonType.Undertide)]
		Undertide_Alloy = 278,
		[Description("Bee (Undertide)")]
		[Order(129)]
		[Gene(DragonType.Undertide)]
		Undertide_Bee = 125,
		[Description("Blaze (Undertide)")]
		[Order(130)]
		[Gene(DragonType.Undertide)]
		Undertide_Blaze = 218,
		[Description("Blend (Undertide)")]
		[Order(131)]
		[Gene(DragonType.Undertide)]
		Undertide_Blend = 115,
		[Description("Butterfly (Undertide)")]
		[Order(132)]
		[Gene(DragonType.Undertide)]
		Undertide_Butterfly = 281,
		[Description("Chess (Undertide)")]
		[Order(133)]
		[Gene(DragonType.Undertide)]
		Undertide_Chess = 127,
		[Description("Constellation (Undertide)")]
		[Order(134)]
		[Gene(DragonType.Undertide)]
		Undertide_Constellation = 273,
		[Description("Current (Undertide)")]
		[Order(135)]
		[Gene(DragonType.Undertide)]
		Undertide_Current = 130,
		[Description("Daub (Undertide)")]
		[Order(136)]
		[Gene(DragonType.Undertide)]
		Undertide_Daub = 117,
		[Description("Eel (Undertide)")]
		[Order(137)]
		[Gene(DragonType.Undertide)]
		Undertide_Eel = 124,
		[Description("Eye Spots (Undertide)")]
		[Order(138)]
		[Gene(DragonType.Undertide)]
		Undertide_EyeSpots = 265,
		[Description("Facet (Undertide)")]
		[Order(139)]
		[Gene(DragonType.Undertide)]
		Undertide_Facet = 118,
		[Description("Fissure (Undertide)")]
		[Order(140)]
		[Gene(DragonType.Undertide)]
		Undertide_Fissure = 268,
		[Description("Flair (Undertide)")]
		[Order(141)]
		[Gene(DragonType.Undertide)]
		Undertide_Flair = 267,
		[Description("Foam (Undertide)")]
		[Order(142)]
		[Gene(DragonType.Undertide)]
		Undertide_Foam = 114,
		[Description("Freckle (Undertide)")]
		[Order(143)]
		[Gene(DragonType.Undertide)]
		Undertide_Freckle = 132,
		[Description("Hex (Undertide)")]
		[Order(144)]
		[Gene(DragonType.Undertide)]
		Undertide_Hex = 123,
		[Description("Jester (Undertide)")]
		[Order(145)]
		[Gene(DragonType.Undertide)]
		Undertide_Jester = 282,
		[Description("Lacquer (Undertide)")]
		[Order(146)]
		[Gene(DragonType.Undertide)]
		Undertide_Lacquer = 276,
		[Description("Malachite (Undertide)")]
		[Order(147)]
		[Gene(DragonType.Undertide)]
		Undertide_Malachite = 264,
		[Description("Marbled (Undertide)")]
		[Order(148)]
		[Gene(DragonType.Undertide)]
		Undertide_Marbled = 134,
		[Description("Myrid (Undertide)")]
		[Order(149)]
		[Gene(DragonType.Undertide)]
		Undertide_Myrid = 135,
		[Description("Noxtide (Undertide)")]
		[Order(150)]
		[Gene(DragonType.Undertide)]
		Undertide_Noxtide = 126,
		[Description("Pack (Undertide)")]
		[Order(151)]
		[Gene(DragonType.Undertide)]
		Undertide_Pack = 116,
		[Description("Paint (Undertide)")]
		[Order(152)]
		[Gene(DragonType.Undertide)]
		Undertide_Paint = 272,
		[Description("Paisley (Undertide)")]
		[Order(153)]
		[Gene(DragonType.Undertide)]
		Undertide_Paisley = 266,
		[Description("Patchwork (Undertide)")]
		[Order(154)]
		[Gene(DragonType.Undertide)]
		Undertide_Patchwork = 280,
		[Description("Peregrine (Undertide)")]
		[Order(155)]
		[Gene(DragonType.Undertide)]
		Undertide_Peregrine = 122,
		[Description("Rings (Undertide)")]
		[Order(156)]
		[Gene(DragonType.Undertide)]
		Undertide_Rings = 133,
		[Description("Rosette (Undertide)")]
		[Order(157)]
		[Gene(DragonType.Undertide)]
		Undertide_Rosette = 269,
		[Description("Saddle (Undertide)")]
		[Order(158)]
		[Gene(DragonType.Undertide)]
		Undertide_Saddle = 128,
		[Description("Safari (Undertide)")]
		[Order(159)]
		[Gene(DragonType.Undertide)]
		Undertide_Safari = 129,
		[Description("Sarcophagus (Undertide)")]
		[Order(160)]
		[Gene(DragonType.Undertide)]
		Undertide_Sarcophagus = 120,
		[Description("Saturn (Undertide)")]
		[Order(161)]
		[Gene(DragonType.Undertide)]
		Undertide_Saturn = 270,
		[Description("Seraph (Undertide)")]
		[Order(162)]
		[Gene(DragonType.Undertide)]
		Undertide_Seraph = 119,
		[Description("Spire (Undertide)")]
		[Order(163)]
		[Gene(DragonType.Undertide)]
		Undertide_Spire = 277,
		[Description("Striation (Undertide)")]
		[Order(164)]
		[Gene(DragonType.Undertide)]
		Undertide_Striation = 274,
		[Description("Stripes (Undertide)")]
		[Order(165)]
		[Gene(DragonType.Undertide)]
		Undertide_Stripes = 275,
		[Description("Toxin (Undertide)")]
		[Order(166)]
		[Gene(DragonType.Undertide)]
		Undertide_Toxin = 131,
		[Description("Trail (Undertide)")]
		[Order(167)]
		[Gene(DragonType.Undertide)]
		Undertide_Trail = 121,
		[Description("Weaver (Undertide)")]
		[Order(168)]
		[Gene(DragonType.Undertide)]
		Undertide_Weaver = 271,
		[Description("Arowana (Sandsurge)")]
		[Order(169)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Arowana = 194,
		[Description("Bee (Sandsurge)")]
		[Order(170)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Bee = 207,
		[Description("Blaze (Sandsurge)")]
		[Order(171)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Blaze = 217,
		[Description("Blend (Sandsurge)")]
		[Order(172)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Blend = 191,
		[Description("Breakup (Sandsurge)")]
		[Order(173)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Breakup = 200,
		[Description("Diamondback (Sandsurge)")]
		[Order(174)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Diamondback = 206,
		[Description("Eye Spots (Sandsurge)")]
		[Order(175)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_EyeSpots = 196,
		[Description("Fissure (Sandsurge)")]
		[Order(176)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Fissure = 186,
		[Description("Flair (Sandsurge)")]
		[Order(177)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Flair = 193,
		[Description("Jester (Sandsurge)")]
		[Order(178)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Jester = 192,
		[Description("Malachite (Sandsurge)")]
		[Order(179)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Malachite = 195,
		[Description("Marbled (Sandsurge)")]
		[Order(180)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Marbled = 209,
		[Description("Marlin (Sandsurge)")]
		[Order(181)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Marlin = 212,
		[Description("Morph (Sandsurge)")]
		[Order(182)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Morph = 203,
		[Description("Myrid (Sandsurge)")]
		[Order(183)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Myrid = 188,
		[Description("Noxtide (Sandsurge)")]
		[Order(184)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Noxtide = 199,
		[Description("Paint (Sandsurge)")]
		[Order(185)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Paint = 201,
		[Description("Rosette (Sandsurge)")]
		[Order(186)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Rosette = 198,
		[Description("Saddle (Sandsurge)")]
		[Order(187)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Saddle = 189,
		[Description("Safari (Sandsurge)")]
		[Order(188)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Safari = 204,
		[Description("Saturn (Sandsurge)")]
		[Order(189)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Saturn = 197,
		[Description("Seraph (Sandsurge)")]
		[Order(190)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Seraph = 190,
		[Description("Sludge (Sandsurge)")]
		[Order(191)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Sludge = 208,
		[Description("Spire (Sandsurge)")]
		[Order(192)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Spire = 187,
		[Description("Striation (Sandsurge)")]
		[Order(193)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Striation = 210,
		[Description("Stripes (Sandsurge)")]
		[Order(194)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Stripes = 211,
		[Description("Trail (Sandsurge)")]
		[Order(195)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Trail = 202,
		[Description("Weaver (Sandsurge)")]
		[Order(196)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Weaver = 287,
		[Description("Bee (Aberration)")]
		[Order(197)]
		[Gene(DragonType.Aberration)]
		Aberration_Bee = 108,
		[Description("Blaze (Aberration)")]
		[Order(198)]
		[Gene(DragonType.Aberration)]
		Aberration_Blaze = 214,
		[Description("Blend (Aberration)")]
		[Order(199)]
		[Gene(DragonType.Aberration)]
		Aberration_Blend = 91,
		[Description("Clouded (Aberration)")]
		[Order(200)]
		[Gene(DragonType.Aberration)]
		Aberration_Clouded = 220,
		[Description("Constellation (Aberration)")]
		[Order(201)]
		[Gene(DragonType.Aberration)]
		Aberration_Constellation = 221,
		[Description("Current (Aberration)")]
		[Order(202)]
		[Gene(DragonType.Aberration)]
		Aberration_Current = 222,
		[Description("Daub (Aberration)")]
		[Order(203)]
		[Gene(DragonType.Aberration)]
		Aberration_Daub = 89,
		[Description("Eel (Aberration)")]
		[Order(204)]
		[Gene(DragonType.Aberration)]
		Aberration_Eel = 105,
		[Description("Eye Spots (Aberration)")]
		[Order(205)]
		[Gene(DragonType.Aberration)]
		Aberration_EyeSpots = 223,
		[Description("Facet (Aberration)")]
		[Order(206)]
		[Gene(DragonType.Aberration)]
		Aberration_Facet = 90,
		[Description("Fissure (Aberration)")]
		[Order(207)]
		[Gene(DragonType.Aberration)]
		Aberration_Fissure = 97,
		[Description("Flair (Aberration)")]
		[Order(208)]
		[Gene(DragonType.Aberration)]
		Aberration_Flair = 112,
		[Description("Foam (Aberration)")]
		[Order(209)]
		[Gene(DragonType.Aberration)]
		Aberration_Foam = 224,
		[Description("Freckle (Aberration)")]
		[Order(210)]
		[Gene(DragonType.Aberration)]
		Aberration_Freckle = 98,
		[Description("Hex (Aberration)")]
		[Order(211)]
		[Gene(DragonType.Aberration)]
		Aberration_Hex = 94,
		[Description("Hypnotic (Aberration)")]
		[Order(212)]
		[Gene(DragonType.Aberration)]
		Aberration_Hypnotic = 96,
		[Description("Jester (Aberration)")]
		[Order(213)]
		[Gene(DragonType.Aberration)]
		Aberration_Jester = 225,
		[Description("Marbled (Aberration)")]
		[Order(214)]
		[Gene(DragonType.Aberration)]
		Aberration_Marbled = 103,
		[Description("Myrid (Aberration)")]
		[Order(215)]
		[Gene(DragonType.Aberration)]
		Aberration_Myrid = 226,
		[Description("Noxtide (Aberration)")]
		[Order(216)]
		[Gene(DragonType.Aberration)]
		Aberration_Noxtide = 100,
		[Description("Paisley (Aberration)")]
		[Order(217)]
		[Gene(DragonType.Aberration)]
		Aberration_Paisley = 227,
		[Description("Patchwork (Aberration)")]
		[Order(218)]
		[Gene(DragonType.Aberration)]
		Aberration_Patchwork = 107,
		[Description("Peregrine (Aberration)")]
		[Order(219)]
		[Gene(DragonType.Aberration)]
		Aberration_Peregrine = 92,
		[Description("Rosette (Aberration)")]
		[Order(220)]
		[Gene(DragonType.Aberration)]
		Aberration_Rosette = 99,
		[Description("Saddle (Aberration)")]
		[Order(221)]
		[Gene(DragonType.Aberration)]
		Aberration_Saddle = 233,
		[Description("Safari (Aberration)")]
		[Order(222)]
		[Gene(DragonType.Aberration)]
		Aberration_Safari = 104,
		[Description("Sarcophagus (Aberration)")]
		[Order(223)]
		[Gene(DragonType.Aberration)]
		Aberration_Sarcophagus = 101,
		[Description("Seraph (Aberration)")]
		[Order(224)]
		[Gene(DragonType.Aberration)]
		Aberration_Seraph = 228,
		[Description("Sludge (Aberration)")]
		[Order(225)]
		[Gene(DragonType.Aberration)]
		Aberration_Sludge = 106,
		[Description("Spade (Aberration)")]
		[Order(226)]
		[Gene(DragonType.Aberration)]
		Aberration_Spade = 93,
		[Description("Spinner (Aberration)")]
		[Order(227)]
		[Gene(DragonType.Aberration)]
		Aberration_Spinner = 229,
		[Description("Striation (Aberration)")]
		[Order(228)]
		[Gene(DragonType.Aberration)]
		Aberration_Striation = 95,
		[Description("Toxin (Aberration)")]
		[Order(229)]
		[Gene(DragonType.Aberration)]
		Aberration_Toxin = 230,
		[Description("Trail (Aberration)")]
		[Order(230)]
		[Gene(DragonType.Aberration)]
		Aberration_Trail = 231,
		[Description("Weaver (Aberration)")]
		[Order(231)]
		[Gene(DragonType.Aberration)]
		Aberration_Weaver = 102,
		[Description("Bee (Dusthide)")]
		[Order(232)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Bee = 310,
		[Description("Blaze (Dusthide)")]
		[Order(233)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Blaze = 295,
		[Description("Blend (Dusthide)")]
		[Order(234)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Blend = 296,
		[Description("Butterfly (Dusthide)")]
		[Order(235)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Butterfly = 304,
		[Description("Chess (Dusthide)")]
		[Order(236)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Chess = 293,
		[Description("Coil (Dusthide)")]
		[Order(237)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Coil = 205,
		[Description("Current (Dusthide)")]
		[Order(238)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Current = 312,
		[Description("Daub (Dusthide)")]
		[Order(239)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Daub = 292,
		[Description("Edged (Dusthide)")]
		[Order(240)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Edged = 302,
		[Description("Eel (Dusthide)")]
		[Order(241)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Eel = 311,
		[Description("Fissure (Dusthide)")]
		[Order(242)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Fissure = 298,
		[Description("Freckle (Dusthide)")]
		[Order(243)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Freckle = 294,
		[Description("Hex (Dusthide)")]
		[Order(244)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Hex = 297,
		[Description("Jester (Dusthide)")]
		[Order(245)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Jester = 299,
		[Description("Lacquer (Dusthide)")]
		[Order(246)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Lacquer = 316,
		[Description("Lode (Dusthide)")]
		[Order(247)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Lode = 309,
		[Description("Loop (Dusthide)")]
		[Order(248)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Loop = 306,
		[Description("Marlin (Dusthide)")]
		[Order(249)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Marlin = 313,
		[Description("Paint (Dusthide)")]
		[Order(250)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Paint = 305,
		[Description("Parade (Dusthide)")]
		[Order(251)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Parade = 317,
		[Description("Peregrine (Dusthide)")]
		[Order(252)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Peregrine = 301,
		[Description("Safari (Dusthide)")]
		[Order(253)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Safari = 314,
		[Description("Saturn (Dusthide)")]
		[Order(254)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Saturn = 300,
		[Description("Spinner (Dusthide)")]
		[Order(255)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Spinner = 315,
		[Description("Spire (Dusthide)")]
		[Order(256)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Spire = 307,
		[Description("Trail (Dusthide)")]
		[Order(257)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Trail = 308,
		[Description("Weaver (Dusthide)")]
		[Order(258)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Weaver = 303,
		[Description("Bee (Gaoler)")]
		[Order(259)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Bee = 28,
		[Description("Blaze (Gaoler)")]
		[Order(260)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Blaze = 216,
		[Description("Blend (Gaoler)")]
		[Order(261)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Blend = 86,
		[Description("Breakup (Gaoler)")]
		[Order(262)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Breakup = 38,
		[Description("Clouded (Gaoler)")]
		[Order(263)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Clouded = 180,
		[Description("Current (Gaoler)")]
		[Order(264)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Current = 78,
		[Description("Daub (Gaoler)")]
		[Order(265)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Daub = 34,
		[Description("Edged (Gaoler)")]
		[Order(266)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Edged = 73,
		[Description("Eel (Gaoler)")]
		[Order(267)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Eel = 174,
		[Description("Eye Spots (Gaoler)")]
		[Order(268)]
		[Gene(DragonType.Gaoler)]
		Gaoler_EyeSpots = 77,
		[Description("Facet (Gaoler)")]
		[Order(269)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Facet = 37,
		[Description("Flair (Gaoler)")]
		[Order(270)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Flair = 111,
		[Description("Foam (Gaoler)")]
		[Order(271)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Foam = 172,
		[Description("Hex (Gaoler)")]
		[Order(272)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Hex = 27,
		[Description("Marbled (Gaoler)")]
		[Order(273)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Marbled = 178,
		[Description("Myrid (Gaoler)")]
		[Order(274)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Myrid = 290,
		[Description("Paint (Gaoler)")]
		[Order(275)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Paint = 31,
		[Description("Patchwork (Gaoler)")]
		[Order(276)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Patchwork = 179,
		[Description("Peregrine (Gaoler)")]
		[Order(277)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Peregrine = 30,
		[Description("Rosette (Gaoler)")]
		[Order(278)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Rosette = 33,
		[Description("Saddle (Gaoler)")]
		[Order(279)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Saddle = 171,
		[Description("Safari (Gaoler)")]
		[Order(280)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Safari = 175,
		[Description("Sludge (Gaoler)")]
		[Order(281)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Sludge = 177,
		[Description("Spinner (Gaoler)")]
		[Order(282)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Spinner = 176,
		[Description("Spirit (Gaoler)")]
		[Order(283)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Spirit = 39,
		[Description("Streak (Gaoler)")]
		[Order(284)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Streak = 29,
		[Description("Striation (Gaoler)")]
		[Order(285)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Striation = 35,
		[Description("Stripes (Gaoler)")]
		[Order(286)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Stripes = 36,
		[Description("Toxin (Gaoler)")]
		[Order(287)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Toxin = 173,
		[Description("Trail (Gaoler)")]
		[Order(288)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Trail = 32,
		[Description("Weaver (Gaoler)")]
		[Order(289)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Weaver = 286,
		[Description("Bee (Veilspun)")]
		[Order(290)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Bee = 60,
		[Description("Blaze (Veilspun)")]
		[Order(291)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Blaze = 219,
		[Description("Blend (Veilspun)")]
		[Order(292)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Blend = 61,
		[Description("Butterfly (Veilspun)")]
		[Order(293)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Butterfly = 144,
		[Description("Clouded (Veilspun)")]
		[Order(294)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Clouded = 142,
		[Description("Constellation (Veilspun)")]
		[Order(295)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Constellation = 66,
		[Description("Daub (Veilspun)")]
		[Order(296)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Daub = 145,
		[Description("Edged (Veilspun)")]
		[Order(297)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Edged = 62,
		[Description("Eye Spots (Veilspun)")]
		[Order(298)]
		[Gene(DragonType.Veilspun)]
		Veilspun_EyeSpots = 76,
		[Description("Facet (Veilspun)")]
		[Order(299)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Facet = 146,
		[Description("Freckle (Veilspun)")]
		[Order(300)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Freckle = 143,
		[Description("Hawkmoth (Veilspun)")]
		[Order(301)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Hawkmoth = 72,
		[Description("Hex (Veilspun)")]
		[Order(302)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Hex = 83,
		[Description("Hypnotic (Veilspun)")]
		[Order(303)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Hypnotic = 64,
		[Description("Loop (Veilspun)")]
		[Order(304)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Loop = 70,
		[Description("Myrid (Veilspun)")]
		[Order(305)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Myrid = 291,
		[Description("Paisley (Veilspun)")]
		[Order(306)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Paisley = 138,
		[Description("Patchwork (Veilspun)")]
		[Order(307)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Patchwork = 67,
		[Description("Peregrine (Veilspun)")]
		[Order(308)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Peregrine = 139,
		[Description("Saddle (Veilspun)")]
		[Order(309)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Saddle = 236,
		[Description("Saturn (Veilspun)")]
		[Order(310)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Saturn = 65,
		[Description("Sludge (Veilspun)")]
		[Order(311)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Sludge = 141,
		[Description("Spinner (Veilspun)")]
		[Order(312)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Spinner = 68,
		[Description("Striation (Veilspun)")]
		[Order(313)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Striation = 63,
		[Description("Toxin (Veilspun)")]
		[Order(314)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Toxin = 140,
		[Description("Vivid (Veilspun)")]
		[Order(315)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Vivid = 69,
		[Description("Weaver (Veilspun)")]
		[Order(316)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Weaver = 288,
		[Description("Web (Veilspun)")]
		[Order(317)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Web = 71,
	}
	
	public enum TertiaryGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Circuit")]
		Circuit = 1,
		[Description("Gembond")]
		Gembond = 4,
		[Description("Underbelly")]
		Underbelly = 5,
		[Description("Crackle")]
		Crackle = 6,
		[Description("Smoke")]
		Smoke = 7,
		[Description("Spines")]
		Spines = 8,
		[Description("Okapi")]
		Okapi = 9,
		[Description("Glimmer")]
		Glimmer = 10,
		[Description("Thylacine")]
		Thylacine = 11,
		[Description("Stained")]
		Stained = 12,
		[Description("Contour")]
		Contour = 13,
		[Description("Runes")]
		Runes = 14,
		[Description("Scales")]
		Scales = 15,
		[Description("Lace")]
		Lace = 16,
		[Description("Opal")]
		Opal = 17,
		[Description("Capsule")]
		Capsule = 18,
		[Description("Smirch")]
		Smirch = 19,
		[Description("Ghost")]
		Ghost = 20,
		[Description("Filigree")]
		Filigree = 21,
		[Description("Firefly")]
		Firefly = 22,
		[Description("Ringlets")]
		Ringlets = 23,
		[Description("Peacock")]
		Peacock = 24,
		[Description("Veined")]
		Veined = 38,
		[Description("Keel")]
		Keel = 53,
		[Description("Glowtail")]
		Glowtail = 54,
		[Description("Koi")]
		Koi = 73,
		[Description("Sparkle")]
		Sparkle = 97,
		[Description("Flecks")]
		Flecks = 103,
		[Description("Soap")]
		Soap = 105,
		[Description("Points")]
		Points = 107,
		[Description("Firebreather")]
		Firebreather = 161,
		[Description("Polkadot")]
		Polkadot = 168,
		[Description("Wish")]
		Wish = 213,
	}
	
	public enum DusthideTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Angler (Dusthide)")]
		Angler = 286,
		[Description("Antlers (Dusthide)")]
		Antlers = 276,
		[Description("Augment (Dusthide)")]
		Augment = 280,
		[Description("Batty (Dusthide)")]
		Batty = 289,
		[Description("Blossom (Dusthide)")]
		Blossom = 281,
		[Description("Carnivore (Dusthide)")]
		Carnivore = 282,
		[Description("Dewlap (Dusthide)")]
		Dewlap = 288,
		[Description("Fishbone (Dusthide)")]
		Fishbone = 277,
		[Description("Gembond (Dusthide)")]
		Gembond = 287,
		[Description("Ghost (Dusthide)")]
		Ghost = 278,
		[Description("Glowtail (Dusthide)")]
		Glowtail = 279,
		[Description("Greenskeeper (Dusthide)")]
		Greenskeeper = 307,
		[Description("Mandibles (Dusthide)")]
		Mandibles = 283,
		[Description("Okapi (Dusthide)")]
		Okapi = 284,
		[Description("Opal (Dusthide)")]
		Opal = 285,
		[Description("Pachy (Dusthide)")]
		Pachy = 290,
		[Description("Polkadot (Dusthide)")]
		Polkadot = 293,
		[Description("Ringlets (Dusthide)")]
		Ringlets = 294,
		[Description("Smoke (Dusthide)")]
		Smoke = 297,
		[Description("Sparkle (Dusthide)")]
		Sparkle = 292,
		[Description("Spines (Dusthide)")]
		Spines = 298,
		[Description("Spores (Dusthide)")]
		Spores = 295,
		[Description("Stained (Dusthide)")]
		Stained = 274,
		[Description("Topcoat (Dusthide)")]
		Topcoat = 299,
		[Description("Underbelly (Dusthide)")]
		Underbelly = 275,
		[Description("Veil (Dusthide)")]
		Veil = 296,
		[Description("Wavecrest (Dusthide)")]
		Wavecrest = 300,
		[Description("Whiskers (Dusthide)")]
		Whiskers = 291,
	}
	
	public enum GaolerTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Angler (Gaoler)")]
		Angler = 156,
		[Description("Blossom (Gaoler)")]
		Blossom = 36,
		[Description("Braids (Gaoler)")]
		Braids = 55,
		[Description("Capsule (Gaoler)")]
		Capsule = 75,
		[Description("Carnivore (Gaoler)")]
		Carnivore = 166,
		[Description("Contour (Gaoler)")]
		Contour = 157,
		[Description("Crystalline (Gaoler)")]
		Crystalline = 265,
		[Description("Fans (Gaoler)")]
		Fans = 3,
		[Description("Firebreather (Gaoler)")]
		Firebreather = 303,
		[Description("Ghost (Gaoler)")]
		Ghost = 25,
		[Description("Glimmer (Gaoler)")]
		Glimmer = 101,
		[Description("Gnarlhorns (Gaoler)")]
		Gnarlhorns = 27,
		[Description("Opal (Gaoler)")]
		Opal = 37,
		[Description("Pinions (Gaoler)")]
		Pinions = 77,
		[Description("Ringlets (Gaoler)")]
		Ringlets = 30,
		[Description("Riot (Gaoler)")]
		Riot = 212,
		[Description("Runes (Gaoler)")]
		Runes = 32,
		[Description("Scorpion (Gaoler)")]
		Scorpion = 33,
		[Description("Shardflank (Gaoler)")]
		Shardflank = 26,
		[Description("Smoke (Gaoler)")]
		Smoke = 28,
		[Description("Sparkle (Gaoler)")]
		Sparkle = 99,
		[Description("Stained (Gaoler)")]
		Stained = 71,
		[Description("Thundercrack (Gaoler)")]
		Thundercrack = 195,
		[Description("Thylacine (Gaoler)")]
		Thylacine = 29,
		[Description("Underbelly (Gaoler)")]
		Underbelly = 31,
		[Description("Veined (Gaoler)")]
		Veined = 2,
		[Description("Weathered (Gaoler)")]
		Weathered = 35,
		[Description("Wintercoat (Gaoler)")]
		Wintercoat = 34,
	}
	
	public enum UndertideTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Angler (Undertide)")]
		Angler = 246,
		[Description("Blossom (Undertide)")]
		Blossom = 267,
		[Description("Branches (Undertide)")]
		Branches = 248,
		[Description("Brightshine (Undertide)")]
		Brightshine = 170,
		[Description("Capsule (Undertide)")]
		Capsule = 111,
		[Description("Carnivore (Undertide)")]
		Carnivore = 165,
		[Description("Circuit (Undertide)")]
		Circuit = 117,
		[Description("Contour (Undertide)")]
		Contour = 249,
		[Description("Crackle (Undertide)")]
		Crackle = 115,
		[Description("Crest (Undertide)")]
		Crest = 250,
		[Description("Fans (Undertide)")]
		Fans = 251,
		[Description("Featherbeard (Undertide)")]
		Featherbeard = 118,
		[Description("Filigree (Undertide)")]
		Filigree = 116,
		[Description("Firebreather (Undertide)")]
		Firebreather = 252,
		[Description("Firefly (Undertide)")]
		Firefly = 253,
		[Description("Flecks (Undertide)")]
		Flecks = 112,
		[Description("Gembond (Undertide)")]
		Gembond = 123,
		[Description("Ghost (Undertide)")]
		Ghost = 121,
		[Description("Jellyfish (Undertide)")]
		Jellyfish = 269,
		[Description("Keel (Undertide)")]
		Keel = 268,
		[Description("Koi (Undertide)")]
		Koi = 254,
		[Description("Medusa (Undertide)")]
		Medusa = 255,
		[Description("Mistral (Undertide)")]
		Mistral = 272,
		[Description("Nudibranch (Undertide)")]
		Nudibranch = 126,
		[Description("Okapi (Undertide)")]
		Okapi = 129,
		[Description("Opal (Undertide)")]
		Opal = 247,
		[Description("Plating (Undertide)")]
		Plating = 128,
		[Description("Porcupine (Undertide)")]
		Porcupine = 256,
		[Description("Pufferfish (Undertide)")]
		Pufferfish = 127,
		[Description("Remora (Undertide)")]
		Remora = 119,
		[Description("Ringlets (Undertide)")]
		Ringlets = 120,
		[Description("Runes (Undertide)")]
		Runes = 114,
		[Description("Sailfin (Undertide)")]
		Sailfin = 130,
		[Description("Scales (Undertide)")]
		Scales = 257,
		[Description("Shark (Undertide)")]
		Shark = 258,
		[Description("Smirch (Undertide)")]
		Smirch = 259,
		[Description("Smoke (Undertide)")]
		Smoke = 260,
		[Description("Soap (Undertide)")]
		Soap = 124,
		[Description("Sparkle (Undertide)")]
		Sparkle = 122,
		[Description("Spines (Undertide)")]
		Spines = 261,
		[Description("Stained (Undertide)")]
		Stained = 110,
		[Description("Stinger (Undertide)")]
		Stinger = 262,
		[Description("Tentacles (Undertide)")]
		Tentacles = 125,
		[Description("Thorns (Undertide)")]
		Thorns = 263,
		[Description("Topcoat (Undertide)")]
		Topcoat = 264,
		[Description("Underbelly (Undertide)")]
		Underbelly = 109,
		[Description("Veined (Undertide)")]
		Veined = 113,
		[Description("Wavecrest (Undertide)")]
		Wavecrest = 301,
	}
	
	public enum VeilspunTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Angler (Veilspun)")]
		Angler = 78,
		[Description("Beetle (Veilspun)")]
		Beetle = 65,
		[Description("Branches (Veilspun)")]
		Branches = 63,
		[Description("Brightshine (Veilspun)")]
		Brightshine = 169,
		[Description("Capsule (Veilspun)")]
		Capsule = 56,
		[Description("Carnivore (Veilspun)")]
		Carnivore = 167,
		[Description("Crackle (Veilspun)")]
		Crackle = 58,
		[Description("Diaphanous (Veilspun)")]
		Diaphanous = 66,
		[Description("Filigree (Veilspun)")]
		Filigree = 133,
		[Description("Firebreather (Veilspun)")]
		Firebreather = 302,
		[Description("Firefly (Veilspun)")]
		Firefly = 61,
		[Description("Flecks (Veilspun)")]
		Flecks = 64,
		[Description("Ghost (Veilspun)")]
		Ghost = 131,
		[Description("Glimmer (Veilspun)")]
		Glimmer = 102,
		[Description("Koi (Veilspun)")]
		Koi = 108,
		[Description("Mop (Veilspun)")]
		Mop = 67,
		[Description("Okapi (Veilspun)")]
		Okapi = 59,
		[Description("Opal (Veilspun)")]
		Opal = 62,
		[Description("Peacock (Veilspun)")]
		Peacock = 60,
		[Description("Runes (Veilspun)")]
		Runes = 57,
		[Description("Sparkle (Veilspun)")]
		Sparkle = 100,
		[Description("Stained (Veilspun)")]
		Stained = 72,
		[Description("Thorns (Veilspun)")]
		Thorns = 68,
		[Description("Trickmurk (Veilspun)")]
		Trickmurk = 270,
		[Description("Underbelly (Veilspun)")]
		Underbelly = 70,
		[Description("Veined (Veilspun)")]
		Veined = 134,
	}
	
	public enum AberrationTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Augment (Aberration)")]
		Augment = 198,
		[Description("Braids (Aberration)")]
		Braids = 199,
		[Description("Capsule (Aberration)")]
		Capsule = 83,
		[Description("Carnivore (Aberration)")]
		Carnivore = 162,
		[Description("Contour (Aberration)")]
		Contour = 200,
		[Description("Fangs (Aberration)")]
		Fangs = 84,
		[Description("Fans (Aberration)")]
		Fans = 201,
		[Description("Firebreather (Aberration)")]
		Firebreather = 202,
		[Description("Firefly (Aberration)")]
		Firefly = 85,
		[Description("Flameforger (Aberration)")]
		Flameforger = 197,
		[Description("Flecks (Aberration)")]
		Flecks = 104,
		[Description("Frills (Aberration)")]
		Frills = 86,
		[Description("Ghost (Aberration)")]
		Ghost = 88,
		[Description("Glimmer (Aberration)")]
		Glimmer = 94,
		[Description("Glowtail (Aberration)")]
		Glowtail = 89,
		[Description("Jewels (Aberration)")]
		Jewels = 87,
		[Description("Koi (Aberration)")]
		Koi = 203,
		[Description("Kumo (Aberration)")]
		Kumo = 80,
		[Description("Mucous (Aberration)")]
		Mucous = 81,
		[Description("Peacock (Aberration)")]
		Peacock = 90,
		[Description("Polkadot (Aberration)")]
		Polkadot = 79,
		[Description("Polypore (Aberration)")]
		Polypore = 82,
		[Description("Riot (Aberration)")]
		Riot = 211,
		[Description("Scales (Aberration)")]
		Scales = 92,
		[Description("Skeletal (Aberration)")]
		Skeletal = 204,
		[Description("Smirch (Aberration)")]
		Smirch = 205,
		[Description("Sparkle (Aberration)")]
		Sparkle = 96,
		[Description("Spines (Aberration)")]
		Spines = 206,
		[Description("Stained (Aberration)")]
		Stained = 207,
		[Description("Thorns (Aberration)")]
		Thorns = 208,
		[Description("Thylacine (Aberration)")]
		Thylacine = 93,
		[Description("Underbelly (Aberration)")]
		Underbelly = 132,
		[Description("Veined (Aberration)")]
		Veined = 91,
	}
	
	public enum SandsurgeTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Augment (Sandsurge)")]
		Augment = 173,
		[Description("Beard (Sandsurge)")]
		Beard = 174,
		[Description("Branches (Sandsurge)")]
		Branches = 189,
		[Description("Chitin (Sandsurge)")]
		Chitin = 183,
		[Description("Crest (Sandsurge)")]
		Crest = 184,
		[Description("Darts (Sandsurge)")]
		Darts = 177,
		[Description("Firebreather (Sandsurge)")]
		Firebreather = 305,
		[Description("Fishbone (Sandsurge)")]
		Fishbone = 185,
		[Description("Gembond (Sandsurge)")]
		Gembond = 176,
		[Description("Keel (Sandsurge)")]
		Keel = 186,
		[Description("Kumo (Sandsurge)")]
		Kumo = 175,
		[Description("Lace (Sandsurge)")]
		Lace = 187,
		[Description("Okapi (Sandsurge)")]
		Okapi = 182,
		[Description("Peacock (Sandsurge)")]
		Peacock = 191,
		[Description("Rockbreaker (Sandsurge)")]
		Rockbreaker = 245,
		[Description("Runes (Sandsurge)")]
		Runes = 178,
		[Description("Shark (Sandsurge)")]
		Shark = 190,
		[Description("Smirch (Sandsurge)")]
		Smirch = 192,
		[Description("Soap (Sandsurge)")]
		Soap = 180,
		[Description("Sparkle (Sandsurge)")]
		Sparkle = 193,
		[Description("Spectre (Sandsurge)")]
		Spectre = 188,
		[Description("Spines (Sandsurge)")]
		Spines = 181,
		[Description("Stained (Sandsurge)")]
		Stained = 172,
		[Description("Starfall (Sandsurge)")]
		Starfall = 210,
		[Description("Thundercrack (Sandsurge)")]
		Thundercrack = 194,
		[Description("Thylacine (Sandsurge)")]
		Thylacine = 179,
		[Description("Underbelly (Sandsurge)")]
		Underbelly = 171,
	}
	
	public enum AuraboaTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Batty (Auraboa)")]
		Batty = 214,
		[Description("Branches (Auraboa)")]
		Branches = 217,
		[Description("Capsule (Auraboa)")]
		Capsule = 229,
		[Description("Contour (Auraboa)")]
		Contour = 230,
		[Description("Crackle (Auraboa)")]
		Crackle = 231,
		[Description("Crest (Auraboa)")]
		Crest = 215,
		[Description("Crystalline (Auraboa)")]
		Crystalline = 266,
		[Description("Firebreather (Auraboa)")]
		Firebreather = 233,
		[Description("Firefly (Auraboa)")]
		Firefly = 232,
		[Description("Fishbone (Auraboa)")]
		Fishbone = 216,
		[Description("Greenskeeper (Auraboa)")]
		Greenskeeper = 308,
		[Description("Keel (Auraboa)")]
		Keel = 234,
		[Description("Koi (Auraboa)")]
		Koi = 235,
		[Description("Medusa (Auraboa)")]
		Medusa = 218,
		[Description("Opal (Auraboa)")]
		Opal = 236,
		[Description("Paradise (Auraboa)")]
		Paradise = 219,
		[Description("Peacock (Auraboa)")]
		Peacock = 237,
		[Description("Plumage (Auraboa)")]
		Plumage = 220,
		[Description("Polkadot (Auraboa)")]
		Polkadot = 238,
		[Description("Porcupine (Auraboa)")]
		Porcupine = 221,
		[Description("Rockbreaker (Auraboa)")]
		Rockbreaker = 244,
		[Description("Sailfin (Auraboa)")]
		Sailfin = 222,
		[Description("Scales (Auraboa)")]
		Scales = 239,
		[Description("Scuttle (Auraboa)")]
		Scuttle = 223,
		[Description("Smoke (Auraboa)")]
		Smoke = 240,
		[Description("Spines (Auraboa)")]
		Spines = 241,
		[Description("Stained (Auraboa)")]
		Stained = 242,
		[Description("Stinger (Auraboa)")]
		Stinger = 224,
		[Description("Terracotta (Auraboa)")]
		Terracotta = 225,
		[Description("Thorns (Auraboa)")]
		Thorns = 226,
		[Description("Topcoat (Auraboa)")]
		Topcoat = 243,
		[Description("Underbelly (Auraboa)")]
		Underbelly = 228,
		[Description("Willow (Auraboa)")]
		Willow = 227,
	}
	
	public enum BanescaleTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Capsule (Banescale)")]
		Capsule = 74,
		[Description("Carnivore (Banescale)")]
		Carnivore = 164,
		[Description("Contour (Banescale)")]
		Contour = 46,
		[Description("Crackle (Banescale)")]
		Crackle = 50,
		[Description("Fans (Banescale)")]
		Fans = 41,
		[Description("Filigree (Banescale)")]
		Filigree = 43,
		[Description("Firebreather (Banescale)")]
		Firebreather = 304,
		[Description("Flameforger (Banescale)")]
		Flameforger = 196,
		[Description("Ghost (Banescale)")]
		Ghost = 47,
		[Description("Gliders (Banescale)")]
		Gliders = 76,
		[Description("Glimmer (Banescale)")]
		Glimmer = 95,
		[Description("Lace (Banescale)")]
		Lace = 44,
		[Description("Mistral (Banescale)")]
		Mistral = 273,
		[Description("Monarch (Banescale)")]
		Monarch = 158,
		[Description("Peacock (Banescale)")]
		Peacock = 106,
		[Description("Plumage (Banescale)")]
		Plumage = 51,
		[Description("Porcupine (Banescale)")]
		Porcupine = 49,
		[Description("Ringlets (Banescale)")]
		Ringlets = 40,
		[Description("Skeletal (Banescale)")]
		Skeletal = 45,
		[Description("Soap (Banescale)")]
		Soap = 159,
		[Description("Sparkle (Banescale)")]
		Sparkle = 98,
		[Description("Spines (Banescale)")]
		Spines = 160,
		[Description("Squiggle (Banescale)")]
		Squiggle = 42,
		[Description("Stained (Banescale)")]
		Stained = 69,
		[Description("Trimmings (Banescale)")]
		Trimmings = 39,
		[Description("Underbelly (Banescale)")]
		Underbelly = 52,
		[Description("Wraith (Banescale)")]
		Wraith = 48,
	}
	
	public enum AetherTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Carnivore (Aether)")]
		Carnivore = 163,
		[Description("Circuit (Aether)")]
		Circuit = 135,
		[Description("Contour (Aether)")]
		Contour = 136,
		[Description("Firebreather (Aether)")]
		Firebreather = 306,
		[Description("Flutter (Aether)")]
		Flutter = 141,
		[Description("Gembond (Aether)")]
		Gembond = 137,
		[Description("Glowtail (Aether)")]
		Glowtail = 138,
		[Description("Keel (Aether)")]
		Keel = 139,
		[Description("Lace (Aether)")]
		Lace = 142,
		[Description("Mandibles (Aether)")]
		Mandibles = 143,
		[Description("Monarch (Aether)")]
		Monarch = 140,
		[Description("Points (Aether)")]
		Points = 146,
		[Description("Scales (Aether)")]
		Scales = 147,
		[Description("Smirch (Aether)")]
		Smirch = 150,
		[Description("Smoke (Aether)")]
		Smoke = 151,
		[Description("Space (Aether)")]
		Space = 149,
		[Description("Sparkle (Aether)")]
		Sparkle = 152,
		[Description("Spines (Aether)")]
		Spines = 153,
		[Description("Stained (Aether)")]
		Stained = 145,
		[Description("Starfall (Aether)")]
		Starfall = 209,
		[Description("Stinger (Aether)")]
		Stinger = 148,
		[Description("Trickmurk (Aether)")]
		Trickmurk = 271,
		[Description("Underbelly (Aether)")]
		Underbelly = 144,
		[Description("Whiskers (Aether)")]
		Whiskers = 154,
		[Description("Wish (Aether)")]
		Wish = 155,
	}
	
	public enum AllTertiaryGene
	{
		[Description("Basic")]
		[Order(0)]
		[Gene(DragonType.Aberration, DragonType.Aether, DragonType.Auraboa, DragonType.Banescale, DragonType.Bogsneak, DragonType.Coatl, DragonType.Dusthide, DragonType.Fae, DragonType.Fathom, DragonType.Gaoler, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Sandsurge, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Undertide, DragonType.Veilspun, DragonType.Wildclaw)]
		Basic = 0,
		[Description("Circuit")]
		[Order(1)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Circuit = 1,
		[Description("Gembond")]
		[Order(2)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Gembond = 4,
		[Description("Underbelly")]
		[Order(3)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Underbelly = 5,
		[Description("Crackle")]
		[Order(4)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Crackle = 6,
		[Description("Smoke")]
		[Order(5)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Smoke = 7,
		[Description("Spines")]
		[Order(6)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Spines = 8,
		[Description("Okapi")]
		[Order(7)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Okapi = 9,
		[Description("Glimmer")]
		[Order(8)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Glimmer = 10,
		[Description("Thylacine")]
		[Order(9)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Thylacine = 11,
		[Description("Stained")]
		[Order(10)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Stained = 12,
		[Description("Contour")]
		[Order(11)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Contour = 13,
		[Description("Runes")]
		[Order(12)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Runes = 14,
		[Description("Scales")]
		[Order(13)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Scales = 15,
		[Description("Lace")]
		[Order(14)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Lace = 16,
		[Description("Opal")]
		[Order(15)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Opal = 17,
		[Description("Capsule")]
		[Order(16)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Capsule = 18,
		[Description("Smirch")]
		[Order(17)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Smirch = 19,
		[Description("Ghost")]
		[Order(18)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Ghost = 20,
		[Description("Filigree")]
		[Order(19)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Filigree = 21,
		[Description("Firefly")]
		[Order(20)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Firefly = 22,
		[Description("Ringlets")]
		[Order(21)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Ringlets = 23,
		[Description("Peacock")]
		[Order(22)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Peacock = 24,
		[Description("Veined")]
		[Order(23)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Veined = 38,
		[Description("Keel")]
		[Order(24)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Keel = 53,
		[Description("Glowtail")]
		[Order(25)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Glowtail = 54,
		[Description("Koi")]
		[Order(26)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Koi = 73,
		[Description("Sparkle")]
		[Order(27)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Sparkle = 97,
		[Description("Flecks")]
		[Order(28)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Flecks = 103,
		[Description("Soap")]
		[Order(29)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Soap = 105,
		[Description("Points")]
		[Order(30)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Points = 107,
		[Description("Firebreather")]
		[Order(31)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Firebreather = 161,
		[Description("Polkadot")]
		[Order(32)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Polkadot = 168,
		[Description("Wish")]
		[Order(33)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Fathom, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Wish = 213,
		[Description("Angler (Dusthide)")]
		[Order(34)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Angler = 286,
		[Description("Antlers (Dusthide)")]
		[Order(35)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Antlers = 276,
		[Description("Augment (Dusthide)")]
		[Order(36)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Augment = 280,
		[Description("Batty (Dusthide)")]
		[Order(37)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Batty = 289,
		[Description("Blossom (Dusthide)")]
		[Order(38)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Blossom = 281,
		[Description("Carnivore (Dusthide)")]
		[Order(39)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Carnivore = 282,
		[Description("Dewlap (Dusthide)")]
		[Order(40)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Dewlap = 288,
		[Description("Fishbone (Dusthide)")]
		[Order(41)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Fishbone = 277,
		[Description("Gembond (Dusthide)")]
		[Order(42)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Gembond = 287,
		[Description("Ghost (Dusthide)")]
		[Order(43)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Ghost = 278,
		[Description("Glowtail (Dusthide)")]
		[Order(44)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Glowtail = 279,
		[Description("Greenskeeper (Dusthide)")]
		[Order(45)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Greenskeeper = 307,
		[Description("Mandibles (Dusthide)")]
		[Order(46)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Mandibles = 283,
		[Description("Okapi (Dusthide)")]
		[Order(47)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Okapi = 284,
		[Description("Opal (Dusthide)")]
		[Order(48)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Opal = 285,
		[Description("Pachy (Dusthide)")]
		[Order(49)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Pachy = 290,
		[Description("Polkadot (Dusthide)")]
		[Order(50)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Polkadot = 293,
		[Description("Ringlets (Dusthide)")]
		[Order(51)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Ringlets = 294,
		[Description("Smoke (Dusthide)")]
		[Order(52)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Smoke = 297,
		[Description("Sparkle (Dusthide)")]
		[Order(53)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Sparkle = 292,
		[Description("Spines (Dusthide)")]
		[Order(54)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Spines = 298,
		[Description("Spores (Dusthide)")]
		[Order(55)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Spores = 295,
		[Description("Stained (Dusthide)")]
		[Order(56)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Stained = 274,
		[Description("Topcoat (Dusthide)")]
		[Order(57)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Topcoat = 299,
		[Description("Underbelly (Dusthide)")]
		[Order(58)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Underbelly = 275,
		[Description("Veil (Dusthide)")]
		[Order(59)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Veil = 296,
		[Description("Wavecrest (Dusthide)")]
		[Order(60)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Wavecrest = 300,
		[Description("Whiskers (Dusthide)")]
		[Order(61)]
		[Gene(DragonType.Dusthide)]
		Dusthide_Whiskers = 291,
		[Description("Angler (Gaoler)")]
		[Order(62)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Angler = 156,
		[Description("Blossom (Gaoler)")]
		[Order(63)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Blossom = 36,
		[Description("Braids (Gaoler)")]
		[Order(64)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Braids = 55,
		[Description("Capsule (Gaoler)")]
		[Order(65)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Capsule = 75,
		[Description("Carnivore (Gaoler)")]
		[Order(66)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Carnivore = 166,
		[Description("Contour (Gaoler)")]
		[Order(67)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Contour = 157,
		[Description("Crystalline (Gaoler)")]
		[Order(68)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Crystalline = 265,
		[Description("Fans (Gaoler)")]
		[Order(69)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Fans = 3,
		[Description("Firebreather (Gaoler)")]
		[Order(70)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Firebreather = 303,
		[Description("Ghost (Gaoler)")]
		[Order(71)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Ghost = 25,
		[Description("Glimmer (Gaoler)")]
		[Order(72)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Glimmer = 101,
		[Description("Gnarlhorns (Gaoler)")]
		[Order(73)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Gnarlhorns = 27,
		[Description("Opal (Gaoler)")]
		[Order(74)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Opal = 37,
		[Description("Pinions (Gaoler)")]
		[Order(75)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Pinions = 77,
		[Description("Ringlets (Gaoler)")]
		[Order(76)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Ringlets = 30,
		[Description("Riot (Gaoler)")]
		[Order(77)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Riot = 212,
		[Description("Runes (Gaoler)")]
		[Order(78)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Runes = 32,
		[Description("Scorpion (Gaoler)")]
		[Order(79)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Scorpion = 33,
		[Description("Shardflank (Gaoler)")]
		[Order(80)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Shardflank = 26,
		[Description("Smoke (Gaoler)")]
		[Order(81)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Smoke = 28,
		[Description("Sparkle (Gaoler)")]
		[Order(82)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Sparkle = 99,
		[Description("Stained (Gaoler)")]
		[Order(83)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Stained = 71,
		[Description("Thundercrack (Gaoler)")]
		[Order(84)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Thundercrack = 195,
		[Description("Thylacine (Gaoler)")]
		[Order(85)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Thylacine = 29,
		[Description("Underbelly (Gaoler)")]
		[Order(86)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Underbelly = 31,
		[Description("Veined (Gaoler)")]
		[Order(87)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Veined = 2,
		[Description("Weathered (Gaoler)")]
		[Order(88)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Weathered = 35,
		[Description("Wintercoat (Gaoler)")]
		[Order(89)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Wintercoat = 34,
		[Description("Angler (Undertide)")]
		[Order(90)]
		[Gene(DragonType.Undertide)]
		Undertide_Angler = 246,
		[Description("Blossom (Undertide)")]
		[Order(91)]
		[Gene(DragonType.Undertide)]
		Undertide_Blossom = 267,
		[Description("Branches (Undertide)")]
		[Order(92)]
		[Gene(DragonType.Undertide)]
		Undertide_Branches = 248,
		[Description("Brightshine (Undertide)")]
		[Order(93)]
		[Gene(DragonType.Undertide)]
		Undertide_Brightshine = 170,
		[Description("Capsule (Undertide)")]
		[Order(94)]
		[Gene(DragonType.Undertide)]
		Undertide_Capsule = 111,
		[Description("Carnivore (Undertide)")]
		[Order(95)]
		[Gene(DragonType.Undertide)]
		Undertide_Carnivore = 165,
		[Description("Circuit (Undertide)")]
		[Order(96)]
		[Gene(DragonType.Undertide)]
		Undertide_Circuit = 117,
		[Description("Contour (Undertide)")]
		[Order(97)]
		[Gene(DragonType.Undertide)]
		Undertide_Contour = 249,
		[Description("Crackle (Undertide)")]
		[Order(98)]
		[Gene(DragonType.Undertide)]
		Undertide_Crackle = 115,
		[Description("Crest (Undertide)")]
		[Order(99)]
		[Gene(DragonType.Undertide)]
		Undertide_Crest = 250,
		[Description("Fans (Undertide)")]
		[Order(100)]
		[Gene(DragonType.Undertide)]
		Undertide_Fans = 251,
		[Description("Featherbeard (Undertide)")]
		[Order(101)]
		[Gene(DragonType.Undertide)]
		Undertide_Featherbeard = 118,
		[Description("Filigree (Undertide)")]
		[Order(102)]
		[Gene(DragonType.Undertide)]
		Undertide_Filigree = 116,
		[Description("Firebreather (Undertide)")]
		[Order(103)]
		[Gene(DragonType.Undertide)]
		Undertide_Firebreather = 252,
		[Description("Firefly (Undertide)")]
		[Order(104)]
		[Gene(DragonType.Undertide)]
		Undertide_Firefly = 253,
		[Description("Flecks (Undertide)")]
		[Order(105)]
		[Gene(DragonType.Undertide)]
		Undertide_Flecks = 112,
		[Description("Gembond (Undertide)")]
		[Order(106)]
		[Gene(DragonType.Undertide)]
		Undertide_Gembond = 123,
		[Description("Ghost (Undertide)")]
		[Order(107)]
		[Gene(DragonType.Undertide)]
		Undertide_Ghost = 121,
		[Description("Jellyfish (Undertide)")]
		[Order(108)]
		[Gene(DragonType.Undertide)]
		Undertide_Jellyfish = 269,
		[Description("Keel (Undertide)")]
		[Order(109)]
		[Gene(DragonType.Undertide)]
		Undertide_Keel = 268,
		[Description("Koi (Undertide)")]
		[Order(110)]
		[Gene(DragonType.Undertide)]
		Undertide_Koi = 254,
		[Description("Medusa (Undertide)")]
		[Order(111)]
		[Gene(DragonType.Undertide)]
		Undertide_Medusa = 255,
		[Description("Mistral (Undertide)")]
		[Order(112)]
		[Gene(DragonType.Undertide)]
		Undertide_Mistral = 272,
		[Description("Nudibranch (Undertide)")]
		[Order(113)]
		[Gene(DragonType.Undertide)]
		Undertide_Nudibranch = 126,
		[Description("Okapi (Undertide)")]
		[Order(114)]
		[Gene(DragonType.Undertide)]
		Undertide_Okapi = 129,
		[Description("Opal (Undertide)")]
		[Order(115)]
		[Gene(DragonType.Undertide)]
		Undertide_Opal = 247,
		[Description("Plating (Undertide)")]
		[Order(116)]
		[Gene(DragonType.Undertide)]
		Undertide_Plating = 128,
		[Description("Porcupine (Undertide)")]
		[Order(117)]
		[Gene(DragonType.Undertide)]
		Undertide_Porcupine = 256,
		[Description("Pufferfish (Undertide)")]
		[Order(118)]
		[Gene(DragonType.Undertide)]
		Undertide_Pufferfish = 127,
		[Description("Remora (Undertide)")]
		[Order(119)]
		[Gene(DragonType.Undertide)]
		Undertide_Remora = 119,
		[Description("Ringlets (Undertide)")]
		[Order(120)]
		[Gene(DragonType.Undertide)]
		Undertide_Ringlets = 120,
		[Description("Runes (Undertide)")]
		[Order(121)]
		[Gene(DragonType.Undertide)]
		Undertide_Runes = 114,
		[Description("Sailfin (Undertide)")]
		[Order(122)]
		[Gene(DragonType.Undertide)]
		Undertide_Sailfin = 130,
		[Description("Scales (Undertide)")]
		[Order(123)]
		[Gene(DragonType.Undertide)]
		Undertide_Scales = 257,
		[Description("Shark (Undertide)")]
		[Order(124)]
		[Gene(DragonType.Undertide)]
		Undertide_Shark = 258,
		[Description("Smirch (Undertide)")]
		[Order(125)]
		[Gene(DragonType.Undertide)]
		Undertide_Smirch = 259,
		[Description("Smoke (Undertide)")]
		[Order(126)]
		[Gene(DragonType.Undertide)]
		Undertide_Smoke = 260,
		[Description("Soap (Undertide)")]
		[Order(127)]
		[Gene(DragonType.Undertide)]
		Undertide_Soap = 124,
		[Description("Sparkle (Undertide)")]
		[Order(128)]
		[Gene(DragonType.Undertide)]
		Undertide_Sparkle = 122,
		[Description("Spines (Undertide)")]
		[Order(129)]
		[Gene(DragonType.Undertide)]
		Undertide_Spines = 261,
		[Description("Stained (Undertide)")]
		[Order(130)]
		[Gene(DragonType.Undertide)]
		Undertide_Stained = 110,
		[Description("Stinger (Undertide)")]
		[Order(131)]
		[Gene(DragonType.Undertide)]
		Undertide_Stinger = 262,
		[Description("Tentacles (Undertide)")]
		[Order(132)]
		[Gene(DragonType.Undertide)]
		Undertide_Tentacles = 125,
		[Description("Thorns (Undertide)")]
		[Order(133)]
		[Gene(DragonType.Undertide)]
		Undertide_Thorns = 263,
		[Description("Topcoat (Undertide)")]
		[Order(134)]
		[Gene(DragonType.Undertide)]
		Undertide_Topcoat = 264,
		[Description("Underbelly (Undertide)")]
		[Order(135)]
		[Gene(DragonType.Undertide)]
		Undertide_Underbelly = 109,
		[Description("Veined (Undertide)")]
		[Order(136)]
		[Gene(DragonType.Undertide)]
		Undertide_Veined = 113,
		[Description("Wavecrest (Undertide)")]
		[Order(137)]
		[Gene(DragonType.Undertide)]
		Undertide_Wavecrest = 301,
		[Description("Angler (Veilspun)")]
		[Order(138)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Angler = 78,
		[Description("Beetle (Veilspun)")]
		[Order(139)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Beetle = 65,
		[Description("Branches (Veilspun)")]
		[Order(140)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Branches = 63,
		[Description("Brightshine (Veilspun)")]
		[Order(141)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Brightshine = 169,
		[Description("Capsule (Veilspun)")]
		[Order(142)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Capsule = 56,
		[Description("Carnivore (Veilspun)")]
		[Order(143)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Carnivore = 167,
		[Description("Crackle (Veilspun)")]
		[Order(144)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Crackle = 58,
		[Description("Diaphanous (Veilspun)")]
		[Order(145)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Diaphanous = 66,
		[Description("Filigree (Veilspun)")]
		[Order(146)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Filigree = 133,
		[Description("Firebreather (Veilspun)")]
		[Order(147)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Firebreather = 302,
		[Description("Firefly (Veilspun)")]
		[Order(148)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Firefly = 61,
		[Description("Flecks (Veilspun)")]
		[Order(149)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Flecks = 64,
		[Description("Ghost (Veilspun)")]
		[Order(150)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Ghost = 131,
		[Description("Glimmer (Veilspun)")]
		[Order(151)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Glimmer = 102,
		[Description("Koi (Veilspun)")]
		[Order(152)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Koi = 108,
		[Description("Mop (Veilspun)")]
		[Order(153)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Mop = 67,
		[Description("Okapi (Veilspun)")]
		[Order(154)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Okapi = 59,
		[Description("Opal (Veilspun)")]
		[Order(155)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Opal = 62,
		[Description("Peacock (Veilspun)")]
		[Order(156)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Peacock = 60,
		[Description("Runes (Veilspun)")]
		[Order(157)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Runes = 57,
		[Description("Sparkle (Veilspun)")]
		[Order(158)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Sparkle = 100,
		[Description("Stained (Veilspun)")]
		[Order(159)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Stained = 72,
		[Description("Thorns (Veilspun)")]
		[Order(160)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Thorns = 68,
		[Description("Trickmurk (Veilspun)")]
		[Order(161)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Trickmurk = 270,
		[Description("Underbelly (Veilspun)")]
		[Order(162)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Underbelly = 70,
		[Description("Veined (Veilspun)")]
		[Order(163)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Veined = 134,
		[Description("Augment (Aberration)")]
		[Order(164)]
		[Gene(DragonType.Aberration)]
		Aberration_Augment = 198,
		[Description("Braids (Aberration)")]
		[Order(165)]
		[Gene(DragonType.Aberration)]
		Aberration_Braids = 199,
		[Description("Capsule (Aberration)")]
		[Order(166)]
		[Gene(DragonType.Aberration)]
		Aberration_Capsule = 83,
		[Description("Carnivore (Aberration)")]
		[Order(167)]
		[Gene(DragonType.Aberration)]
		Aberration_Carnivore = 162,
		[Description("Contour (Aberration)")]
		[Order(168)]
		[Gene(DragonType.Aberration)]
		Aberration_Contour = 200,
		[Description("Fangs (Aberration)")]
		[Order(169)]
		[Gene(DragonType.Aberration)]
		Aberration_Fangs = 84,
		[Description("Fans (Aberration)")]
		[Order(170)]
		[Gene(DragonType.Aberration)]
		Aberration_Fans = 201,
		[Description("Firebreather (Aberration)")]
		[Order(171)]
		[Gene(DragonType.Aberration)]
		Aberration_Firebreather = 202,
		[Description("Firefly (Aberration)")]
		[Order(172)]
		[Gene(DragonType.Aberration)]
		Aberration_Firefly = 85,
		[Description("Flameforger (Aberration)")]
		[Order(173)]
		[Gene(DragonType.Aberration)]
		Aberration_Flameforger = 197,
		[Description("Flecks (Aberration)")]
		[Order(174)]
		[Gene(DragonType.Aberration)]
		Aberration_Flecks = 104,
		[Description("Frills (Aberration)")]
		[Order(175)]
		[Gene(DragonType.Aberration)]
		Aberration_Frills = 86,
		[Description("Ghost (Aberration)")]
		[Order(176)]
		[Gene(DragonType.Aberration)]
		Aberration_Ghost = 88,
		[Description("Glimmer (Aberration)")]
		[Order(177)]
		[Gene(DragonType.Aberration)]
		Aberration_Glimmer = 94,
		[Description("Glowtail (Aberration)")]
		[Order(178)]
		[Gene(DragonType.Aberration)]
		Aberration_Glowtail = 89,
		[Description("Jewels (Aberration)")]
		[Order(179)]
		[Gene(DragonType.Aberration)]
		Aberration_Jewels = 87,
		[Description("Koi (Aberration)")]
		[Order(180)]
		[Gene(DragonType.Aberration)]
		Aberration_Koi = 203,
		[Description("Kumo (Aberration)")]
		[Order(181)]
		[Gene(DragonType.Aberration)]
		Aberration_Kumo = 80,
		[Description("Mucous (Aberration)")]
		[Order(182)]
		[Gene(DragonType.Aberration)]
		Aberration_Mucous = 81,
		[Description("Peacock (Aberration)")]
		[Order(183)]
		[Gene(DragonType.Aberration)]
		Aberration_Peacock = 90,
		[Description("Polkadot (Aberration)")]
		[Order(184)]
		[Gene(DragonType.Aberration)]
		Aberration_Polkadot = 79,
		[Description("Polypore (Aberration)")]
		[Order(185)]
		[Gene(DragonType.Aberration)]
		Aberration_Polypore = 82,
		[Description("Riot (Aberration)")]
		[Order(186)]
		[Gene(DragonType.Aberration)]
		Aberration_Riot = 211,
		[Description("Scales (Aberration)")]
		[Order(187)]
		[Gene(DragonType.Aberration)]
		Aberration_Scales = 92,
		[Description("Skeletal (Aberration)")]
		[Order(188)]
		[Gene(DragonType.Aberration)]
		Aberration_Skeletal = 204,
		[Description("Smirch (Aberration)")]
		[Order(189)]
		[Gene(DragonType.Aberration)]
		Aberration_Smirch = 205,
		[Description("Sparkle (Aberration)")]
		[Order(190)]
		[Gene(DragonType.Aberration)]
		Aberration_Sparkle = 96,
		[Description("Spines (Aberration)")]
		[Order(191)]
		[Gene(DragonType.Aberration)]
		Aberration_Spines = 206,
		[Description("Stained (Aberration)")]
		[Order(192)]
		[Gene(DragonType.Aberration)]
		Aberration_Stained = 207,
		[Description("Thorns (Aberration)")]
		[Order(193)]
		[Gene(DragonType.Aberration)]
		Aberration_Thorns = 208,
		[Description("Thylacine (Aberration)")]
		[Order(194)]
		[Gene(DragonType.Aberration)]
		Aberration_Thylacine = 93,
		[Description("Underbelly (Aberration)")]
		[Order(195)]
		[Gene(DragonType.Aberration)]
		Aberration_Underbelly = 132,
		[Description("Veined (Aberration)")]
		[Order(196)]
		[Gene(DragonType.Aberration)]
		Aberration_Veined = 91,
		[Description("Augment (Sandsurge)")]
		[Order(197)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Augment = 173,
		[Description("Beard (Sandsurge)")]
		[Order(198)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Beard = 174,
		[Description("Branches (Sandsurge)")]
		[Order(199)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Branches = 189,
		[Description("Chitin (Sandsurge)")]
		[Order(200)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Chitin = 183,
		[Description("Crest (Sandsurge)")]
		[Order(201)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Crest = 184,
		[Description("Darts (Sandsurge)")]
		[Order(202)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Darts = 177,
		[Description("Firebreather (Sandsurge)")]
		[Order(203)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Firebreather = 305,
		[Description("Fishbone (Sandsurge)")]
		[Order(204)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Fishbone = 185,
		[Description("Gembond (Sandsurge)")]
		[Order(205)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Gembond = 176,
		[Description("Keel (Sandsurge)")]
		[Order(206)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Keel = 186,
		[Description("Kumo (Sandsurge)")]
		[Order(207)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Kumo = 175,
		[Description("Lace (Sandsurge)")]
		[Order(208)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Lace = 187,
		[Description("Okapi (Sandsurge)")]
		[Order(209)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Okapi = 182,
		[Description("Peacock (Sandsurge)")]
		[Order(210)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Peacock = 191,
		[Description("Rockbreaker (Sandsurge)")]
		[Order(211)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Rockbreaker = 245,
		[Description("Runes (Sandsurge)")]
		[Order(212)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Runes = 178,
		[Description("Shark (Sandsurge)")]
		[Order(213)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Shark = 190,
		[Description("Smirch (Sandsurge)")]
		[Order(214)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Smirch = 192,
		[Description("Soap (Sandsurge)")]
		[Order(215)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Soap = 180,
		[Description("Sparkle (Sandsurge)")]
		[Order(216)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Sparkle = 193,
		[Description("Spectre (Sandsurge)")]
		[Order(217)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Spectre = 188,
		[Description("Spines (Sandsurge)")]
		[Order(218)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Spines = 181,
		[Description("Stained (Sandsurge)")]
		[Order(219)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Stained = 172,
		[Description("Starfall (Sandsurge)")]
		[Order(220)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Starfall = 210,
		[Description("Thundercrack (Sandsurge)")]
		[Order(221)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Thundercrack = 194,
		[Description("Thylacine (Sandsurge)")]
		[Order(222)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Thylacine = 179,
		[Description("Underbelly (Sandsurge)")]
		[Order(223)]
		[Gene(DragonType.Sandsurge)]
		Sandsurge_Underbelly = 171,
		[Description("Batty (Auraboa)")]
		[Order(224)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Batty = 214,
		[Description("Branches (Auraboa)")]
		[Order(225)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Branches = 217,
		[Description("Capsule (Auraboa)")]
		[Order(226)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Capsule = 229,
		[Description("Contour (Auraboa)")]
		[Order(227)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Contour = 230,
		[Description("Crackle (Auraboa)")]
		[Order(228)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Crackle = 231,
		[Description("Crest (Auraboa)")]
		[Order(229)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Crest = 215,
		[Description("Crystalline (Auraboa)")]
		[Order(230)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Crystalline = 266,
		[Description("Firebreather (Auraboa)")]
		[Order(231)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Firebreather = 233,
		[Description("Firefly (Auraboa)")]
		[Order(232)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Firefly = 232,
		[Description("Fishbone (Auraboa)")]
		[Order(233)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Fishbone = 216,
		[Description("Greenskeeper (Auraboa)")]
		[Order(234)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Greenskeeper = 308,
		[Description("Keel (Auraboa)")]
		[Order(235)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Keel = 234,
		[Description("Koi (Auraboa)")]
		[Order(236)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Koi = 235,
		[Description("Medusa (Auraboa)")]
		[Order(237)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Medusa = 218,
		[Description("Opal (Auraboa)")]
		[Order(238)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Opal = 236,
		[Description("Paradise (Auraboa)")]
		[Order(239)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Paradise = 219,
		[Description("Peacock (Auraboa)")]
		[Order(240)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Peacock = 237,
		[Description("Plumage (Auraboa)")]
		[Order(241)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Plumage = 220,
		[Description("Polkadot (Auraboa)")]
		[Order(242)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Polkadot = 238,
		[Description("Porcupine (Auraboa)")]
		[Order(243)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Porcupine = 221,
		[Description("Rockbreaker (Auraboa)")]
		[Order(244)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Rockbreaker = 244,
		[Description("Sailfin (Auraboa)")]
		[Order(245)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Sailfin = 222,
		[Description("Scales (Auraboa)")]
		[Order(246)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Scales = 239,
		[Description("Scuttle (Auraboa)")]
		[Order(247)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Scuttle = 223,
		[Description("Smoke (Auraboa)")]
		[Order(248)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Smoke = 240,
		[Description("Spines (Auraboa)")]
		[Order(249)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Spines = 241,
		[Description("Stained (Auraboa)")]
		[Order(250)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Stained = 242,
		[Description("Stinger (Auraboa)")]
		[Order(251)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Stinger = 224,
		[Description("Terracotta (Auraboa)")]
		[Order(252)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Terracotta = 225,
		[Description("Thorns (Auraboa)")]
		[Order(253)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Thorns = 226,
		[Description("Topcoat (Auraboa)")]
		[Order(254)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Topcoat = 243,
		[Description("Underbelly (Auraboa)")]
		[Order(255)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Underbelly = 228,
		[Description("Willow (Auraboa)")]
		[Order(256)]
		[Gene(DragonType.Auraboa)]
		Auraboa_Willow = 227,
		[Description("Capsule (Banescale)")]
		[Order(257)]
		[Gene(DragonType.Banescale)]
		Banescale_Capsule = 74,
		[Description("Carnivore (Banescale)")]
		[Order(258)]
		[Gene(DragonType.Banescale)]
		Banescale_Carnivore = 164,
		[Description("Contour (Banescale)")]
		[Order(259)]
		[Gene(DragonType.Banescale)]
		Banescale_Contour = 46,
		[Description("Crackle (Banescale)")]
		[Order(260)]
		[Gene(DragonType.Banescale)]
		Banescale_Crackle = 50,
		[Description("Fans (Banescale)")]
		[Order(261)]
		[Gene(DragonType.Banescale)]
		Banescale_Fans = 41,
		[Description("Filigree (Banescale)")]
		[Order(262)]
		[Gene(DragonType.Banescale)]
		Banescale_Filigree = 43,
		[Description("Firebreather (Banescale)")]
		[Order(263)]
		[Gene(DragonType.Banescale)]
		Banescale_Firebreather = 304,
		[Description("Flameforger (Banescale)")]
		[Order(264)]
		[Gene(DragonType.Banescale)]
		Banescale_Flameforger = 196,
		[Description("Ghost (Banescale)")]
		[Order(265)]
		[Gene(DragonType.Banescale)]
		Banescale_Ghost = 47,
		[Description("Gliders (Banescale)")]
		[Order(266)]
		[Gene(DragonType.Banescale)]
		Banescale_Gliders = 76,
		[Description("Glimmer (Banescale)")]
		[Order(267)]
		[Gene(DragonType.Banescale)]
		Banescale_Glimmer = 95,
		[Description("Lace (Banescale)")]
		[Order(268)]
		[Gene(DragonType.Banescale)]
		Banescale_Lace = 44,
		[Description("Mistral (Banescale)")]
		[Order(269)]
		[Gene(DragonType.Banescale)]
		Banescale_Mistral = 273,
		[Description("Monarch (Banescale)")]
		[Order(270)]
		[Gene(DragonType.Banescale)]
		Banescale_Monarch = 158,
		[Description("Peacock (Banescale)")]
		[Order(271)]
		[Gene(DragonType.Banescale)]
		Banescale_Peacock = 106,
		[Description("Plumage (Banescale)")]
		[Order(272)]
		[Gene(DragonType.Banescale)]
		Banescale_Plumage = 51,
		[Description("Porcupine (Banescale)")]
		[Order(273)]
		[Gene(DragonType.Banescale)]
		Banescale_Porcupine = 49,
		[Description("Ringlets (Banescale)")]
		[Order(274)]
		[Gene(DragonType.Banescale)]
		Banescale_Ringlets = 40,
		[Description("Skeletal (Banescale)")]
		[Order(275)]
		[Gene(DragonType.Banescale)]
		Banescale_Skeletal = 45,
		[Description("Soap (Banescale)")]
		[Order(276)]
		[Gene(DragonType.Banescale)]
		Banescale_Soap = 159,
		[Description("Sparkle (Banescale)")]
		[Order(277)]
		[Gene(DragonType.Banescale)]
		Banescale_Sparkle = 98,
		[Description("Spines (Banescale)")]
		[Order(278)]
		[Gene(DragonType.Banescale)]
		Banescale_Spines = 160,
		[Description("Squiggle (Banescale)")]
		[Order(279)]
		[Gene(DragonType.Banescale)]
		Banescale_Squiggle = 42,
		[Description("Stained (Banescale)")]
		[Order(280)]
		[Gene(DragonType.Banescale)]
		Banescale_Stained = 69,
		[Description("Trimmings (Banescale)")]
		[Order(281)]
		[Gene(DragonType.Banescale)]
		Banescale_Trimmings = 39,
		[Description("Underbelly (Banescale)")]
		[Order(282)]
		[Gene(DragonType.Banescale)]
		Banescale_Underbelly = 52,
		[Description("Wraith (Banescale)")]
		[Order(283)]
		[Gene(DragonType.Banescale)]
		Banescale_Wraith = 48,
		[Description("Carnivore (Aether)")]
		[Order(284)]
		[Gene(DragonType.Aether)]
		Aether_Carnivore = 163,
		[Description("Circuit (Aether)")]
		[Order(285)]
		[Gene(DragonType.Aether)]
		Aether_Circuit = 135,
		[Description("Contour (Aether)")]
		[Order(286)]
		[Gene(DragonType.Aether)]
		Aether_Contour = 136,
		[Description("Firebreather (Aether)")]
		[Order(287)]
		[Gene(DragonType.Aether)]
		Aether_Firebreather = 306,
		[Description("Flutter (Aether)")]
		[Order(288)]
		[Gene(DragonType.Aether)]
		Aether_Flutter = 141,
		[Description("Gembond (Aether)")]
		[Order(289)]
		[Gene(DragonType.Aether)]
		Aether_Gembond = 137,
		[Description("Glowtail (Aether)")]
		[Order(290)]
		[Gene(DragonType.Aether)]
		Aether_Glowtail = 138,
		[Description("Keel (Aether)")]
		[Order(291)]
		[Gene(DragonType.Aether)]
		Aether_Keel = 139,
		[Description("Lace (Aether)")]
		[Order(292)]
		[Gene(DragonType.Aether)]
		Aether_Lace = 142,
		[Description("Mandibles (Aether)")]
		[Order(293)]
		[Gene(DragonType.Aether)]
		Aether_Mandibles = 143,
		[Description("Monarch (Aether)")]
		[Order(294)]
		[Gene(DragonType.Aether)]
		Aether_Monarch = 140,
		[Description("Points (Aether)")]
		[Order(295)]
		[Gene(DragonType.Aether)]
		Aether_Points = 146,
		[Description("Scales (Aether)")]
		[Order(296)]
		[Gene(DragonType.Aether)]
		Aether_Scales = 147,
		[Description("Smirch (Aether)")]
		[Order(297)]
		[Gene(DragonType.Aether)]
		Aether_Smirch = 150,
		[Description("Smoke (Aether)")]
		[Order(298)]
		[Gene(DragonType.Aether)]
		Aether_Smoke = 151,
		[Description("Space (Aether)")]
		[Order(299)]
		[Gene(DragonType.Aether)]
		Aether_Space = 149,
		[Description("Sparkle (Aether)")]
		[Order(300)]
		[Gene(DragonType.Aether)]
		Aether_Sparkle = 152,
		[Description("Spines (Aether)")]
		[Order(301)]
		[Gene(DragonType.Aether)]
		Aether_Spines = 153,
		[Description("Stained (Aether)")]
		[Order(302)]
		[Gene(DragonType.Aether)]
		Aether_Stained = 145,
		[Description("Starfall (Aether)")]
		[Order(303)]
		[Gene(DragonType.Aether)]
		Aether_Starfall = 209,
		[Description("Stinger (Aether)")]
		[Order(304)]
		[Gene(DragonType.Aether)]
		Aether_Stinger = 148,
		[Description("Trickmurk (Aether)")]
		[Order(305)]
		[Gene(DragonType.Aether)]
		Aether_Trickmurk = 271,
		[Description("Underbelly (Aether)")]
		[Order(306)]
		[Gene(DragonType.Aether)]
		Aether_Underbelly = 144,
		[Description("Whiskers (Aether)")]
		[Order(307)]
		[Gene(DragonType.Aether)]
		Aether_Whiskers = 154,
		[Description("Wish (Aether)")]
		[Order(308)]
		[Gene(DragonType.Aether)]
		Aether_Wish = 155,
	}

	public static class GeneratedFREnumExtentions
	{
		public static bool IsAncientBreed(this DragonType type)
		{
			if ((int)type == 17)
				return true;
			if ((int)type == 18)
				return true;
			if ((int)type == 19)
				return true;
			if ((int)type == 20)
				return true;
			if ((int)type == 21)
				return true;
			if ((int)type == 22)
				return true;
			if ((int)type == 23)
				return true;
			if ((int)type == 24)
				return true;
			if ((int)type == 25)
				return true;
			return false;
		}

		public static System.Type PrimaryGeneType(this DragonType type)
		{
			if ((int)type == 17)
				return typeof(GaolerBodyGene);
			if ((int)type == 18)
				return typeof(BanescaleBodyGene);
			if ((int)type == 19)
				return typeof(VeilspunBodyGene);
			if ((int)type == 20)
				return typeof(AberrationBodyGene);
			if ((int)type == 21)
				return typeof(UndertideBodyGene);
			if ((int)type == 22)
				return typeof(AetherBodyGene);
			if ((int)type == 23)
				return typeof(SandsurgeBodyGene);
			if ((int)type == 24)
				return typeof(AuraboaBodyGene);
			if ((int)type == 25)
				return typeof(DusthideBodyGene);
			return typeof(BodyGene);
		}

		public static System.Type SecondaryGeneType(this DragonType type)
		{
			if ((int)type == 17)
				return typeof(GaolerWingGene);
			if ((int)type == 18)
				return typeof(BanescaleWingGene);
			if ((int)type == 19)
				return typeof(VeilspunWingGene);
			if ((int)type == 20)
				return typeof(AberrationWingGene);
			if ((int)type == 21)
				return typeof(UndertideWingGene);
			if ((int)type == 22)
				return typeof(AetherWingGene);
			if ((int)type == 23)
				return typeof(SandsurgeWingGene);
			if ((int)type == 24)
				return typeof(AuraboaWingGene);
			if ((int)type == 25)
				return typeof(DusthideWingGene);
			return typeof(WingGene);
		}

		public static System.Type TertiaryGeneType(this DragonType type)
		{
			if ((int)type == 17)
				return typeof(GaolerTertGene);
			if ((int)type == 18)
				return typeof(BanescaleTertGene);
			if ((int)type == 19)
				return typeof(VeilspunTertGene);
			if ((int)type == 20)
				return typeof(AberrationTertGene);
			if ((int)type == 21)
				return typeof(UndertideTertGene);
			if ((int)type == 22)
				return typeof(AetherTertGene);
			if ((int)type == 23)
				return typeof(SandsurgeTertGene);
			if ((int)type == 24)
				return typeof(AuraboaTertGene);
			if ((int)type == 25)
				return typeof(DusthideTertGene);
			return typeof(TertiaryGene);
		}
	}

	public static class GeneratedFRHelpers
	{
		public static DragonType[] GetModernBreeds()
		{
			return new[]
			{
				DragonType.Fae, DragonType.Guardian, DragonType.Mirror, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Tundra, DragonType.Spiral, DragonType.Imperial, DragonType.Snapper, DragonType.Wildclaw, DragonType.Nocturne, DragonType.Coatl, DragonType.Skydancer, DragonType.Bogsneak, DragonType.Obelisk, DragonType.Fathom, 
			};
		}

		public static DragonType[] GetAncientBreeds()
		{
			return new[]
			{
				DragonType.Gaoler, DragonType.Banescale, DragonType.Veilspun, DragonType.Aberration, DragonType.Undertide, DragonType.Aether, DragonType.Sandsurge, DragonType.Auraboa, DragonType.Dusthide, 
			};
		}

		public static Task<string> GenerateDragonImageUrl(DataModels.FlightRisingModels.DragonCache dragon, bool swapSilhouette = false)
		{
			var gender = swapSilhouette ? (dragon.Gender == Gender.Male ? Gender.Female : Gender.Male) : dragon.Gender;
			switch (dragon.DragonType)
			{
				case DragonType.Gaoler:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (GaolerBodyGene)dragon.BodyGene,
						dragon.BodyColor, (GaolerWingGene)dragon.WingGene, dragon.WingColor, (GaolerTertGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
				case DragonType.Banescale:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (BanescaleBodyGene)dragon.BodyGene,
						dragon.BodyColor, (BanescaleWingGene)dragon.WingGene, dragon.WingColor, (BanescaleTertGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
				case DragonType.Veilspun:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (VeilspunBodyGene)dragon.BodyGene,
						dragon.BodyColor, (VeilspunWingGene)dragon.WingGene, dragon.WingColor, (VeilspunTertGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
				case DragonType.Aberration:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (AberrationBodyGene)dragon.BodyGene,
						dragon.BodyColor, (AberrationWingGene)dragon.WingGene, dragon.WingColor, (AberrationTertGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
				case DragonType.Undertide:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (UndertideBodyGene)dragon.BodyGene,
						dragon.BodyColor, (UndertideWingGene)dragon.WingGene, dragon.WingColor, (UndertideTertGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
				case DragonType.Aether:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (AetherBodyGene)dragon.BodyGene,
						dragon.BodyColor, (AetherWingGene)dragon.WingGene, dragon.WingColor, (AetherTertGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
				case DragonType.Sandsurge:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (SandsurgeBodyGene)dragon.BodyGene,
						dragon.BodyColor, (SandsurgeWingGene)dragon.WingGene, dragon.WingColor, (SandsurgeTertGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
				case DragonType.Auraboa:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (AuraboaBodyGene)dragon.BodyGene,
						dragon.BodyColor, (AuraboaWingGene)dragon.WingGene, dragon.WingColor, (AuraboaTertGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
				case DragonType.Dusthide:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (DusthideBodyGene)dragon.BodyGene,
						dragon.BodyColor, (DusthideWingGene)dragon.WingGene, dragon.WingColor, (DusthideTertGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
				default:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (BodyGene)dragon.BodyGene,
						dragon.BodyColor, (WingGene)dragon.WingGene, dragon.WingColor, (TertiaryGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
			}
		}

		public static Task<string> GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, GaolerBodyGene bodygene, Color body, GaolerWingGene winggene, Color wings, GaolerTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static Task<string> GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, BanescaleBodyGene bodygene, Color body, BanescaleWingGene winggene, Color wings, BanescaleTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static Task<string> GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, VeilspunBodyGene bodygene, Color body, VeilspunWingGene winggene, Color wings, VeilspunTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static Task<string> GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, AberrationBodyGene bodygene, Color body, AberrationWingGene winggene, Color wings, AberrationTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static Task<string> GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, UndertideBodyGene bodygene, Color body, UndertideWingGene winggene, Color wings, UndertideTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static Task<string> GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, AetherBodyGene bodygene, Color body, AetherWingGene winggene, Color wings, AetherTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static Task<string> GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, SandsurgeBodyGene bodygene, Color body, SandsurgeWingGene winggene, Color wings, SandsurgeTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static Task<string> GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, AuraboaBodyGene bodygene, Color body, AuraboaWingGene winggene, Color wings, AuraboaTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static Task<string> GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, DusthideBodyGene bodygene, Color body, DusthideWingGene winggene, Color wings, DusthideTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static Task<string> GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, BodyGene bodygene, Color body, WingGene winggene, Color wings, TertiaryGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);

		public static async Task<string> GenerateDragonImageUrl(int breed, int gender, int age, int bodygene, int body, int winggene, int wings, int tertgene, int tert, int element, int eyetype)
		{
			using (var client = new HttpClient())
			{
				var response = await client.PostAsync("https://www1.flightrising.com/scrying/ajax-predict", new FormUrlEncodedContent(new KeyValuePair<string, string>[]
				{
					new("breed", breed.ToString()),
					new("gender", gender.ToString()),
					new("age", age.ToString()),
					new("bodygene", bodygene.ToString()),
					new("body", body.ToString()),
					new("winggene", winggene.ToString()),
					new("wings", wings.ToString()),
					new("tertgene", tertgene.ToString()),
					new("tert", tert.ToString()),
					new("element", element.ToString()),
					new("eyetype", eyetype.ToString()),
				}));

				var str = await response.Content.ReadAsStringAsync();
				var dragonUrl = JsonConvert.DeserializeObject<DragonPredict>(str).DragonUrl;
				return "https://www1.flightrising.com" + dragonUrl;
			}
		}
	}

	public class DragonPredict
	{
		[JsonProperty("ok")]
		public bool Ok { get; set; }
		[JsonProperty("dragon_url")]
		public string DragonUrl { get; set; }
		[JsonProperty("attributes")]
		public Attributes Attributes { get; set; }
	}

	public class Attributes
	{
		[JsonProperty("breed")]
		public string Breed { get; set; }
		[JsonProperty("gender")]
		public string Gender { get; set; }
		[JsonProperty("age")]
		public string Age { get; set; }
		[JsonProperty("bodygene")]
		public string BodyGene { get; set; }
		[JsonProperty("body")]
		public string Body { get; set; }
		[JsonProperty("winggene")]
		public string WingGene { get; set; }
		[JsonProperty("wings")]
		public string Wings { get; set; }
		[JsonProperty("tertgene")]
		public string TertGene { get; set; }
		[JsonProperty("tert")]
		public string Tert { get; set; }
		[JsonProperty("element")]
		public string Element { get; set; }
		[JsonProperty("eyetype")]
		public string Eyetype { get; set; }
	}
}
