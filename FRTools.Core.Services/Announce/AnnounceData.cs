using System.ComponentModel;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;

namespace FRTools.Core.Services.Announce
{
    public abstract class AnnounceData
    {
        public abstract Type AnnouncerType { get; }
    }

    [Description("dominance")]
    public class DominanceAnnounceData : AnnounceData
    {
        public override Type AnnouncerType => typeof(IDominanceAnnouncer);
        public Flight[] Flights { get; }

        public DominanceAnnounceData(Flight[] flights)
        {
            Flights = flights;
        }
    }

    [Description("flashsale")]
    public class FlashSaleAnnounceData : AnnounceData
    {
        public override Type AnnouncerType => typeof(IFlashSaleAnnouncer);
        public FRItem FRItem { get; }
        public string MarketplaceLink { get; }

        public FlashSaleAnnounceData(FRItem item, string marketplaceLink)
        {
            FRItem = item;
            MarketplaceLink = marketplaceLink;
        }
    }

    [Description("new items")]
    public class NewItemsAnnounceData : AnnounceData
    {
        public override Type AnnouncerType => typeof(INewItemAnnouncer);
        public FRItem[] FRItems { get; }

        public NewItemsAnnounceData(FRItem[] items)
        {
            FRItems = items;
        }
    }
}
