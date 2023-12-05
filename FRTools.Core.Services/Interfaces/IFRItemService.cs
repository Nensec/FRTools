using FRTools.Core.Data.DataModels.FlightRisingModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IFRItemService
    {
        Task<FRItem> FetchItem(int itemId, string category = "skins");
    }
}