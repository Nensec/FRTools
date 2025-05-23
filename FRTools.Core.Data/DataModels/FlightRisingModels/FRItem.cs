﻿namespace FRTools.Core.Data.DataModels.FlightRisingModels
{
    public class FRItem
    {
        public int Id { get; set; }

        public int FRId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FRItemCategory ItemCategory { get; set; }
        public string IconUrl { get; set; }
        public int? Rarity { get; set; }
        public string AssetUrl { get; set; }
        public int? TreasureValue { get; set; }
        public string ItemType { get; set; }
        public FRFoodType? FoodType { get; set; }
        public int? FoodValue { get; set; }
        public int? RequiredLevel { get; set; }
        public virtual FRUser Creator { get; set; }

        public virtual ICollection<FRItemFlashSale> FlashSales { get; set; } = new HashSet<FRItemFlashSale>();
        public virtual ICollection<FRItem> BundleItems { get; set; } = new HashSet<FRItem>();

        public bool IsBundle => BundleItems.Count != 0;
    }

    public enum FRItemCategory
    {
        Food,
        Skins,
        Equipment,
        Familiar,
        Battle_Items,
        Trinket
    }

    public enum FRFoodType
    {
        Insect,
        Meat,
        Seafood,
        Plant
    }
}
