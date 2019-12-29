using FRTools.Data.DataModels.DiscordModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FRTools.Web.Models
{
    public class DiscordMetadata
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

    public class DiscordModule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Aliases { get; set; }
        public List<DiscordCommand> Commands { get; set; } = new List<DiscordCommand>();
        public List<DiscordSetting> Settings { get; set; } = new List<DiscordSetting>();
        public bool RequireOwner { get; set; }
    }

    public class DiscordCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Aliases { get; set; }
        public long RequireGuildPermission { get; set; }
        public long RequireChannelPermission { get; set; }
        public bool RequireOwner { get; set; }
    }

    public class DiscordBaseViewModel
    {
        public DiscordUser CurrentUser { get; set; }
    }

    public class ServersViewModel : DiscordBaseViewModel
    {
        public List<DiscordServer> Servers { get; set; }
    }

    public class ServerViewModel : DiscordBaseViewModel
    {
        public long SelectedServer { get; set; }
    }

    public class ModulesViewModel : ServerViewModel
    {
        public List<DiscordModule> Modules { get; set; }
    }

    public class ModuleViewModel : ServerViewModel
    {
        public DiscordModule SelectedModule { get; set; }        
    }

    public class CommandViewModel : ModuleViewModel
    {
        public DiscordCommand SelectedCommand { get; set; }
    }
}