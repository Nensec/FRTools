using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IFRUserService
    {
        Task<FRUser> GetOrUpdateFRUser(int userId, DataContext ctx = null);
        Task<FRUser> GetOrUpdateFRUser(string username, DataContext ctx = null);
        Task<FRUser> UpdateFRUser(FRUser frUser);
    }
}