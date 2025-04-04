using FRTools.Core.Common;
using FRTools.Core.Services.Interfaces;

namespace FRTools.Core.Services
{
    public class ItemAssetDataService : IItemAssetDataService
    {
        public async Task<byte[]> GetProxyDummyDragonApparel(int dragonBreed, int gender, int itemId)
        {
            using (var client = new HttpClient())
                return await client.GetByteArrayAsync(Helpers.GetProxyDummyDragonApparelUrl(dragonBreed, gender, itemId));
        }

        public async Task<byte[]> GetProxyDummyDragonGene(int dragonBreed, int gender, int itemId)
        {
            using (var client = new HttpClient())
                return await client.GetByteArrayAsync(Helpers.GetProxyDummyDragonGeneUrl(dragonBreed, gender, itemId));
        }

        public async Task<byte[]> GetProxyDummyDragonSkin(int dragonBreed, int gender, int itemId)
        {
            using (var client = new HttpClient())
                return await client.GetByteArrayAsync(Helpers.GetProxyDummyDragonSkinUrl(dragonBreed, gender, itemId));
        }

        public async Task<byte[]> GetProxyIcon(int itemId)
        {
            using (var client = new HttpClient())
                return await client.GetByteArrayAsync(Helpers.GetProxyIconUrl(itemId));
        }

        public async Task<byte[]> GetSceneArt(int itemId)
        {
            using (var client = new HttpClient())
                return await client.GetByteArrayAsync(string.Format(FRHelpers.SceneArtUrl, itemId));
        }

        public async Task<byte[]> GetFamiliarArt(int itemId)
        {
            using (var client = new HttpClient())
                return await client.GetByteArrayAsync(string.Format(FRHelpers.FamiliarArtUrl, itemId));
        }

        public async Task<byte[]> GetDragonRender(long dragonId)
        {
            using (var webClient = new HttpClient())
                return await webClient.GetByteArrayAsync(FRHelpers.GetRenderUrl(dragonId));
        }

        public async Task<byte[]> GetAssetArt(string assetUrl)
        {
            using (var client = new HttpClient())
                return await client.GetByteArrayAsync($"https://www1.flightrising.com{assetUrl}");
        }

        public async Task<byte[]> GetApparalRender(int dragonBreed, int gender, IEnumerable<int> itemIds)
        {
            using (var client = new HttpClient())
                return await client.GetByteArrayAsync(Helpers.GetProxyDummyDragonApparelUrl(dragonBreed, gender, itemIds));
        }
    }
}
