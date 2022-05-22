using System.ComponentModel;

namespace FRTools.Data
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
		[Description("Gaoler")]
		Gaoler = 17,
		[Description("Banescale")]
		Banescale = 18,
		[Description("Veilspun")]
		Veilspun = 19,
		[Description("Aberration")]
		Aberration = 20,
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
	}

	public enum Color
	{
		[Color("#ffffff1a", -1)]
		Unknown = 0,
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
	}
	
	public enum VeilspunBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Arc (Veilspun)")]
		Arc = 70,
		[Description("Bright (Veilspun)")]
		Bright = 69,
		[Description("Clown (Veilspun)")]
		Clown = 76,
		[Description("Fade (Veilspun)")]
		Fade = 60,
		[Description("Giraffe (Veilspun)")]
		Giraffe = 83,
		[Description("Jupiter (Veilspun)")]
		Jupiter = 64,
		[Description("Laced (Veilspun)")]
		Laced = 61,
		[Description("Shell (Veilspun)")]
		Shell = 71,
		[Description("Skink (Veilspun)")]
		Skink = 67,
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
		[Description("Crystal (Aberration)")]
		Crystal = 91,
		[Description("Diamond (Aberration)")]
		Diamond = 93,
		[Description("Fade (Aberration)")]
		Fade = 90,
		[Description("Falcon (Aberration)")]
		Falcon = 92,
		[Description("Giraffe (Aberration)")]
		Giraffe = 94,
		[Description("Ground (Aberration)")]
		Ground = 97,
		[Description("Jaguar (Aberration)")]
		Jaguar = 99,
		[Description("Lionfish (Aberration)")]
		Lionfish = 100,
		[Description("Orb (Aberration)")]
		Orb = 102,
		[Description("Pharaoh (Aberration)")]
		Pharaoh = 101,
		[Description("Ribbon (Aberration)")]
		Ribbon = 105,
		[Description("Savannah (Aberration)")]
		Savannah = 103,
		[Description("Slime (Aberration)")]
		Slime = 106,
		[Description("Speckle (Aberration)")]
		Speckle = 98,
		[Description("Stitched (Aberration)")]
		Stitched = 107,
		[Description("Swirl (Aberration)")]
		Swirl = 104,
		[Description("Tapir (Aberration)")]
		Tapir = 95,
		[Description("Vipera (Aberration)")]
		Vipera = 96,
		[Description("Wasp (Aberration)")]
		Wasp = 108,
	}
	
	public enum GaolerBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bar (Gaoler)")]
		Bar = 34,
		[Description("Clown (Gaoler)")]
		Clown = 77,
		[Description("Crystal (Gaoler)")]
		Crystal = 37,
		[Description("Fade (Gaoler)")]
		Fade = 86,
		[Description("Falcon (Gaoler)")]
		Falcon = 30,
		[Description("Giraffe (Gaoler)")]
		Giraffe = 27,
		[Description("Jaguar (Gaoler)")]
		Jaguar = 33,
		[Description("Laced (Gaoler)")]
		Laced = 73,
		[Description("Mosaic (Gaoler)")]
		Mosaic = 38,
		[Description("Phantom (Gaoler)")]
		Phantom = 39,
		[Description("Piebald (Gaoler)")]
		Piebald = 31,
		[Description("Pinstripe (Gaoler)")]
		Pinstripe = 32,
		[Description("Ripple (Gaoler)")]
		Ripple = 78,
		[Description("Shaggy (Gaoler)")]
		Shaggy = 29,
		[Description("Tapir (Gaoler)")]
		Tapir = 35,
		[Description("Tiger (Gaoler)")]
		Tiger = 36,
		[Description("Wasp (Gaoler)")]
		Wasp = 28,
	}
	
	public enum BanescaleBodyGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Candycane (Banescale)")]
		Candycane = 55,
		[Description("Cherub (Banescale)")]
		Cherub = 43,
		[Description("Chevron (Banescale)")]
		Chevron = 54,
		[Description("Clown (Banescale)")]
		Clown = 75,
		[Description("Fade (Banescale)")]
		Fade = 85,
		[Description("Falcon (Banescale)")]
		Falcon = 80,
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
		[Description("Petals (Banescale)")]
		Petals = 51,
		[Description("Pinstripe (Banescale)")]
		Pinstripe = 45,
		[Description("Poison (Banescale)")]
		Poison = 53,
		[Description("Ragged (Banescale)")]
		Ragged = 56,
		[Description("Ripple (Banescale)")]
		Ripple = 79,
		[Description("Savannah (Banescale)")]
		Savannah = 50,
		[Description("Skink (Banescale)")]
		Skink = 52,
		[Description("Tapir (Banescale)")]
		Tapir = 74,
		[Description("Tiger (Banescale)")]
		Tiger = 46,
	}
	
	public enum AllBodyGene
	{
		[Description("Basic")]
		[Order(0)]
		[Gene(DragonType.Aberration, DragonType.Banescale, DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Gaoler, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Veilspun, DragonType.Wildclaw)]
		Basic = 0,		
		[Description("Iridescent")]
		[Order(1)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Iridescent = 1,		
		[Description("Tiger")]
		[Order(2)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Tiger = 2,		
		[Description("Clown")]
		[Order(3)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Clown = 3,		
		[Description("Speckle")]
		[Order(4)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Speckle = 4,		
		[Description("Ripple")]
		[Order(5)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Ripple = 5,		
		[Description("Bar")]
		[Order(6)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Bar = 6,		
		[Description("Crystal")]
		[Order(7)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Crystal = 7,		
		[Description("Vipera")]
		[Order(8)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Vipera = 8,		
		[Description("Piebald")]
		[Order(9)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Piebald = 9,		
		[Description("Cherub")]
		[Order(10)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Cherub = 10,		
		[Description("Poison")]
		[Order(11)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Poison = 11,		
		[Description("Giraffe")]
		[Order(12)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Giraffe = 12,		
		[Description("Petals")]
		[Order(13)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Petals = 13,		
		[Description("Jupiter")]
		[Order(14)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Jupiter = 14,		
		[Description("Skink")]
		[Order(15)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Skink = 15,		
		[Description("Falcon")]
		[Order(16)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Falcon = 16,		
		[Description("Metallic")]
		[Order(17)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Metallic = 17,		
		[Description("Savannah")]
		[Order(18)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Savannah = 18,		
		[Description("Jaguar")]
		[Order(19)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Jaguar = 19,		
		[Description("Wasp")]
		[Order(20)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Wasp = 20,		
		[Description("Tapir")]
		[Order(21)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Tapir = 21,		
		[Description("Pinstripe")]
		[Order(22)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Pinstripe = 22,		
		[Description("Python")]
		[Order(23)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Python = 23,		
		[Description("Starmap")]
		[Order(24)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Starmap = 24,		
		[Description("Lionfish")]
		[Order(25)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Lionfish = 25,		
		[Description("Laced")]
		[Order(26)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Laced = 26,		
		[Description("Leopard")]
		[Order(27)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Leopard = 40,		
		[Description("Slime")]
		[Order(28)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Slime = 41,		
		[Description("Fade")]
		[Order(29)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Fade = 42,		
		[Description("Swirl")]
		[Order(30)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Swirl = 57,		
		[Description("Mosaic")]
		[Order(31)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Mosaic = 58,		
		[Description("Stitched")]
		[Order(32)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Stitched = 59,		
		[Description("Flaunt")]
		[Order(33)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Flaunt = 82,		
		[Description("Ribbon")]
		[Order(34)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Ribbon = 84,		
		[Description("Pharaoh")]
		[Order(35)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Pharaoh = 87,		
		[Description("Ground")]
		[Order(36)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Ground = 88,		
		[Description("Boulder")]
		[Order(37)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Boulder = 110,		
		[Description("Arc (Veilspun)")]
		[Order(38)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Arc = 70,
		[Description("Bright (Veilspun)")]
		[Order(39)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Bright = 69,
		[Description("Clown (Veilspun)")]
		[Order(40)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Clown = 76,
		[Description("Fade (Veilspun)")]
		[Order(41)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Fade = 60,
		[Description("Giraffe (Veilspun)")]
		[Order(42)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Giraffe = 83,
		[Description("Jupiter (Veilspun)")]
		[Order(43)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Jupiter = 64,
		[Description("Laced (Veilspun)")]
		[Order(44)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Laced = 61,
		[Description("Shell (Veilspun)")]
		[Order(45)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Shell = 71,
		[Description("Skink (Veilspun)")]
		[Order(46)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Skink = 67,
		[Description("Sphinxmoth (Veilspun)")]
		[Order(47)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Sphinxmoth = 72,
		[Description("Starmap (Veilspun)")]
		[Order(48)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Starmap = 65,
		[Description("Stitched (Veilspun)")]
		[Order(49)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Stitched = 66,
		[Description("Tapir (Veilspun)")]
		[Order(50)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Tapir = 62,
		[Description("Vipera (Veilspun)")]
		[Order(51)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Vipera = 63,
		[Description("Wasp (Veilspun)")]
		[Order(52)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Wasp = 68,
		[Description("Bar (Aberration)")]
		[Order(53)]
		[Gene(DragonType.Aberration)]
		Aberration_Bar = 89,
		[Description("Crystal (Aberration)")]
		[Order(54)]
		[Gene(DragonType.Aberration)]
		Aberration_Crystal = 91,
		[Description("Diamond (Aberration)")]
		[Order(55)]
		[Gene(DragonType.Aberration)]
		Aberration_Diamond = 93,
		[Description("Fade (Aberration)")]
		[Order(56)]
		[Gene(DragonType.Aberration)]
		Aberration_Fade = 90,
		[Description("Falcon (Aberration)")]
		[Order(57)]
		[Gene(DragonType.Aberration)]
		Aberration_Falcon = 92,
		[Description("Giraffe (Aberration)")]
		[Order(58)]
		[Gene(DragonType.Aberration)]
		Aberration_Giraffe = 94,
		[Description("Ground (Aberration)")]
		[Order(59)]
		[Gene(DragonType.Aberration)]
		Aberration_Ground = 97,
		[Description("Jaguar (Aberration)")]
		[Order(60)]
		[Gene(DragonType.Aberration)]
		Aberration_Jaguar = 99,
		[Description("Lionfish (Aberration)")]
		[Order(61)]
		[Gene(DragonType.Aberration)]
		Aberration_Lionfish = 100,
		[Description("Orb (Aberration)")]
		[Order(62)]
		[Gene(DragonType.Aberration)]
		Aberration_Orb = 102,
		[Description("Pharaoh (Aberration)")]
		[Order(63)]
		[Gene(DragonType.Aberration)]
		Aberration_Pharaoh = 101,
		[Description("Ribbon (Aberration)")]
		[Order(64)]
		[Gene(DragonType.Aberration)]
		Aberration_Ribbon = 105,
		[Description("Savannah (Aberration)")]
		[Order(65)]
		[Gene(DragonType.Aberration)]
		Aberration_Savannah = 103,
		[Description("Slime (Aberration)")]
		[Order(66)]
		[Gene(DragonType.Aberration)]
		Aberration_Slime = 106,
		[Description("Speckle (Aberration)")]
		[Order(67)]
		[Gene(DragonType.Aberration)]
		Aberration_Speckle = 98,
		[Description("Stitched (Aberration)")]
		[Order(68)]
		[Gene(DragonType.Aberration)]
		Aberration_Stitched = 107,
		[Description("Swirl (Aberration)")]
		[Order(69)]
		[Gene(DragonType.Aberration)]
		Aberration_Swirl = 104,
		[Description("Tapir (Aberration)")]
		[Order(70)]
		[Gene(DragonType.Aberration)]
		Aberration_Tapir = 95,
		[Description("Vipera (Aberration)")]
		[Order(71)]
		[Gene(DragonType.Aberration)]
		Aberration_Vipera = 96,
		[Description("Wasp (Aberration)")]
		[Order(72)]
		[Gene(DragonType.Aberration)]
		Aberration_Wasp = 108,
		[Description("Bar (Gaoler)")]
		[Order(73)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Bar = 34,
		[Description("Clown (Gaoler)")]
		[Order(74)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Clown = 77,
		[Description("Crystal (Gaoler)")]
		[Order(75)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Crystal = 37,
		[Description("Fade (Gaoler)")]
		[Order(76)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Fade = 86,
		[Description("Falcon (Gaoler)")]
		[Order(77)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Falcon = 30,
		[Description("Giraffe (Gaoler)")]
		[Order(78)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Giraffe = 27,
		[Description("Jaguar (Gaoler)")]
		[Order(79)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Jaguar = 33,
		[Description("Laced (Gaoler)")]
		[Order(80)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Laced = 73,
		[Description("Mosaic (Gaoler)")]
		[Order(81)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Mosaic = 38,
		[Description("Phantom (Gaoler)")]
		[Order(82)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Phantom = 39,
		[Description("Piebald (Gaoler)")]
		[Order(83)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Piebald = 31,
		[Description("Pinstripe (Gaoler)")]
		[Order(84)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Pinstripe = 32,
		[Description("Ripple (Gaoler)")]
		[Order(85)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Ripple = 78,
		[Description("Shaggy (Gaoler)")]
		[Order(86)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Shaggy = 29,
		[Description("Tapir (Gaoler)")]
		[Order(87)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Tapir = 35,
		[Description("Tiger (Gaoler)")]
		[Order(88)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Tiger = 36,
		[Description("Wasp (Gaoler)")]
		[Order(89)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Wasp = 28,
		[Description("Candycane (Banescale)")]
		[Order(90)]
		[Gene(DragonType.Banescale)]
		Banescale_Candycane = 55,
		[Description("Cherub (Banescale)")]
		[Order(91)]
		[Gene(DragonType.Banescale)]
		Banescale_Cherub = 43,
		[Description("Chevron (Banescale)")]
		[Order(92)]
		[Gene(DragonType.Banescale)]
		Banescale_Chevron = 54,
		[Description("Clown (Banescale)")]
		[Order(93)]
		[Gene(DragonType.Banescale)]
		Banescale_Clown = 75,
		[Description("Fade (Banescale)")]
		[Order(94)]
		[Gene(DragonType.Banescale)]
		Banescale_Fade = 85,
		[Description("Falcon (Banescale)")]
		[Order(95)]
		[Gene(DragonType.Banescale)]
		Banescale_Falcon = 80,
		[Description("Giraffe (Banescale)")]
		[Order(96)]
		[Gene(DragonType.Banescale)]
		Banescale_Giraffe = 81,
		[Description("Jaguar (Banescale)")]
		[Order(97)]
		[Gene(DragonType.Banescale)]
		Banescale_Jaguar = 44,
		[Description("Laced (Banescale)")]
		[Order(98)]
		[Gene(DragonType.Banescale)]
		Banescale_Laced = 48,
		[Description("Leopard (Banescale)")]
		[Order(99)]
		[Gene(DragonType.Banescale)]
		Banescale_Leopard = 109,
		[Description("Marble (Banescale)")]
		[Order(100)]
		[Gene(DragonType.Banescale)]
		Banescale_Marble = 47,
		[Description("Metallic (Banescale)")]
		[Order(101)]
		[Gene(DragonType.Banescale)]
		Banescale_Metallic = 49,
		[Description("Petals (Banescale)")]
		[Order(102)]
		[Gene(DragonType.Banescale)]
		Banescale_Petals = 51,
		[Description("Pinstripe (Banescale)")]
		[Order(103)]
		[Gene(DragonType.Banescale)]
		Banescale_Pinstripe = 45,
		[Description("Poison (Banescale)")]
		[Order(104)]
		[Gene(DragonType.Banescale)]
		Banescale_Poison = 53,
		[Description("Ragged (Banescale)")]
		[Order(105)]
		[Gene(DragonType.Banescale)]
		Banescale_Ragged = 56,
		[Description("Ripple (Banescale)")]
		[Order(106)]
		[Gene(DragonType.Banescale)]
		Banescale_Ripple = 79,
		[Description("Savannah (Banescale)")]
		[Order(107)]
		[Gene(DragonType.Banescale)]
		Banescale_Savannah = 50,
		[Description("Skink (Banescale)")]
		[Order(108)]
		[Gene(DragonType.Banescale)]
		Banescale_Skink = 52,
		[Description("Tapir (Banescale)")]
		[Order(109)]
		[Gene(DragonType.Banescale)]
		Banescale_Tapir = 74,
		[Description("Tiger (Banescale)")]
		[Order(110)]
		[Gene(DragonType.Banescale)]
		Banescale_Tiger = 46,
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
	}
	
	public enum BanescaleWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Alloy (Banescale)")]
		Alloy = 49,
		[Description("Arrow (Banescale)")]
		Arrow = 54,
		[Description("Blend (Banescale)")]
		Blend = 85,
		[Description("Butterfly (Banescale)")]
		Butterfly = 51,
		[Description("Clouded (Banescale)")]
		Clouded = 109,
		[Description("Current (Banescale)")]
		Current = 79,
		[Description("Edged (Banescale)")]
		Edged = 48,
		[Description("Eye Spots (Banescale)")]
		EyeSpots = 75,
		[Description("Hex (Banescale)")]
		Hex = 81,
		[Description("Mottle (Banescale)")]
		Mottle = 47,
		[Description("Peregrine (Banescale)")]
		Peregrine = 80,
		[Description("Rosette (Banescale)")]
		Rosette = 44,
		[Description("Safari (Banescale)")]
		Safari = 50,
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
	}
	
	public enum AberrationWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bee (Aberration)")]
		Bee = 108,
		[Description("Blend (Aberration)")]
		Blend = 91,
		[Description("Daub (Aberration)")]
		Daub = 89,
		[Description("Eel (Aberration)")]
		Eel = 105,
		[Description("Facet (Aberration)")]
		Facet = 90,
		[Description("Fissure (Aberration)")]
		Fissure = 97,
		[Description("Freckle (Aberration)")]
		Freckle = 98,
		[Description("Hex (Aberration)")]
		Hex = 94,
		[Description("Hypnotic (Aberration)")]
		Hypnotic = 96,
		[Description("Marbled (Aberration)")]
		Marbled = 103,
		[Description("Noxtide (Aberration)")]
		Noxtide = 100,
		[Description("Patchwork (Aberration)")]
		Patchwork = 107,
		[Description("Peregrine (Aberration)")]
		Peregrine = 92,
		[Description("Rosette (Aberration)")]
		Rosette = 99,
		[Description("Safari (Aberration)")]
		Safari = 104,
		[Description("Sarcophagus (Aberration)")]
		Sarcophagus = 101,
		[Description("Sludge (Aberration)")]
		Sludge = 106,
		[Description("Spade (Aberration)")]
		Spade = 93,
		[Description("Striation (Aberration)")]
		Striation = 95,
		[Description("Weaver (Aberration)")]
		Weaver = 102,
	}
	
	public enum GaolerWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bee (Gaoler)")]
		Bee = 28,
		[Description("Blend (Gaoler)")]
		Blend = 86,
		[Description("Breakup (Gaoler)")]
		Breakup = 38,
		[Description("Current (Gaoler)")]
		Current = 78,
		[Description("Daub (Gaoler)")]
		Daub = 34,
		[Description("Edged (Gaoler)")]
		Edged = 73,
		[Description("Eye Spots (Gaoler)")]
		EyeSpots = 77,
		[Description("Facet (Gaoler)")]
		Facet = 37,
		[Description("Hex (Gaoler)")]
		Hex = 27,
		[Description("Paint (Gaoler)")]
		Paint = 31,
		[Description("Peregrine (Gaoler)")]
		Peregrine = 30,
		[Description("Rosette (Gaoler)")]
		Rosette = 33,
		[Description("Spirit (Gaoler)")]
		Spirit = 39,
		[Description("Streak (Gaoler)")]
		Streak = 29,
		[Description("Striation (Gaoler)")]
		Striation = 35,
		[Description("Stripes (Gaoler)")]
		Stripes = 36,
		[Description("Trail (Gaoler)")]
		Trail = 32,
	}
	
	public enum VeilspunWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bee (Veilspun)")]
		Bee = 60,
		[Description("Blend (Veilspun)")]
		Blend = 61,
		[Description("Constellation (Veilspun)")]
		Constellation = 66,
		[Description("Edged (Veilspun)")]
		Edged = 62,
		[Description("Eye Spots (Veilspun)")]
		EyeSpots = 76,
		[Description("Hawkmoth (Veilspun)")]
		Hawkmoth = 72,
		[Description("Hex (Veilspun)")]
		Hex = 83,
		[Description("Hypnotic (Veilspun)")]
		Hypnotic = 64,
		[Description("Loop (Veilspun)")]
		Loop = 70,
		[Description("Patchwork (Veilspun)")]
		Patchwork = 67,
		[Description("Saturn (Veilspun)")]
		Saturn = 65,
		[Description("Spinner (Veilspun)")]
		Spinner = 68,
		[Description("Striation (Veilspun)")]
		Striation = 63,
		[Description("Vivid (Veilspun)")]
		Vivid = 69,
		[Description("Web (Veilspun)")]
		Web = 71,
	}
	
	public enum AllWingGene
	{
		[Description("Basic")]
		[Order(0)]
		[Gene(DragonType.Aberration, DragonType.Banescale, DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Gaoler, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Veilspun, DragonType.Wildclaw)]
		Basic = 0,
		[Description("Shimmer")]
		[Order(1)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Shimmer = 1,
		[Description("Stripes")]
		[Order(2)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Stripes = 2,
		[Description("Eye Spots")]
		[Order(3)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		EyeSpots = 3,
		[Description("Freckle")]
		[Order(4)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Freckle = 4,
		[Description("Seraph")]
		[Order(5)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Seraph = 5,
		[Description("Current")]
		[Order(6)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Current = 6,
		[Description("Daub")]
		[Order(7)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Daub = 7,
		[Description("Facet")]
		[Order(8)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Facet = 8,
		[Description("Hypnotic")]
		[Order(9)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Hypnotic = 9,
		[Description("Paint")]
		[Order(10)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Paint = 10,
		[Description("Peregrine")]
		[Order(11)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Peregrine = 11,
		[Description("Toxin")]
		[Order(12)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Toxin = 12,
		[Description("Butterfly")]
		[Order(13)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Butterfly = 13,
		[Description("Hex")]
		[Order(14)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Hex = 14,
		[Description("Saturn")]
		[Order(15)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Saturn = 15,
		[Description("Spinner")]
		[Order(16)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Spinner = 16,
		[Description("Alloy")]
		[Order(17)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Alloy = 17,
		[Description("Safari")]
		[Order(18)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Safari = 18,
		[Description("Rosette")]
		[Order(19)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Rosette = 19,
		[Description("Bee")]
		[Order(20)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Bee = 20,
		[Description("Striation")]
		[Order(21)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Striation = 21,
		[Description("Trail")]
		[Order(22)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Trail = 22,
		[Description("Morph")]
		[Order(23)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Morph = 23,
		[Description("Noxtide")]
		[Order(24)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Noxtide = 24,
		[Description("Constellation")]
		[Order(25)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Constellation = 25,
		[Description("Edged")]
		[Order(26)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Edged = 26,
		[Description("Clouded")]
		[Order(27)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Clouded = 40,
		[Description("Sludge")]
		[Order(28)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Sludge = 41,
		[Description("Blend")]
		[Order(29)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Blend = 42,
		[Description("Marbled")]
		[Order(30)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Marbled = 57,
		[Description("Breakup")]
		[Order(31)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Breakup = 58,
		[Description("Patchwork")]
		[Order(32)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Patchwork = 59,
		[Description("Flair")]
		[Order(33)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Flair = 82,
		[Description("Eel")]
		[Order(34)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Eel = 84,
		[Description("Sarcophagus")]
		[Order(35)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Sarcophagus = 87,
		[Description("Fissure")]
		[Order(36)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Fissure = 88,
		[Description("Myrid")]
		[Order(37)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Myrid = 110,
		[Description("Alloy (Banescale)")]
		[Order(38)]
		[Gene(DragonType.Banescale)]
		Banescale_Alloy = 49,
		[Description("Arrow (Banescale)")]
		[Order(39)]
		[Gene(DragonType.Banescale)]
		Banescale_Arrow = 54,
		[Description("Blend (Banescale)")]
		[Order(40)]
		[Gene(DragonType.Banescale)]
		Banescale_Blend = 85,
		[Description("Butterfly (Banescale)")]
		[Order(41)]
		[Gene(DragonType.Banescale)]
		Banescale_Butterfly = 51,
		[Description("Clouded (Banescale)")]
		[Order(42)]
		[Gene(DragonType.Banescale)]
		Banescale_Clouded = 109,
		[Description("Current (Banescale)")]
		[Order(43)]
		[Gene(DragonType.Banescale)]
		Banescale_Current = 79,
		[Description("Edged (Banescale)")]
		[Order(44)]
		[Gene(DragonType.Banescale)]
		Banescale_Edged = 48,
		[Description("Eye Spots (Banescale)")]
		[Order(45)]
		[Gene(DragonType.Banescale)]
		Banescale_EyeSpots = 75,
		[Description("Hex (Banescale)")]
		[Order(46)]
		[Gene(DragonType.Banescale)]
		Banescale_Hex = 81,
		[Description("Mottle (Banescale)")]
		[Order(47)]
		[Gene(DragonType.Banescale)]
		Banescale_Mottle = 47,
		[Description("Peregrine (Banescale)")]
		[Order(48)]
		[Gene(DragonType.Banescale)]
		Banescale_Peregrine = 80,
		[Description("Rosette (Banescale)")]
		[Order(49)]
		[Gene(DragonType.Banescale)]
		Banescale_Rosette = 44,
		[Description("Safari (Banescale)")]
		[Order(50)]
		[Gene(DragonType.Banescale)]
		Banescale_Safari = 50,
		[Description("Seraph (Banescale)")]
		[Order(51)]
		[Gene(DragonType.Banescale)]
		Banescale_Seraph = 43,
		[Description("Spinner (Banescale)")]
		[Order(52)]
		[Gene(DragonType.Banescale)]
		Banescale_Spinner = 52,
		[Description("Striation (Banescale)")]
		[Order(53)]
		[Gene(DragonType.Banescale)]
		Banescale_Striation = 74,
		[Description("Stripes (Banescale)")]
		[Order(54)]
		[Gene(DragonType.Banescale)]
		Banescale_Stripes = 46,
		[Description("Sugarplum (Banescale)")]
		[Order(55)]
		[Gene(DragonType.Banescale)]
		Banescale_Sugarplum = 55,
		[Description("Tear (Banescale)")]
		[Order(56)]
		[Gene(DragonType.Banescale)]
		Banescale_Tear = 56,
		[Description("Toxin (Banescale)")]
		[Order(57)]
		[Gene(DragonType.Banescale)]
		Banescale_Toxin = 53,
		[Description("Trail (Banescale)")]
		[Order(58)]
		[Gene(DragonType.Banescale)]
		Banescale_Trail = 45,
		[Description("Bee (Aberration)")]
		[Order(59)]
		[Gene(DragonType.Aberration)]
		Aberration_Bee = 108,
		[Description("Blend (Aberration)")]
		[Order(60)]
		[Gene(DragonType.Aberration)]
		Aberration_Blend = 91,
		[Description("Daub (Aberration)")]
		[Order(61)]
		[Gene(DragonType.Aberration)]
		Aberration_Daub = 89,
		[Description("Eel (Aberration)")]
		[Order(62)]
		[Gene(DragonType.Aberration)]
		Aberration_Eel = 105,
		[Description("Facet (Aberration)")]
		[Order(63)]
		[Gene(DragonType.Aberration)]
		Aberration_Facet = 90,
		[Description("Fissure (Aberration)")]
		[Order(64)]
		[Gene(DragonType.Aberration)]
		Aberration_Fissure = 97,
		[Description("Freckle (Aberration)")]
		[Order(65)]
		[Gene(DragonType.Aberration)]
		Aberration_Freckle = 98,
		[Description("Hex (Aberration)")]
		[Order(66)]
		[Gene(DragonType.Aberration)]
		Aberration_Hex = 94,
		[Description("Hypnotic (Aberration)")]
		[Order(67)]
		[Gene(DragonType.Aberration)]
		Aberration_Hypnotic = 96,
		[Description("Marbled (Aberration)")]
		[Order(68)]
		[Gene(DragonType.Aberration)]
		Aberration_Marbled = 103,
		[Description("Noxtide (Aberration)")]
		[Order(69)]
		[Gene(DragonType.Aberration)]
		Aberration_Noxtide = 100,
		[Description("Patchwork (Aberration)")]
		[Order(70)]
		[Gene(DragonType.Aberration)]
		Aberration_Patchwork = 107,
		[Description("Peregrine (Aberration)")]
		[Order(71)]
		[Gene(DragonType.Aberration)]
		Aberration_Peregrine = 92,
		[Description("Rosette (Aberration)")]
		[Order(72)]
		[Gene(DragonType.Aberration)]
		Aberration_Rosette = 99,
		[Description("Safari (Aberration)")]
		[Order(73)]
		[Gene(DragonType.Aberration)]
		Aberration_Safari = 104,
		[Description("Sarcophagus (Aberration)")]
		[Order(74)]
		[Gene(DragonType.Aberration)]
		Aberration_Sarcophagus = 101,
		[Description("Sludge (Aberration)")]
		[Order(75)]
		[Gene(DragonType.Aberration)]
		Aberration_Sludge = 106,
		[Description("Spade (Aberration)")]
		[Order(76)]
		[Gene(DragonType.Aberration)]
		Aberration_Spade = 93,
		[Description("Striation (Aberration)")]
		[Order(77)]
		[Gene(DragonType.Aberration)]
		Aberration_Striation = 95,
		[Description("Weaver (Aberration)")]
		[Order(78)]
		[Gene(DragonType.Aberration)]
		Aberration_Weaver = 102,
		[Description("Bee (Gaoler)")]
		[Order(79)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Bee = 28,
		[Description("Blend (Gaoler)")]
		[Order(80)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Blend = 86,
		[Description("Breakup (Gaoler)")]
		[Order(81)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Breakup = 38,
		[Description("Current (Gaoler)")]
		[Order(82)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Current = 78,
		[Description("Daub (Gaoler)")]
		[Order(83)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Daub = 34,
		[Description("Edged (Gaoler)")]
		[Order(84)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Edged = 73,
		[Description("Eye Spots (Gaoler)")]
		[Order(85)]
		[Gene(DragonType.Gaoler)]
		Gaoler_EyeSpots = 77,
		[Description("Facet (Gaoler)")]
		[Order(86)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Facet = 37,
		[Description("Hex (Gaoler)")]
		[Order(87)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Hex = 27,
		[Description("Paint (Gaoler)")]
		[Order(88)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Paint = 31,
		[Description("Peregrine (Gaoler)")]
		[Order(89)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Peregrine = 30,
		[Description("Rosette (Gaoler)")]
		[Order(90)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Rosette = 33,
		[Description("Spirit (Gaoler)")]
		[Order(91)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Spirit = 39,
		[Description("Streak (Gaoler)")]
		[Order(92)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Streak = 29,
		[Description("Striation (Gaoler)")]
		[Order(93)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Striation = 35,
		[Description("Stripes (Gaoler)")]
		[Order(94)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Stripes = 36,
		[Description("Trail (Gaoler)")]
		[Order(95)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Trail = 32,
		[Description("Bee (Veilspun)")]
		[Order(96)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Bee = 60,
		[Description("Blend (Veilspun)")]
		[Order(97)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Blend = 61,
		[Description("Constellation (Veilspun)")]
		[Order(98)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Constellation = 66,
		[Description("Edged (Veilspun)")]
		[Order(99)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Edged = 62,
		[Description("Eye Spots (Veilspun)")]
		[Order(100)]
		[Gene(DragonType.Veilspun)]
		Veilspun_EyeSpots = 76,
		[Description("Hawkmoth (Veilspun)")]
		[Order(101)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Hawkmoth = 72,
		[Description("Hex (Veilspun)")]
		[Order(102)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Hex = 83,
		[Description("Hypnotic (Veilspun)")]
		[Order(103)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Hypnotic = 64,
		[Description("Loop (Veilspun)")]
		[Order(104)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Loop = 70,
		[Description("Patchwork (Veilspun)")]
		[Order(105)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Patchwork = 67,
		[Description("Saturn (Veilspun)")]
		[Order(106)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Saturn = 65,
		[Description("Spinner (Veilspun)")]
		[Order(107)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Spinner = 68,
		[Description("Striation (Veilspun)")]
		[Order(108)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Striation = 63,
		[Description("Vivid (Veilspun)")]
		[Order(109)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Vivid = 69,
		[Description("Web (Veilspun)")]
		[Order(110)]
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
		[Description("Capsule (Veilspun)")]
		Capsule = 56,
		[Description("Crackle (Veilspun)")]
		Crackle = 58,
		[Description("Diaphanous (Veilspun)")]
		Diaphanous = 66,
		[Description("Firefly (Veilspun)")]
		Firefly = 61,
		[Description("Flecks (Veilspun)")]
		Flecks = 64,
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
		[Description("Underbelly (Veilspun)")]
		Underbelly = 70,
	}
	
	public enum GaolerTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Blossom (Gaoler)")]
		Blossom = 36,
		[Description("Braids (Gaoler)")]
		Braids = 55,
		[Description("Capsule (Gaoler)")]
		Capsule = 75,
		[Description("Fans (Gaoler)")]
		Fans = 3,
		[Description("Ghost (Gaoler)")]
		Ghost = 25,
		[Description("Gnarlhorns (Gaoler)")]
		Gnarlhorns = 27,
		[Description("Opal (Gaoler)")]
		Opal = 37,
		[Description("Pinions (Gaoler)")]
		Pinions = 77,
		[Description("Ringlets (Gaoler)")]
		Ringlets = 30,
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
	
	public enum AberrationTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Capsule (Aberration)")]
		Capsule = 83,
		[Description("Fangs (Aberration)")]
		Fangs = 84,
		[Description("Firefly (Aberration)")]
		Firefly = 85,
		[Description("Frills (Aberration)")]
		Frills = 86,
		[Description("Ghost (Aberration)")]
		Ghost = 88,
		[Description("Glowtail (Aberration)")]
		Glowtail = 89,
		[Description("Jewels (Aberration)")]
		Jewels = 87,
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
		[Description("Scales (Aberration)")]
		Scales = 92,
		[Description("Sparkle (Aberration)")]
		Sparkle = 96,
		[Description("Thylacine (Aberration)")]
		Thylacine = 93,
		[Description("Veined (Aberration)")]
		Veined = 91,
	}
	
	public enum BanescaleTertGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Capsule (Banescale)")]
		Capsule = 74,
		[Description("Contour (Banescale)")]
		Contour = 46,
		[Description("Crackle (Banescale)")]
		Crackle = 50,
		[Description("Fans (Banescale)")]
		Fans = 41,
		[Description("Filigree (Banescale)")]
		Filigree = 43,
		[Description("Ghost (Banescale)")]
		Ghost = 47,
		[Description("Gliders (Banescale)")]
		Gliders = 76,
		[Description("Lace (Banescale)")]
		Lace = 44,
		[Description("Plumage (Banescale)")]
		Plumage = 51,
		[Description("Porcupine (Banescale)")]
		Porcupine = 49,
		[Description("Ringlets (Banescale)")]
		Ringlets = 40,
		[Description("Skeletal (Banescale)")]
		Skeletal = 45,
		[Description("Sparkle (Banescale)")]
		Sparkle = 98,
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
	
	public enum AllTertiaryGene
	{
		[Description("Basic")]
		[Order(0)]
		[Gene(DragonType.Aberration, DragonType.Banescale, DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Gaoler, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Veilspun, DragonType.Wildclaw)]
		Basic = 0,
		[Description("Circuit")]
		[Order(1)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Circuit = 1,
		[Description("Gembond")]
		[Order(2)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Gembond = 4,
		[Description("Underbelly")]
		[Order(3)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Underbelly = 5,
		[Description("Crackle")]
		[Order(4)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Crackle = 6,
		[Description("Smoke")]
		[Order(5)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Smoke = 7,
		[Description("Spines")]
		[Order(6)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Spines = 8,
		[Description("Okapi")]
		[Order(7)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Okapi = 9,
		[Description("Glimmer")]
		[Order(8)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Glimmer = 10,
		[Description("Thylacine")]
		[Order(9)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Thylacine = 11,
		[Description("Stained")]
		[Order(10)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Stained = 12,
		[Description("Contour")]
		[Order(11)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Contour = 13,
		[Description("Runes")]
		[Order(12)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Runes = 14,
		[Description("Scales")]
		[Order(13)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Scales = 15,
		[Description("Lace")]
		[Order(14)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Lace = 16,
		[Description("Opal")]
		[Order(15)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Opal = 17,
		[Description("Capsule")]
		[Order(16)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Capsule = 18,
		[Description("Smirch")]
		[Order(17)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Smirch = 19,
		[Description("Ghost")]
		[Order(18)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Ghost = 20,
		[Description("Filigree")]
		[Order(19)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Filigree = 21,
		[Description("Firefly")]
		[Order(20)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Firefly = 22,
		[Description("Ringlets")]
		[Order(21)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Ringlets = 23,
		[Description("Peacock")]
		[Order(22)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Peacock = 24,
		[Description("Veined")]
		[Order(23)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Veined = 38,
		[Description("Keel")]
		[Order(24)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Keel = 53,
		[Description("Glowtail")]
		[Order(25)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Glowtail = 54,
		[Description("Koi")]
		[Order(26)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Koi = 73,
		[Description("Sparkle")]
		[Order(27)]
		[Gene(DragonType.Bogsneak, DragonType.Coatl, DragonType.Fae, DragonType.Guardian, DragonType.Imperial, DragonType.Mirror, DragonType.Nocturne, DragonType.Obelisk, DragonType.Pearlcatcher, DragonType.Ridgeback, DragonType.Skydancer, DragonType.Snapper, DragonType.Spiral, DragonType.Tundra, DragonType.Wildclaw)]
		Sparkle = 97,
		[Description("Angler (Veilspun)")]
		[Order(28)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Angler = 78,
		[Description("Beetle (Veilspun)")]
		[Order(29)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Beetle = 65,
		[Description("Branches (Veilspun)")]
		[Order(30)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Branches = 63,
		[Description("Capsule (Veilspun)")]
		[Order(31)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Capsule = 56,
		[Description("Crackle (Veilspun)")]
		[Order(32)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Crackle = 58,
		[Description("Diaphanous (Veilspun)")]
		[Order(33)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Diaphanous = 66,
		[Description("Firefly (Veilspun)")]
		[Order(34)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Firefly = 61,
		[Description("Flecks (Veilspun)")]
		[Order(35)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Flecks = 64,
		[Description("Mop (Veilspun)")]
		[Order(36)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Mop = 67,
		[Description("Okapi (Veilspun)")]
		[Order(37)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Okapi = 59,
		[Description("Opal (Veilspun)")]
		[Order(38)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Opal = 62,
		[Description("Peacock (Veilspun)")]
		[Order(39)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Peacock = 60,
		[Description("Runes (Veilspun)")]
		[Order(40)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Runes = 57,
		[Description("Sparkle (Veilspun)")]
		[Order(41)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Sparkle = 100,
		[Description("Stained (Veilspun)")]
		[Order(42)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Stained = 72,
		[Description("Thorns (Veilspun)")]
		[Order(43)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Thorns = 68,
		[Description("Underbelly (Veilspun)")]
		[Order(44)]
		[Gene(DragonType.Veilspun)]
		Veilspun_Underbelly = 70,
		[Description("Blossom (Gaoler)")]
		[Order(45)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Blossom = 36,
		[Description("Braids (Gaoler)")]
		[Order(46)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Braids = 55,
		[Description("Capsule (Gaoler)")]
		[Order(47)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Capsule = 75,
		[Description("Fans (Gaoler)")]
		[Order(48)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Fans = 3,
		[Description("Ghost (Gaoler)")]
		[Order(49)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Ghost = 25,
		[Description("Gnarlhorns (Gaoler)")]
		[Order(50)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Gnarlhorns = 27,
		[Description("Opal (Gaoler)")]
		[Order(51)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Opal = 37,
		[Description("Pinions (Gaoler)")]
		[Order(52)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Pinions = 77,
		[Description("Ringlets (Gaoler)")]
		[Order(53)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Ringlets = 30,
		[Description("Runes (Gaoler)")]
		[Order(54)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Runes = 32,
		[Description("Scorpion (Gaoler)")]
		[Order(55)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Scorpion = 33,
		[Description("Shardflank (Gaoler)")]
		[Order(56)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Shardflank = 26,
		[Description("Smoke (Gaoler)")]
		[Order(57)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Smoke = 28,
		[Description("Sparkle (Gaoler)")]
		[Order(58)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Sparkle = 99,
		[Description("Stained (Gaoler)")]
		[Order(59)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Stained = 71,
		[Description("Thylacine (Gaoler)")]
		[Order(60)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Thylacine = 29,
		[Description("Underbelly (Gaoler)")]
		[Order(61)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Underbelly = 31,
		[Description("Veined (Gaoler)")]
		[Order(62)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Veined = 2,
		[Description("Weathered (Gaoler)")]
		[Order(63)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Weathered = 35,
		[Description("Wintercoat (Gaoler)")]
		[Order(64)]
		[Gene(DragonType.Gaoler)]
		Gaoler_Wintercoat = 34,
		[Description("Capsule (Aberration)")]
		[Order(65)]
		[Gene(DragonType.Aberration)]
		Aberration_Capsule = 83,
		[Description("Fangs (Aberration)")]
		[Order(66)]
		[Gene(DragonType.Aberration)]
		Aberration_Fangs = 84,
		[Description("Firefly (Aberration)")]
		[Order(67)]
		[Gene(DragonType.Aberration)]
		Aberration_Firefly = 85,
		[Description("Frills (Aberration)")]
		[Order(68)]
		[Gene(DragonType.Aberration)]
		Aberration_Frills = 86,
		[Description("Ghost (Aberration)")]
		[Order(69)]
		[Gene(DragonType.Aberration)]
		Aberration_Ghost = 88,
		[Description("Glowtail (Aberration)")]
		[Order(70)]
		[Gene(DragonType.Aberration)]
		Aberration_Glowtail = 89,
		[Description("Jewels (Aberration)")]
		[Order(71)]
		[Gene(DragonType.Aberration)]
		Aberration_Jewels = 87,
		[Description("Kumo (Aberration)")]
		[Order(72)]
		[Gene(DragonType.Aberration)]
		Aberration_Kumo = 80,
		[Description("Mucous (Aberration)")]
		[Order(73)]
		[Gene(DragonType.Aberration)]
		Aberration_Mucous = 81,
		[Description("Peacock (Aberration)")]
		[Order(74)]
		[Gene(DragonType.Aberration)]
		Aberration_Peacock = 90,
		[Description("Polkadot (Aberration)")]
		[Order(75)]
		[Gene(DragonType.Aberration)]
		Aberration_Polkadot = 79,
		[Description("Polypore (Aberration)")]
		[Order(76)]
		[Gene(DragonType.Aberration)]
		Aberration_Polypore = 82,
		[Description("Scales (Aberration)")]
		[Order(77)]
		[Gene(DragonType.Aberration)]
		Aberration_Scales = 92,
		[Description("Sparkle (Aberration)")]
		[Order(78)]
		[Gene(DragonType.Aberration)]
		Aberration_Sparkle = 96,
		[Description("Thylacine (Aberration)")]
		[Order(79)]
		[Gene(DragonType.Aberration)]
		Aberration_Thylacine = 93,
		[Description("Veined (Aberration)")]
		[Order(80)]
		[Gene(DragonType.Aberration)]
		Aberration_Veined = 91,
		[Description("Capsule (Banescale)")]
		[Order(81)]
		[Gene(DragonType.Banescale)]
		Banescale_Capsule = 74,
		[Description("Contour (Banescale)")]
		[Order(82)]
		[Gene(DragonType.Banescale)]
		Banescale_Contour = 46,
		[Description("Crackle (Banescale)")]
		[Order(83)]
		[Gene(DragonType.Banescale)]
		Banescale_Crackle = 50,
		[Description("Fans (Banescale)")]
		[Order(84)]
		[Gene(DragonType.Banescale)]
		Banescale_Fans = 41,
		[Description("Filigree (Banescale)")]
		[Order(85)]
		[Gene(DragonType.Banescale)]
		Banescale_Filigree = 43,
		[Description("Ghost (Banescale)")]
		[Order(86)]
		[Gene(DragonType.Banescale)]
		Banescale_Ghost = 47,
		[Description("Gliders (Banescale)")]
		[Order(87)]
		[Gene(DragonType.Banescale)]
		Banescale_Gliders = 76,
		[Description("Lace (Banescale)")]
		[Order(88)]
		[Gene(DragonType.Banescale)]
		Banescale_Lace = 44,
		[Description("Plumage (Banescale)")]
		[Order(89)]
		[Gene(DragonType.Banescale)]
		Banescale_Plumage = 51,
		[Description("Porcupine (Banescale)")]
		[Order(90)]
		[Gene(DragonType.Banescale)]
		Banescale_Porcupine = 49,
		[Description("Ringlets (Banescale)")]
		[Order(91)]
		[Gene(DragonType.Banescale)]
		Banescale_Ringlets = 40,
		[Description("Skeletal (Banescale)")]
		[Order(92)]
		[Gene(DragonType.Banescale)]
		Banescale_Skeletal = 45,
		[Description("Sparkle (Banescale)")]
		[Order(93)]
		[Gene(DragonType.Banescale)]
		Banescale_Sparkle = 98,
		[Description("Squiggle (Banescale)")]
		[Order(94)]
		[Gene(DragonType.Banescale)]
		Banescale_Squiggle = 42,
		[Description("Stained (Banescale)")]
		[Order(95)]
		[Gene(DragonType.Banescale)]
		Banescale_Stained = 69,
		[Description("Trimmings (Banescale)")]
		[Order(96)]
		[Gene(DragonType.Banescale)]
		Banescale_Trimmings = 39,
		[Description("Underbelly (Banescale)")]
		[Order(97)]
		[Gene(DragonType.Banescale)]
		Banescale_Underbelly = 52,
		[Description("Wraith (Banescale)")]
		[Order(98)]
		[Gene(DragonType.Banescale)]
		Banescale_Wraith = 48,
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
			return typeof(TertiaryGene);
		}
	}

	public static class GeneratedFRHelpers
	{
		public static DragonType[] GetModernBreeds()
		{
			return new[]
			{
				DragonType.Fae,
				DragonType.Guardian,
				DragonType.Mirror,
				DragonType.Pearlcatcher,
				DragonType.Ridgeback,
				DragonType.Tundra,
				DragonType.Spiral,
				DragonType.Imperial,
				DragonType.Snapper,
				DragonType.Wildclaw,
				DragonType.Nocturne,
				DragonType.Coatl,
				DragonType.Skydancer,
				DragonType.Bogsneak,
				DragonType.Obelisk,
				
			};
		}

		public static DragonType[] GetAncientBreeds()
		{
			return new[]
			{
				DragonType.Gaoler,
				DragonType.Banescale,
				DragonType.Veilspun,
				DragonType.Aberration,
				
			};
		}

		public static string GenerateDragonImageUrl(DataModels.FlightRisingModels.DragonCache dragon, bool swapSilhouette = false)
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
				default:
					return GenerateDragonImageUrl(dragon.DragonType, gender, dragon.Age, (BodyGene)dragon.BodyGene,
						dragon.BodyColor, (WingGene)dragon.WingGene, dragon.WingColor, (TertiaryGene)dragon.TertiaryGene,
						dragon.TertiaryColor, dragon.Element, dragon.EyeType);
			}
		}

		public static string GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, GaolerBodyGene bodygene, Color body, GaolerWingGene winggene, Color wings, GaolerTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static string GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, BanescaleBodyGene bodygene, Color body, BanescaleWingGene winggene, Color wings, BanescaleTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static string GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, VeilspunBodyGene bodygene, Color body, VeilspunWingGene winggene, Color wings, VeilspunTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static string GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, AberrationBodyGene bodygene, Color body, AberrationWingGene winggene, Color wings, AberrationTertGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);
		public static string GenerateDragonImageUrl(DragonType breed, Gender gender, Age age, BodyGene bodygene, Color body, WingGene winggene, Color wings, TertiaryGene tertgene, Color tert, Element element, EyeType eyetype)
			=> GenerateDragonImageUrl((int)breed, (int)gender, (int)age, (int)bodygene, (int)body, (int)winggene, (int)wings, (int)tertgene, (int)tert, (int)element, (int)eyetype);

		public static string GenerateDragonImageUrl(int breed, int gender, int age, int bodygene, int body, int winggene, int wings, int tertgene, int tert, int element, int eyetype)
		{
			using (var client = new System.Net.WebClient())
			{
				var result = client.UploadValues("https://www1.flightrising.com/scrying/ajax-predict", new System.Collections.Specialized.NameValueCollection
				{
					{ "breed", breed.ToString() },
					{ "gender", gender.ToString() },
					{ "age", age.ToString() },
					{ "bodygene", bodygene.ToString() },
					{ "body", body.ToString() },
					{ "winggene", winggene.ToString() },
					{ "wings", wings.ToString() },
					{ "tertgene", tertgene.ToString() },
					{ "tert", tert.ToString() },
					{ "element", element.ToString() },
					{ "eyetype", eyetype.ToString() },
				});
				var str = System.Text.Encoding.UTF8.GetString(result);
				var dragonUrl = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(str).dragon_url;
				return "https://www1.flightrising.com" + dragonUrl;
			}
		}
	}
}
