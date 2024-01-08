using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FRTools.Core.Data;
using HtmlAgilityPack;

namespace FRTools.Core.Common
{
    public static class Helpers
    {
        public static bool IsLocal = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));

        public static string GetProxyDummyDragonSkinUrl(DragonType dragonBreed, Gender gender, int itemId) => GetProxyDummyDragonSkinUrl((int)dragonBreed, (int)gender, itemId);
        public static string GetProxyDummyDragonSkinUrl(int dragonBreed, Gender gender, int itemId) => GetProxyDummyDragonSkinUrl(dragonBreed, (int)gender, itemId);
        public static string GetProxyDummyDragonSkinUrl(DragonType dragonBreed, int gender, int itemId) => GetProxyDummyDragonSkinUrl((int)dragonBreed, gender, itemId);
        public static string GetProxyDummyDragonSkinUrl(int dragonBreed, int gender, int itemId) => $"http{(IsLocal ? "" : "s")}://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/proxy/dragon/skin/{dragonBreed}/{gender}/{itemId}";
        public static string GetProxyDummyDragonGeneUrl(DragonType dragonBreed, Gender gender, int itemId) => GetProxyDummyDragonGeneUrl((int)dragonBreed, (int)gender, itemId);
        public static string GetProxyDummyDragonGeneUrl(int dragonBreed, Gender gender, int itemId) => GetProxyDummyDragonGeneUrl(dragonBreed, (int)gender, itemId);
        public static string GetProxyDummyDragonGeneUrl(DragonType dragonBreed, int gender, int itemId) => GetProxyDummyDragonGeneUrl((int)dragonBreed, gender, itemId);
        public static string GetProxyDummyDragonGeneUrl(int dragonBreed, int gender, int itemId) => $"http{(IsLocal ? "" : "s")}://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/proxy/dragon/gene/{dragonBreed}/{gender}/{itemId}";
        public static string GetProxyDummyDragonApparelUrl(DragonType dragonBreed, Gender gender, int itemId) => GetProxyDummyDragonApparelUrl((int)dragonBreed, (int)gender, itemId);
        public static string GetProxyDummyDragonApparelUrl(int dragonBreed, Gender gender, int itemId) => GetProxyDummyDragonApparelUrl(dragonBreed, (int)gender, itemId);
        public static string GetProxyDummyDragonApparelUrl(DragonType dragonBreed, int gender, int itemId) => GetProxyDummyDragonApparelUrl((int)dragonBreed, gender, itemId);
        public static string GetProxyDummyDragonApparelUrl(int dragonBreed, int gender, int itemId) => $"http{(IsLocal ? "" : "s")}://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/proxy/dragon/apparel/{dragonBreed}/{gender}/{itemId}";
        public static string GetProxyIconUrl(int itemId) => $"http{(IsLocal ? "" : "s")}://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/proxy/icon/{itemId}";

        public static async Task<HtmlDocument> LoadHtmlPage(string url)
        {
            var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true });

            var response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Found && response.Headers.Location != null)
            {
                return await LoadHtmlPage(response.Headers.Location.AbsoluteUri);
            }

            var resultDocument = new HtmlDocument();
            resultDocument.Load(response.Content.ReadAsStream(), Encoding.GetEncoding("utf-8"));

            return resultDocument;
        }
    }
}
