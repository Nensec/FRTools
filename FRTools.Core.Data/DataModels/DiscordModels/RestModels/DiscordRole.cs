namespace FRTools.Core.Data.DataModels.DiscordModels.RestModels
{
    public class DiscordRole
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Permissions { get; set; }
        public int Position { get; set; }
        public int Color { get; set; }
        public bool Hoist { get; set; }
        public bool Managed { get; set; }
        public bool Mentionable { get; set; }
    }

}
