using System;

namespace FRTools.Discord.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DiscordSettingCategoryAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; }

        public DiscordSettingCategoryAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
