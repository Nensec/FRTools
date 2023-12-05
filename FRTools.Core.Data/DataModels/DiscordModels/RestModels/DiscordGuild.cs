namespace FRTools.Core.Data.DataModels.DiscordModels.RestModels
{

    public class DiscordGuild
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public object Description { get; set; }
        public string Splash { get; set; }
        public object Discovery_splash { get; set; }
        public int Approximate_member_count { get; set; }
        public int Approximate_presence_count { get; set; }
        public string[] Features { get; set; }
        public DiscordEmoji[] Emojis { get; set; }
        public string Banner { get; set; }
        public long Owner_id { get; set; }
        public object Application_id { get; set; }
        public object Region { get; set; }
        public object Afk_channel_id { get; set; }
        public int Afk_timeout { get; set; }
        public object System_channel_id { get; set; }
        public bool Widget_enabled { get; set; }
        public string Widget_channel_id { get; set; }
        public int Verification_level { get; set; }
        public DiscordRole[] Roles { get; set; }
        public int Default_message_notifications { get; set; }
        public int Mfa_level { get; set; }
        public int Explicit_content_filter { get; set; }
        public object Max_presences { get; set; }
        public int Max_members { get; set; }
        public int Max_video_channel_users { get; set; }
        public string Vanity_url_code { get; set; }
        public int Premium_tier { get; set; }
        public int Premium_subscription_count { get; set; }
        public int System_channel_flags { get; set; }
        public string Preferred_locale { get; set; }
        public object Rules_channel_id { get; set; }
        public object Public_updates_channel_id { get; set; }
        public bool Owner { get; set; }
        public long Permissions { get; set; }
    }
}
