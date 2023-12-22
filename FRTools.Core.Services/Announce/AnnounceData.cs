using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;

namespace FRTools.Core.Services.Announce
{
    public abstract class AnnounceData
    {
    }

    public class DominanceAnnounceData : AnnounceData
    {
        public Flight[] Flights { get; }

        public DominanceAnnounceData(Flight[] flights)
        {
            Flights = flights;
        }
    }

    public class FlashSaleData : AnnounceData
    {
        public FRItem FRItem { get; }
        public string MarketplaceLink { get; }

        public FlashSaleData(FRItem item, string marketplaceLink)
        {
            FRItem = item;
            MarketplaceLink = marketplaceLink;
        }
    }
}
