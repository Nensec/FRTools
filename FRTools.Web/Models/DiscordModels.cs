using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FRTools.Web.Models
{
    class DiscordMetadata
    {
        public List<DiscordSetting> BotSettings { get; set; } = new List<DiscordSetting>();
        public List<DiscordModule> Modules { get; set; } = new List<DiscordModule>();
    }

    public class DiscordSetting
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
    }

    class DiscordModule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Aliases { get; set; }
        public List<DiscordCommand> Commands { get; set; } = new List<DiscordCommand>();
        public List<DiscordSetting> Settings { get; set; } = new List<DiscordSetting>();
        public bool RequireOwner { get; set; }
    }

    class DiscordCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Aliases { get; set; }
        public long RequireGuildPermission { get; set; }
        public long RequireChannelPermission { get; set; }
        public bool RequireOwner { get; set; }
    }
}