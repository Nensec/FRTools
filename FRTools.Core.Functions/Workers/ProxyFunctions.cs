using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FRTools.Core.Common;
using FRTools.Core.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class ProxyFunctions : FunctionBase
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ProxyFunctions> _logger;

        public ProxyFunctions(DataContext dataContext, ILogger<ProxyFunctions> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        [Function(nameof(ProxyDummyDragonSkin))]
        public async Task<HttpResponseData> ProxyDummyDragonSkin([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/dragon/skin/{dragonType:int}/{gender:int}/{skinId:int}")] HttpRequestData request, int dragonType, int gender, int skinId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a ProxyDummyDragonSkin request.");

            using (var client = new HttpClient())
            {
                var apparelBytes = await client.GetByteArrayAsync(string.Format(FRHelpers.DressingRoomDummySkinUrl, dragonType, gender, skinId));
                return await FileResult(request, apparelBytes);
            }
        }

        [Function(nameof(ProxyDummyDragonGene))]
        public async Task<HttpResponseData> ProxyDummyDragonGene([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/dragon/gene/{dragonType:int}/{gender:int}/{geneId:int}")] HttpRequestData request, int dragonType, int gender, int geneId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a ProxyDummyDragonGene request.");

            var item = _dataContext.FRItems.FirstOrDefault(x => x.FRId == geneId);
            if (item == null)
                return request.CreateResponse(System.Net.HttpStatusCode.BadRequest);

            int primary = 0, secondary = 0, tertiary = 0;
            int primaryColor = 0, secondaryColor = 0, tertiaryColor = 0;
            var random = new Random();
            byte[] geneBytes;

            if (item.Name.StartsWith("Primary"))
            {
                primary = FRHelpers.GetGeneId(item);
                primaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
            }
            if (item.Name.StartsWith("Secondary"))
            {
                secondary = FRHelpers.GetGeneId(item);
                secondaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
            }
            if (item.Name.StartsWith("Tertiary"))
            {
                tertiary = FRHelpers.GetGeneId(item);
                tertiaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
            }

            using (var client = new HttpClient())
                geneBytes = await client.GetByteArrayAsync(await GeneratedFRHelpers.GenerateDragonImageUrl(dragonType, gender, 1, primary, primaryColor, secondary, secondaryColor, tertiary, tertiaryColor, random.Next(0, Enum.GetValues(typeof(Element)).Length + 1), random.Next(0, Enum.GetValues(typeof(EyeType)).Length + 1)));

            return await FileResult(request, geneBytes);
        }

        [Function(nameof(ProxyDummyDragonApparel))]
        public async Task<HttpResponseData> ProxyDummyDragonApparel([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/dragon/apparel/{dragonType:int}/{gender:int}/{apparelId:int}")] HttpRequestData request, int dragonType, int gender, int apparelId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a ProxyDummyDragonApparel request.");

            using (var client = new HttpClient())
            {
                var apparelBytes = await client.GetByteArrayAsync(string.Format(FRHelpers.DressingRoomDummyApparalUrl, dragonType, gender, apparelId));

                return await FileResult(request, apparelBytes);
            }
        }

        [Function(nameof(ProxyIcon))]
        public async Task<HttpResponseData> ProxyIcon([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/icon/{itemId:int}")] HttpRequestData request, int itemId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a ProxyIcon request.");

            var item = _dataContext.FRItems.FirstOrDefault(x => x.FRId == itemId);
            if (item == null)
                return request.CreateResponse(System.Net.HttpStatusCode.BadRequest);

            using (var client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync("https://flightrising.com" + item.IconUrl);

                return await FileResult(request, bytes);
            }
        }
    }
}
