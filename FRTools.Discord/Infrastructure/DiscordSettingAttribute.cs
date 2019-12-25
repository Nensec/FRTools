using System;

namespace FRTools.Discord.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class DiscordSettingAttribute : Attribute
    {
        public string Key { get; }
        public Type Type { get; }
        public string Name { get; }
        public string Description { get; }

        public DiscordSettingAttribute(string key, Type type, string name, string description)
        {
            Key = key;
            Type = type;
            Name = name;
            Description = description;
        }
    }
}
