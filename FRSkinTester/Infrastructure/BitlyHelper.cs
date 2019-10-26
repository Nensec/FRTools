using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FRTools.Web.Infrastructure
{
    public static class BitlyHelper
    {
        public static adnmaster.Bitly.BitlyClient BitlyClient { get; } = new adnmaster.Bitly.BitlyClient(ConfigurationManager.AppSettings["BitlyClientId"]);

        static BitlyHelper() => BitlyClient.ApplyAccessToken(ConfigurationManager.AppSettings["BitlyAT"]);
        public static string TryGenerateUrl(string url)
        {
            // bitly cannot create shortened links for localhost
            if (url.Contains("localhost"))
                return url;

            try
            {
                var result = BitlyClient.Links.Shorten(url);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    return url;
                return result.Data.ShortUrl;
            }
            catch
            {
                return url;
            }
        }
    }
}