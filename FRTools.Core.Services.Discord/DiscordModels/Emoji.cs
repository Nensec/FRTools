using FRTools.Core.Services.Discord.DiscordModels.UserModels;

namespace FRTools.Core.Services.Discord.DiscordModels
{
    public class Emoji
    {
        public string id { get; set; }
        public string name { get; set; }
        public string[] roles { get; set; }
        public User user { get; set; }
        public bool require_colons { get; set; }
        public bool managed { get; set; }
        public bool animated { get; set; }
    }
}
