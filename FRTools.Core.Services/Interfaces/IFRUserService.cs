using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IFRUserService
    {
        Task<FRUser?> GetOrUpdateFRUser(int? userId);
        Task<FRUser?> GetOrUpdateFRUser(string? username);
        Task<FRUser?> UpdateFRUser(FRUser frUser);
    }
}