using FRTools.Core.Data;

namespace FRTools.Core.Services.Interfaces
{
    public interface IItemAssetDataService
    {
        Task<byte[]> GetProxyIcon(int itemId);
        Task<byte[]> GetProxyDummyDragonSkin(int dragonBreed, int gender, int itemId);
        Task<byte[]> GetProxyDummyDragonGene(int dragonBreed, int gender, int itemId);
        Task<byte[]> GetProxyDummyDragonApparel(int dragonBreed, int gender, int itemId);
        Task<byte[]> GetSceneArt(int itemId);
        Task<byte[]> GetAssetArt(string assetUrl);
        Task<byte[]> GetFamiliarArt(int itemId);
        Task<byte[]> GetDragonRender(long dragonId);
    }
}
