using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels
{
    public abstract class DiscordInteractionResponse
    {
        [JsonProperty("type")]
        public abstract InteractionResponseType Type { get; }
        [JsonProperty("data")]
        public DiscordInteractionResponseData? Data { get; set; }

        public class ContentResponse : DiscordInteractionResponse
        {
            public override InteractionResponseType Type => InteractionResponseType.CHANNEL_MESSAGE_WITH_SOURCE;
        }

        public class DefferedContentResponse : DiscordInteractionResponse
        {
            public override InteractionResponseType Type => InteractionResponseType.DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE;
        }

        public class EphemeralDeferredContentResponse : DefferedContentResponse
        {
            public EphemeralDeferredContentResponse()
            {
                Data = new DiscordInteractionResponseData { Flags = MessageFlags.EPHEMERAL };
            }
        }

        public class PongResponse : DiscordInteractionResponse
        {
            public override InteractionResponseType Type => InteractionResponseType.PONG;
        }
    }

    public class DiscordInteractionResponseData
    {
        [JsonProperty("content")]
        public string? Content { get; set; }
        [JsonProperty("embeds")]
        public IEnumerable<DiscordEmbed> Embeds { get; set; }
        [JsonProperty("flags")]
        public MessageFlags? Flags { get; set; }
        [JsonProperty("components")]
        public IEnumerable<DiscordInteractionResponseComponent> Components { get; set; } = Enumerable.Empty<DiscordInteractionResponseComponent>();
    }

    public abstract class DiscordInteractionResponseComponent
    {
        [JsonProperty("type")]
        public abstract ComponentType Type { get; }
    }

    public class DiscordInteractionResponseActionRowComponent : DiscordInteractionResponseComponent
    {
        public override ComponentType Type => ComponentType.ACTION_ROW;
        [JsonProperty("components")]
        public IEnumerable<IDiscordInteractionResponseActionRowComponent> Components { get; set; } = Enumerable.Empty<IDiscordInteractionResponseActionRowComponent>();
    }

    public interface IDiscordInteractionResponseActionRowComponent { }

    public class DiscordInteractionResponseButtonComponent : DiscordInteractionResponseComponent, IDiscordInteractionResponseActionRowComponent
    {
        public override ComponentType Type => ComponentType.BUTTON;
        [JsonProperty("style")]
        public ButtonComponentStyle Style { get; set; }
        [JsonProperty("label")]
        public string? Label { get; set; }
        [JsonProperty("emoji")]
        public Emoji? Emoji { get; set; }
        [JsonProperty("custom_id")]
        public string? CustomId { get; set; }
        [JsonProperty("url")]
        public string? Url { get; set; }
        [JsonProperty("disabled")]
        public bool? Disabled { get; set; }
    }

    public abstract class DiscordInteractionResponseSelectComponent : DiscordInteractionResponseComponent, IDiscordInteractionResponseActionRowComponent
    {
        [JsonProperty("custom_id")]
        public string? Custom_id { get; set; }
        [JsonProperty("disabled")]
        public bool? Disabled { get; set; }
        [JsonProperty("min_values")]
        public int? MinValues { get; set; } = 1;
        [JsonProperty("max_values")]
        public int? MaxValues { get; set; } = 1;
        [JsonProperty("placeholder")]
        public string? Placeholder { get; set; }

        public class String : DiscordInteractionResponseSelectComponent
        {
            [JsonProperty("label")]
            public string? Label { get; set; }
            public override ComponentType Type => ComponentType.STRING_SELECT;
            [JsonProperty("options")]
            public IEnumerable<DiscordInteractionSelectOption> Options { get; set; } = Enumerable.Empty<DiscordInteractionSelectOption>();
        }

        public class User : DiscordInteractionResponseSelectComponent
        {
            public override ComponentType Type => ComponentType.USER_SELECT;
        }

        public class Role : DiscordInteractionResponseSelectComponent
        {
            public override ComponentType Type => ComponentType.ROLE_SELECT;
        }

        public class Mentionable : DiscordInteractionResponseSelectComponent
        {
            public override ComponentType Type => ComponentType.MENTIONABLE_SELECT;
        }

        public class Channel : DiscordInteractionResponseSelectComponent
        {
            public override ComponentType Type => ComponentType.CHANNEL_SELECT;
            [JsonProperty("channel_types")]
            public ChannelType? ChannelTypes { get; set; }
        }
    }

    public class DiscordInteractionResponseDefaultSelect
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
