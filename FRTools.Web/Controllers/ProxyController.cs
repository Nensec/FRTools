using FRTools.Common;
using FRTools.Data;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("proxy")]
    public class ProxyController : BaseController
    {
        // In-memory caches to prevent requesting the same image over and over again, at least until site restart
        private static readonly Dictionary<(int, int), byte[]> _dummyCache = new Dictionary<(int, int), byte[]>();
        private static readonly Dictionary<(int, int, string), byte[]> _apparelCache = new Dictionary<(int, int, string), byte[]>();
        private static readonly Dictionary<(int, int, string), byte[]> _skinCache = new Dictionary<(int, int, string), byte[]>();
        private static readonly Dictionary<int, byte[]> _iconCache = new Dictionary<int, byte[]>();

        [Route("dummy/{dragonType}/{gender}", Name = "GetDummyDragon")]
        [Route("dummy", Name = "GetDummyDragonQueryString")]
        public async Task<ActionResult> GetDummyDragon(int dragonType, int gender, string apparel = null, string skin = null)
        {
            if (apparel != null)
            {
                if (!_apparelCache.TryGetValue((dragonType, gender, apparel), out var apparelBytes))
                {
                    using (var client = new WebClient())
                        _apparelCache[(dragonType, gender, apparel)] = apparelBytes = await client.DownloadDataTaskAsync(string.Format(FRHelpers.DressingRoomDummyApparalUrl, dragonType, gender, apparel));
                }
                return File(apparelBytes, "image/png");
            }

            if (skin != null)
            {
                if (!_skinCache.TryGetValue((dragonType, gender, skin), out var skinBytes))
                {
                    using (var client = new WebClient())
                        _skinCache[(dragonType, gender, apparel)] = skinBytes = await client.DownloadDataTaskAsync(string.Format(FRHelpers.DressingRoomDummySkinUrl, dragonType, gender, skin));
                }
                return File(skinBytes, "image/png");
            }

            if (!_dummyCache.TryGetValue((dragonType, gender), out var bytes))
            {
                var dummyDragon = await FRHelpers.GetDragonBaseImage(string.Format(FRHelpers.DressingRoomDummyUrl, dragonType, gender));
                using (var memStream = new MemoryStream())
                {
                    dummyDragon.Save(memStream, ImageFormat.Png);
                    _dummyCache[(dragonType, gender)] = bytes = memStream.ToArray();
                }
            }

            return File(bytes, "image/png");
        }

        [Route("icon/{id}/", Name = "GetIcon")]
        public async Task<ActionResult> GetIcon(int id)
        {
            if (!_iconCache.TryGetValue(id, out var bytes))
            {
                var item = DataContext.FRItems.FirstOrDefault(x => x.FRId == id);
                if (item == null)
                    return null;

                using (var client = new WebClient())
                    bytes = await client.DownloadDataTaskAsync("https://flightrising.com" + item.IconUrl);
            }

            return File(bytes, "image/png");
        }

    }
}