using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.GuildModels
{
    public class RoleTag
    {
        [JsonProperty("bot_id")]
        public ulong BotId { get; set; }
        [JsonProperty("integration_id")]
        public ulong IntegrationId { get; set; }
        [JsonProperty("premium_subscriber")]
        public bool PremiumSubscriber { get; set; }
        [JsonProperty("subscription_listing_id")]
        public ulong SubscriptionListingId { get; set; }
        [JsonProperty("available_for_purchase")]
        public bool AvailableForPurchase { get; set; }
        [JsonProperty("guild_connections")]
        public bool GuildConnections { get; set; }
    }
}
