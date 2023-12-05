namespace FRTools.Core.Data.DataModels.DiscordModels.RestModels
{
    public class DiscordEmoji
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public object[] Roles { get; set; }
        public bool Require_colons { get; set; }
        public bool Managed { get; set; }
        public bool Animated { get; set; }
        public bool Available { get; set; }
    }

}
