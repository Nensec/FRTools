﻿using FRTools.Core.Data;
using FRTools.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FRTools.Core.Services
{
    public class ConfigService : IConfigService
    {
        private readonly DataContext _dataContext;

        public ConfigService(DataContext dataContext) => _dataContext = dataContext;

        public async Task AddOrUpdateConfig(string key, string value, ulong guildId)
        {
            var setting = _dataContext.DiscordSettings.FirstOrDefault(x => x.Server.ServerId == (long)guildId) ?? new Data.DataModels.DiscordModels.DiscordSetting();
            setting.Key = key;
            setting.Value = value;
            setting.Server ??= new Data.DataModels.DiscordModels.DiscordServer { ServerId = (long)guildId };
            _dataContext.DiscordSettings.Update(setting);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<(string Key, string Value, ulong GuildId)>> GetAllConfig(string key) => (await _dataContext.DiscordSettings.Where(x => x.Key == key).ToListAsync()).Select(x => (key, x.Value, (ulong)x.Server.ServerId));

        public async Task<string?> GetConfigValue(string key, ulong guildId) => (await _dataContext.DiscordSettings.FirstOrDefaultAsync(x => x.Key == key && x.Server.ServerId == (long)guildId))?.Value;

        public async Task RemoveConfig(string key, ulong guildId)
        {
            var config = await _dataContext.DiscordSettings.FirstOrDefaultAsync(x => x.Key == key && x.Server.ServerId == (long)guildId);
            if (config != null)
            {
                _dataContext.Remove(config);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
