namespace FRTools.Core.Services.DiscordModels
{
    public enum InteractionType
    {
        PING = 1,
        APPLICATION_COMMAND = 2,
        MESSAGE_COMPONENT = 3,
        APPLICATION_COMMAND_AUTOCOMPLETE = 4,
        MODAL_SUBMIT = 5
    }

    public enum InteractionResponseType
    {
        PONG = 1,
        CHANNEL_MESSAGE_WITH_SOURCE = 4,
        DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE = 5,
        DEFERRED_UPDATE_MESSAGE = 6,
        UPDATE_MESSAGE = 7,
        APPLICATION_COMMAND_AUTOCOMPLETE_RESULT = 8,
        MODAL = 9,
        PREMIUM_REQUIRED = 10
    }

    public enum AppCommandType
    {
        CHAT_INPUT = 1,
        USER = 2,
        MESSAGE = 3
    }
    public enum AppCommandOptionType
    {
        SUB_COMMAND = 1,
        SUB_COMMAND_GROUP = 2,
        STRING = 3,
        INTEGER = 4,
        BOOLEAN = 5,
        USER = 6,
        CHANNEL = 7,
        ROLE = 8,
        MENTIONABLE = 9,
        NUMBER = 10,
        ATTACHMENT = 11,
    }

    public enum ChannelType
    {
        GUILD_TEXT = 0,
        DM = 1,
        GUILD_VOICE = 2,
        GROUP_DM = 3,
        GUILD_CATEGORY = 4,
        GUILD_ANNOUNCEMENT = 5,
        ANNOUNCEMENT_THREAD = 10,
        PUBLIC_THREAD = 11,
        PRIVATE_THREAD = 12,
        GUILD_STAGE_VOICE = 13,
        GUILD_DIRECTORY = 14,
        GUILD_FORUM = 15,
        GUILD_MEDIA = 16,
    }

    public enum ComponentType
    {
        ACTION_ROW = 1,
        BUTTON = 2,
        STRING_SELECT = 3,
        TEXT_INPUT = 4,
        USER_SELECT = 5,
        ROLE_SELECT = 6,
        MENTIONABLE_SELECT = 7,
        CHANNEL_SELECT = 8,
    }

    public enum ButtonComponentStyle
    {
        PRIMARY = 1,
        SECONDARY = 2,
        SUCCESS = 3,
        DANGER = 4,
        LINK = 5
    }

    [Flags]
    public enum Permissions : long
    {
        CREATE_INSTANT_INVITE = 1 << 0,
        KICK_MEMBERS = 1 << 1,
        BAN_MEMBERS = 1 << 2,
        ADMINISTRATOR = 1 << 3,
        MANAGE_CHANNELS = 1 << 4,
        MANAGE_GUILD = 1 << 5,
        ADD_REACTIONS = 1 << 6,
        VIEW_AUDIT_LOG = 1 << 7,
        PRIORITY_SPEAKER = 1 << 8,
        STREAM = 1 << 9,
        VIEW_CHANNEL = 1 << 10,
        SEND_MESSAGES = 1 << 11,
        SEND_TTS_MESSAGES = 1 << 12,
        MANAGE_MESSAGES = 1 << 13,
        EMBED_LINKS = 1 << 14,
        ATTACH_FILES = 1 << 15,
        READ_MESSAGE_HISTORY = 1 << 16,
        MENTION_EVERYONE = 1 << 17,
        USE_EXTERNAL_EMOJIS = 1 << 18,
        VIEW_GUILD_INSIGHTS = 1 << 19,
        CONNECT = 1 << 20,
        SPEAK = 1 << 21,
        MUTE_MEMBERS = 1 << 22,
        DEAFEN_MEMBERS = 1 << 23,
        MOVE_MEMBERS = 1 << 24,
        USE_VAD = 1 << 25,
        CHANGE_NICKNAME = 1 << 26,
        MANAGE_NICKNAMES = 1 << 27,
        MANAGE_ROLES = 1 << 28,
        MANAGE_WEBHOOKS = 1 << 29,
        MANAGE_GUILD_EXPRESSIONS = 1 << 30,
        USE_APPLICATION_COMMANDS = 1 << 31,
        REQUEST_TO_SPEAK = 1 << 32,
        MANAGE_EVENTS = 1 << 33,
        MANAGE_THREADS = 1 << 34,
        CREATE_PUBLIC_THREADS = 1 << 35,
        CREATE_PRIVATE_THREADS = 1 << 36,
        USE_EXTERNAL_STICKERS = 1 << 37,
        SEND_MESSAGES_IN_THREADS = 1 << 38,
        USE_EMBEDDED_ACTIVITIES = 1 << 39,
        MODERATE_MEMBERS = 1 << 40,
        VIEW_CREATOR_MONETIZATION_ANALYTICS = 1 << 41,
        USE_SOUNDBOARD = 1 << 42,
        CREATE_GUILD_EXPRESSIONS = 1 << 43,
        CREATE_EVENTS = 1 << 44,
        USE_EXTERNAL_SOUNDS = 1 << 45,
        SEND_VOICE_MESSAGES = 1 << 46
    }

    [Flags]
    public enum MessageFlags
    {
        CROSSPOSTED = 1 << 0,
        IS_CROSSPOST = 1 << 1,
        SUPPRESS_EMBEDS = 1 << 2,
        SOURCE_MESSAGE_DELETED = 1 << 3,
        URGENT = 1 << 4,
        HAS_THREAD = 1 << 5,
        EPHEMERAL = 1 << 6,
        LOADING = 1 << 7,
        FAILED_TO_MENTION_SOME_ROLES_IN_THREAD = 1 << 8,
        SUPPRESS_NOTIFICATIONS = 1 << 12,
        IS_VOICE_MESSAGE = 1 << 13,
        HAS_SNAPSHOT = 1 << 14,
        IS_COMPONENTS_V2 = 1 << 15,
    }

    public enum MessageActivity
    {
        JOIN = 1,
        SPECTATE = 2,
        LISTEN = 3,
        JOIN_REQUEST = 4
    }
}
