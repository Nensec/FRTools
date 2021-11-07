﻿using FRTools.Data.DataModels;
using FRTools.Data.DataModels.DiscordModels;
using System.Collections.Generic;
using System.Linq;

namespace FRTools.Common
{
    public class DiscordMetadata
    {
        public List<DiscordSettingCategory> BotSettingCategories { get; set; } = new List<DiscordSettingCategory>();
        public List<DiscordSetting> BotSettings { get; set; } = new List<DiscordSetting>();
        public List<DiscordModule> Modules { get; set; } = new List<DiscordModule>();

        public List<DiscordSetting> AllSettings => BotSettings.Concat(Modules.SelectMany(x => x.Settings)).Concat(Modules.SelectMany(x => x.Commands.SelectMany(c => c.Settings))).ToList();
    }

    public class DiscordSetting
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
        public string[] ExtraArgs { get; set; }
        public DiscordSettingCategory Category { get; set; }
        public int Order { get; set; }
        public string DefaultValue { get; set; }
    }

    public class DiscordSettingCategory
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class DiscordModule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Aliases { get; set; }
        public List<DiscordCommand> Commands { get; set; } = new List<DiscordCommand>();
        public List<DiscordSettingCategory> SettingCategories { get; set; } = new List<DiscordSettingCategory>();
        public List<DiscordSetting> Settings { get; set; } = new List<DiscordSetting>();
        public DiscordHelp Help { get; set; }
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
        public DiscordHelp Help { get; set; }
        public DiscordModule ParentModule { get; set; }
        public List<DiscordCommandParameter> Parameters { get; set; }
        public List<DiscordSubCommand> SubCommands { get; set; }
        public List<DiscordSetting> Settings { get; set; }
    }

    public class DiscordSubCommand
    {
        public List<DiscordCommandParameter> Parameters { get; set; } = new List<DiscordCommandParameter>();
    }

    public class DiscordCommandParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public object MustMatchValue { get; set; }
        public bool IsOptional { get; set; }
    }

    public class DiscordHelp
    {
        public string Synopsis { get; set; }
        public string Detailed { get; set; }
        public string IconBase64 { get; set; }
        public string[] Images { get; set; }
        public long RequiresGuildPermission { get; set; }
    }

    public class ServersViewModel
    {
        public List<ServerViewModel> Servers { get; set; } = new List<ServerViewModel>();
    }

    public class ServerViewModel
    {
        public long ServerId { get; set; }
        public string IconHash { get; set; }
        public string ServerName { get; set; }
        public int UserCount { get; set; }
        public List<DiscordRoleViewModel> Roles { get; set; }
        public List<DiscordChannelViewModel> Channels { get; set; }
        public List<DiscordSettingCategory> BotSettingCategories { get; set; }
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
        public string Command { get; set; }
        public string[] ExtraArgs { get; set; }
        public DiscordSettingCategory Category { get; set; }
        public int Order { get; set; }
        public List<DiscordSettingViewModel> Settings { get; set; }
    }

    public class DiscordModuleViewModel
    {
        public ServerViewModel ParentServer { get; set; }
        public DiscordModule SelectedModule { get; set; }
        public List<DiscordSettingViewModel> Settings { get; set; }
        public List<DiscordSettingCategory> Categories { get; set; }
        public List<DiscordCommandViewModel> Commands { get; set; }
    }

    public class DiscordCommandViewModel
    {
        public ServerViewModel ParentServer { get; set; }
        public DiscordModule SelectedModule { get; set; }
        public DiscordCommand SelectedCommand { get; set; }
        public List<DiscordSettingViewModel> Settings { get; set; }
    }

    public class DiscordHelpViewModel
    {
        public List<DiscordModuleHelpViewModel> Modules { get; set; }
    }

    public class DiscordModuleHelpViewModel
    {
        public DiscordModule Module { get; set; }
        public string Name => Module.Name;
        public DiscordHelp Help => Module.Help;
        public List<DiscordCommandHelpViewModel> Commands { get; set; }
    }

    public class DiscordCommandHelpViewModel
    {
        public DiscordCommand Command { get; set; }
        public string Name => Command.Name;
        public DiscordHelp Help => Command.Help;
    }
}