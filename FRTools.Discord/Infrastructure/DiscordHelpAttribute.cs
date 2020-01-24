using Discord;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace FRTools.Discord.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class DiscordHelpAttribute : Attribute
    {
        static List<string> _keys = new List<string>();
        static DiscordHelpAttribute()
        {
            string resxFile = Path.GetDirectoryName(Assembly.GetAssembly(typeof(DiscordHelpAttribute)).Location) + @"\Resources.resx";

            using (var resxReader = new ResXResourceReader(resxFile))
                _keys = resxReader.Cast<DictionaryEntry>().Select(x => x.Key.ToString()).ToList();
        }

        public DiscordHelpAttribute(string baseResourceKey)
        {
            Synopsis = Resources.ResourceManager.GetString(baseResourceKey + "Synopsis");
            Detailed = Resources.ResourceManager.GetString(baseResourceKey + "Detailed");
            IconBase64 = Resources.ResourceManager.GetString(baseResourceKey + "Icon");
            Images = _keys.Where(x => x.StartsWith(baseResourceKey + "_")).Select(x => Resources.ResourceManager.GetString(x)).ToArray();
        }

        public DiscordHelpAttribute(string baseResourceKey, GuildPermission requiresPermission) : this(baseResourceKey)
        {
            RequiresPermission = requiresPermission;
        }

        public string Synopsis { get; }
        public string Detailed { get; }
        public string IconBase64 { get; }
        public GuildPermission? RequiresPermission { get; }
        public string[] Images { get; }
    }
}
