using System;
using FRTools.Core.Data;

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
        public static string GetDiscordInteractionUrl() => $"http{(IsLocal ? "" : "s")}://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/discord";
    }
}
