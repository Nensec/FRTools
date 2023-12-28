using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.ResponseModels
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
        public IEnumerable<DiscordEmbed> Embeds { get; set; } = Enumerable.Empty<DiscordEmbed>();
        [JsonProperty("flags")]
        public MessageFlags Flags { get; set; }
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
        public bool? DIsabled { get; set; }
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
        [JsonProperty("channel_types")]
        public ChannelType? ChannelTypes { get; set; }
        [JsonProperty("placeholder")]
        public string? Placeholder { get; set; }

        public class String : DiscordInteractionResponseSelectComponent
        {
            public override ComponentType Type => ComponentType.STRING_SELECT;
            public IEnumerable<DiscordInteractionResponseSelectOption> options { get; set; } = Enumerable.Empty<DiscordInteractionResponseSelectOption>();
        }

        public class User : DiscordInteractionResponseSelectComponent
        {
            public override ComponentType Type => ComponentType.USER_SELECT;
            public IEnumerable<DiscordInteractionResponseDefaultSelect> options { get; set; } = Enumerable.Empty<DiscordInteractionResponseDefaultSelect>();
        }

        public class Role : DiscordInteractionResponseSelectComponent
        {
            public override ComponentType Type => ComponentType.ROLE_SELECT;
            public IEnumerable<DiscordInteractionResponseDefaultSelect> options { get; set; } = Enumerable.Empty<DiscordInteractionResponseDefaultSelect>();
        }

        public class Mentionable : DiscordInteractionResponseSelectComponent
        {
            public override ComponentType Type => ComponentType.MENTIONABLE_SELECT;
            public IEnumerable<DiscordInteractionResponseDefaultSelect> options { get; set; } = Enumerable.Empty<DiscordInteractionResponseDefaultSelect>();
        }

        public class Channel : DiscordInteractionResponseSelectComponent
        {
            public override ComponentType Type => ComponentType.CHANNEL_SELECT;
            public IEnumerable<DiscordInteractionResponseDefaultSelect> options { get; set; } = Enumerable.Empty<DiscordInteractionResponseDefaultSelect>();
        }
    }

    public class DiscordInteractionResponseSelectOption
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("description")]
        public string? Description { get; set; }
        [JsonProperty("emoji")]
        public Emoji? Emoji { get; set; }
        [JsonProperty("default")]
        public bool? Default { get; set; } = false;
    }

    public class DiscordInteractionResponseDefaultSelect
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
