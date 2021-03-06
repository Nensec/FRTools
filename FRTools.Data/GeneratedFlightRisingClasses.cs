﻿using System.ComponentModel;

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
		[Description("Falcon (Banescale)")]
		Falcon = 80,
		[Description("Giraffe (Banescale)")]
		Giraffe = 81,
		[Description("Jaguar (Banescale)")]
		Jaguar = 44,
		[Description("Laced (Banescale)")]
		Laced = 48,
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
	}
	
	public enum BanescaleWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Alloy (Banescale)")]
		Alloy = 49,
		[Description("Arrow (Banescale)")]
		Arrow = 54,
		[Description("Butterfly (Banescale)")]
		Butterfly = 51,
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
	
	public enum GaolerWingGene
	{
		[Description("Basic")]
		Basic = 0,
		[Description("Bee (Gaoler)")]
		Bee = 28,
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
	}
	
	public enum VeilspunTertGene
	{
		[Description("Basic")]
		Basic = 0,
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
		[Description("Fans (Gaoler)")]
		Fans = 3,
		[Description("Ghost (Gaoler)")]
		Ghost = 25,
		[Description("Gnarlhorns (Gaoler)")]
		Gnarlhorns = 27,
		[Description("Opal (Gaoler)")]
		Opal = 37,
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
	
	public enum BanescaleTertGene
	{
		[Description("Basic")]
		Basic = 0,
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
