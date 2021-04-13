using System.Configuration;

namespace FRTools.Common
{
    public class SiteHelpers
    {
        public static string DummyDragonProxyUrl { get; } = $"{ConfigurationManager.AppSettings["WebsiteBaseUrl"]}/proxy/dummy/{{0}}/{{1}}";
        public static string DummyDragonGeneProxyUrl { get; } = $"{ConfigurationManager.AppSettings["WebsiteBaseUrl"]}/proxy/dummy/{{0}}/{{1}}?gene={{2}}";
        public static string DummyDragonApparelProxyUrl { get; } = $"{ConfigurationManager.AppSettings["WebsiteBaseUrl"]}/proxy/dummy/{{0}}/{{1}}?apparel={{2}}";
        public static string DummyDragonSkinProxyUrl { get; } = $"{ConfigurationManager.AppSettings["WebsiteBaseUrl"]}/proxy/dummy/{{0}}/{{1}}?skin={{2}}";
        public static string IconProxyUrl { get; } = $"{ConfigurationManager.AppSettings["WebsiteBaseUrl"]}/proxy/icon/{{0}}";
        public static string ProfilePageUrl { get; } = $"{ConfigurationManager.AppSettings["WebsiteBaseUrl"]}/profile/{{0}}";
    }
}
