namespace FRTools.Core.Data.DataModels.DiscordModels
{
    public class DiscordSetting
    {
        public int Id { get; set; }
        public virtual DiscordServer Server { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
