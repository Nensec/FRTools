namespace FRTools.Data.DataModels.DiscordModels
{
    public class DiscordRole
    {
        public int Id { get; set; }
        public DiscordServer Server { get; set; }
        public long RoleId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public long DiscordPermissions { get; set; }
    }
}
