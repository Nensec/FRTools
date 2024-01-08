using System.Text.Json;
using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.Discord.DiscordModels.GuildModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using FRTools.Core.Services.Discord.DiscordModels.UserModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.MessageModels
{
    public class Message
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }
        [JsonProperty("author")]
        public User Author { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("edited_timestamp")]
        public DateTime EditedTimestamp { get; set; }
        [JsonProperty("tts")]
        public bool TTS { get; set; }
        [JsonProperty("mention_everyone")]
        public bool MentionEveryone { get; set; }
        [JsonProperty("mentions")]
        public IEnumerable<User> Mentions { get; set; } = Enumerable.Empty<User>();
        [JsonProperty("mention_roles")]
        public IEnumerable<Role> MentionRoles { get; set; } = Enumerable.Empty<Role>();
        [JsonProperty("mention_channels")]
        public IEnumerable<Channel> MentionChannels { get; set; } = Enumerable.Empty<Channel>();
        [JsonProperty("attachments")]
        public IEnumerable<Attachment> Attachments { get; set; } = Enumerable.Empty<Attachment>();
        [JsonProperty("embeds")]
        public IEnumerable<DiscordEmbed> Embeds { get; set; } = Enumerable.Empty<DiscordEmbed>();
        [JsonProperty("reactions")]
        public IEnumerable<Reaction> Users { get; set; } = Enumerable.Empty<Reaction>();
        [JsonProperty("nonce")]
        public int Nonce { get; set; }
        [JsonProperty("pinned")]
        public bool Pinned { get; set; }
        [JsonProperty("webhook_id")]
        public ulong WebhookId { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("activity")]
        public Activity Activity { get; set; }
        [JsonProperty("message_reference")]
        public MessageReference MessageReference { get; set; }
        [JsonProperty("flags")]
        public int Flags { get; set; }
        [JsonProperty("interaction")]
        public MessageInteraction Interaction { get; set; }
        [JsonProperty("thread")]
        public Channel Thread { get; set; }
        [JsonProperty("components")]
        public IEnumerable<DiscordInteractionResponseComponent> Components { get; set; } = Enumerable.Empty<DiscordInteractionResponseComponent>();
        [JsonProperty("position")]
        public int Position { get; set; }

        //application? partial application object sent with Rich Presence-related chat embeds
        //application_id?	snowflake	if the message is an Interaction or application-owned webhook, this is the id of the application
        //sticker_items? array of message sticker item objects sent if the message contains stickers
        //stickers? array of sticker objects Deprecated the stickers sent with the message
        //role_subscription_data?	role subscription data object data of the role subscription purchase or renewal that prompted this ROLE_SUBSCRIPTION_PURCHASE message
        //resolved? resolved data
    }
}
