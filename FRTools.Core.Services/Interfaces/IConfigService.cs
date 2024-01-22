namespace FRTools.Core.Services.Interfaces
{
    public interface IConfigService
    {
        Task<string> GetConfigValue(string key, ulong guildId);
        Task<IEnumerable<(string Value, ulong GuildId)>> GetAllConfig(string key);
        Task AddOrUpdateConfig(string key, string value, ulong guildId);
        Task RemoveConfig(string v, ulong guildId);
    }
}
