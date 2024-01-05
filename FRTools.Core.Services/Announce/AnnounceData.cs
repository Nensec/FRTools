using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;

namespace FRTools.Core.Services.Announce
{
    public abstract class AnnounceData
    {
        public abstract Type AnnouncerType { get; }
    }

    public class DominanceAnnounceData : AnnounceData
    {
        public override Type AnnouncerType => typeof(IDominanceAnnouncer);
        public Flight[] Flights { get; }

        public DominanceAnnounceData(Flight[] flights)
        {
            Flights = flights;
        }
    }

    public class FlashSaleData : AnnounceData
    {
        public override Type AnnouncerType => typeof(IFlashSaleAnnouncer);
        public FRItem FRItem { get; }
        public string MarketplaceLink { get; }

        public FlashSaleData(FRItem item, string marketplaceLink)
        {
            FRItem = item;
            MarketplaceLink = marketplaceLink;
        }
    }
}