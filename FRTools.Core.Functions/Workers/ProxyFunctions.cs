using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FRTools.Core.Common;
using FRTools.Core.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class ProxyFunctions : FunctionBase
    {
        private readonly DataContext _dataContext;

        public ProxyFunctions(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [FunctionName(nameof(ProxyDummyDragonSkin))]
        public async Task<IActionResult> ProxyDummyDragonSkin([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/dragon/skin/{dragonType:int}/{gender:int}/{skinId:int}")] HttpRequest request, int dragonType, int gender, int skinId, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a ProxyDummyDragonSkin request.");

            using (var client = new HttpClient())
            {
                var apparelBytes = await client.GetByteArrayAsync(string.Format(FRHelpers.DressingRoomDummySkinUrl, dragonType, gender, skinId));

                return new FileContentResult(apparelBytes, "image/png");
            }
        }

        [FunctionName(nameof(ProxyDummyDragonGene))]
        public async Task<IActionResult> ProxyDummyDragonGene([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/dragon/gene/{dragonType:int}/{gender:int}/{geneId:int}")] HttpRequest request, int dragonType, int gender, int geneId, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a ProxyDummyDragonGene request.");

            var item = _dataContext.FRItems.FirstOrDefault(x => x.FRId == geneId);
            int primary = 0, secondary = 0, tertiary = 0;
            int primaryColor = 0, secondaryColor = 0, tertiaryColor = 0;
            var itemSplit = item.Name.Split(':', '(');
            var random = new Random();
            byte[] geneBytes;
            switch (itemSplit[0])
            {
                case "Primary Gene":
                    primary = (int)Enum.Parse(((DragonType)dragonType).PrimaryGeneType(), itemSplit[1].Replace("-", "").Replace(" ", ""));
                    primaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
                    break;
                case "Secondary Gene":
                    secondary = (int)Enum.Parse(((DragonType)dragonType).SecondaryGeneType(), itemSplit[1].Replace("-", "").Replace(" ", ""));
                    secondaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
                    break;
                case "Tertiary Gene":
                    tertiary = (int)Enum.Parse(((DragonType)dragonType).TertiaryGeneType(), itemSplit[1].Replace("-", "").Replace(" ", ""));
                    tertiaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
                    break;
            }

            using (var client = new HttpClient())
                geneBytes = await client.GetByteArrayAsync(await GeneratedFRHelpers.GenerateDragonImageUrl(dragonType, gender, 1, primary, primaryColor, secondary, secondaryColor, tertiary, tertiaryColor, random.Next(0, Enum.GetValues(typeof(Element)).Length + 1), random.Next(0, Enum.GetValues(typeof(EyeType)).Length + 1)));

            return new FileContentResult(geneBytes, "image/png");
        }

        [FunctionName(nameof(ProxyDummyDragonApparel))]
        public async Task<IActionResult> ProxyDummyDragonApparel([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/dragon/apparel/{dragonType:int}/{gender:int}/{apparelId:int}")] HttpRequest request, int dragonType, int gender, int apparelId, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a ProxyDummyDragonApparel request.");

            using (var client = new HttpClient())
            {
                var apparelBytes = await client.GetByteArrayAsync(string.Format(FRHelpers.DressingRoomDummyApparalUrl, dragonType, gender, apparelId));

                return new FileContentResult(apparelBytes, "image/png");
            }
        }

        [FunctionName(nameof(ProxyIcon))]
        public async Task<IActionResult> ProxyIcon([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/icon/{itemId:int}")] HttpRequest request, int itemId, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a ProxyIcon request.");

            var item = _dataContext.FRItems.FirstOrDefault(x => x.FRId == itemId);
            if (item == null)
                return null;

            using (var client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync("https://flightrising.com" + item.IconUrl);

                return new FileContentResult(bytes, "image/png");
            }
        }
    }
}
