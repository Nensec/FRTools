using System.ComponentModel;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;

namespace FRTools.Core.Services.Announce
{
    public abstract class AnnounceData
    {
        public abstract Type AnnouncerType { get; }
    }

    public abstract class AnnounceData<T> : AnnounceData
    {
        public override Type AnnouncerType => typeof(T);
    }

    [Description("dominance")]
    public class DominanceAnnounceData : AnnounceData<IDominanceAnnouncer>
    {
        public Flight[] Flights { get; }

        public DominanceAnnounceData(Flight[] flights)
        {
            Flights = flights;
        }
    }

    [Description("flashsale")]
    public class FlashSaleAnnounceData : AnnounceData<IFlashSaleAnnouncer>
    {
        public FRItem FRItem { get; }
        public string MarketplaceLink { get; }

        public FlashSaleAnnounceData(FRItem item, string marketplaceLink)
        {
            FRItem = item;
            MarketplaceLink = marketplaceLink;
        }
    }

    [Description("new items")]
    public class NewItemsAnnounceData : AnnounceData<INewItemAnnouncer>
    {
        public IEnumerable<FRItem> FRItems { get; }

        public NewItemsAnnounceData(IEnumerable<FRItem> items)
        {
            FRItems = items;
        }
    }
}
