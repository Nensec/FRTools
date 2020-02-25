using BitlyAPI;
using System.Configuration;
using System.Threading.Tasks;

namespace FRTools.Web.Infrastructure
{
    public static class BitlyHelper
    {
        public static Bitly BitlyClient { get; } = new Bitly(ConfigurationManager.AppSettings["BitlyAT"]);

        public static async Task<string> TryGenerateUrl(string url)
        {
            // bitly cannot create shortened links for localhost
            if (url.Contains("localhost"))
                return url;

            try
            {
                var result = await BitlyClient.PostShorten(url);
                return result.Link;
            }
            catch
            {
                return url;
            }
        }
    }
}