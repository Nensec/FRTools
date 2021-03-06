﻿using System;

namespace FRTools.Discord.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class DiscordSettingAttribute : Attribute
    {
        public string Key { get; }
        public Type Type { get; }
        public string Name { get; }
        public string Description { get; }
        public string[] ExtraArgs { get; }
        public string Category { get; set; }
        public int Order { get; set; } = 1;
        public string DefaultValue { get; set; }

        public DiscordSettingAttribute(string key, Type type, string name, string description, params string[] extraArgs)
        {
            Key = key;
            Type = type;
            Name = name;
            Description = description;
            ExtraArgs = extraArgs;
        }
    }
}
