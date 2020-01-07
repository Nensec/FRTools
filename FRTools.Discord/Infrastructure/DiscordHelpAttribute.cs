using Discord;
using System;

namespace FRTools.Discord.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false]
    public class DiscordHelpAttribute : Attribute
    {
        public DiscordHelpAttribute(string synopsis, string detailed, params string[] images)
        {
            Synopsis = synopsis;
            Detailed = detailed;
            Images = images;
        }

        public DiscordHelpAttribute(string synopsis, string detailed, GuildPermission requiresPermission, params string[] images)
        {
            Synopsis = synopsis;
            Detailed = detailed;
            RequiresPermission = requiresPermission;
            Images = images;
        }

        public string Synopsis { get; }
        public string Detailed { get; }
        public GuildPermission? RequiresPermission { get; }
        public string[] Images { get; }
    }
}
