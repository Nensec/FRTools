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
        public List<ServerViewModel> Servers { get; set; }
    }

    public class ServerViewModel
    {
        public long ServerId { get; set; }
        public string IconBase64 { get; set; }
        public string ServerName { get; set; }
        public int UserCount { get; set; }
        public List<DiscordRoleViewModel> Roles { get; set; }
        public List<DiscordChannelViewModel> Channels { get; set; }
        public List<DiscordSettingViewModel> BotSettings { get; set; }
        public List<DiscordModule> Modules { get; set; }
    }

    public class DiscordRoleViewModel
    {
        public ServerViewModel ParentServer { get; set; }
        public string RoleName { get; set; }
        public long RoleId { get; set; }
    }

    public class DiscordChannelViewModel
    {
        public ServerViewModel ParentServer { get; set; }
        public DiscordChannelType DiscordChannelType { get; set; }
        public string ChannelName { get; set; }
        public long ChannelId { get; set; }
    }

    public class DiscordSettingViewModel
    {
        public ServerViewModel ParentServer { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string SettingType { get; set; }
        public string SettingName { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
    }

    public class ModuleViewModel
    {
        public ServerViewModel ParentServer { get; set; }
        public DiscordModule SelectedModule { get; set; }    
        public List<DiscordSettingViewModel> ModuleSettings { get; set; }
    }

    public class CommandViewModel
    {
        public ServerViewModel ParentServer { get; set; }
        public DiscordModule SelectedModule { get; set; }
        public DiscordCommand SelectedCommand { get; set; }
    }
}