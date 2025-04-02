using System.Linq.Expressions;
using FRTools.Core.Data.DataModels.FlightRisingModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IFRItemService
    {
        Task<FRItem?> FetchItemFromFR(int itemId, string category = "skins");
        Task<List<int>> FindMissingIds();
        Task<int> GetHighestItemId();
        Task<FRItem?> GetItem(int itemId);
        Task<IEnumerable<FRItem>> SearchItems(Expression<Func<FRItem, bool>> expression);
        Task SetItemBundle(FRItem fRItem, IEnumerable<FRItem> fRItems);
    }
}