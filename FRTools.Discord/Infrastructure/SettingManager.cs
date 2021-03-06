﻿using Discord;
using FRTools.Common;
using FRTools.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordSetting = FRTools.Data.DataModels.DiscordModels.DiscordSetting;

namespace FRTools.Discord.Infrastructure
{
    public class SettingManager
    {
        private Dictionary<(IGuild, string), string> _settingCache = new Dictionary<(IGuild, string), string>();

        private DiscordMetadata DiscordMetadata { get; }

        public SettingManager()
        {
            var json = System.IO.File.ReadAllText("DiscordMetadata.json");
            DiscordMetadata = JsonConvert.DeserializeObject<DiscordMetadata>(json);
        }

        public async Task<string> GetSettingValue(string key, IGuild guild = null)
        {
            if (!_settingCache.TryGetValue((guild, key), out var val))
            {
                if (guild != null && !_settingCache.TryGetValue((null, key), out val))
                {
                    using (var ctx = new DataContext())
                    {
                        var dbSetting = ctx.DiscordSettings.FirstOrDefault(x => x.Server.ServerId == (long)guild.Id && x.Key == key);
                        if (dbSetting != null)
                            val = _settingCache[(guild, key)] = dbSetting.Value;
                    }
                }

                if (val == null)
                {
                    using (var ctx = new DataContext())
                    {
                        var dbSetting = ctx.DiscordSettings.FirstOrDefault(x => x.Server == null && x.Key == key);
                        if (dbSetting != null)
                            val = _settingCache[(null, key)] = dbSetting.Value;
                    }
                }

                if (val == null)
                {
                    var setting = DiscordMetadata.AllSettings.FirstOrDefault(x => x.Key == key);
                    if (setting != null)
                    {
                        var type = Type.GetType(setting.Type);
                        if (type.IsArray)
                        {
                            var arrayType = type.GetElementType();

                            if (setting.DefaultValue == "ALL" && guild != null)
                            {
                                if (arrayType.IsEnum)                                
                                    val = string.Join(",", Enum.GetValues(arrayType));                                
                                else
                                {
                                    var channels = (await guild.GetChannelsAsync()).ToList();
                                    switch (arrayType.Name)
                                    {
                                        case "ITextChannel":
                                            channels = channels.Where(x => x is ITextChannel).ToList();
                                            goto case "IChannel";
                                        case "IVoiceChannel":
                                            channels = channels.Where(x => x is IVoiceChannel).ToList();
                                            goto case "IChannel";
                                        case "ICategoryChannel":
                                            channels = channels.Where(x => x is ICategoryChannel).ToList();
                                            goto case "IChannel";
                                        case "IChannel":
                                            val = string.Join(",", channels.Select(x => x.Id.ToString()));
                                            break;                                            
                                    }
                                }
                            }
                        }
                        else
                            val = setting.DefaultValue;
                    }
                }
            }

            return val;
        }

        public string SetSettingValue(string key, string value, IGuild guild = null)
        {
            _settingCache[(guild, key)] = value;
            using (var ctx = new DataContext())
            {
                var dbSetting = guild != null ? ctx.DiscordSettings.FirstOrDefault(x => x.Server.ServerId == (long)guild.Id && x.Key == key) : ctx.DiscordSettings.FirstOrDefault(x => x.Key == key);
                if (dbSetting == null)
                    dbSetting = ctx.DiscordSettings.Add(new DiscordSetting { Server = guild != null ? ctx.DiscordServers.FirstOrDefault(x => x.ServerId == (long)guild.Id) : null, Key = key });
                dbSetting.Value = value;
                ctx.SaveChanges();
            }

            return value;
        }

        public void GetAllGuildSettings(IGuild guild)
        {
            using (var ctx = new DataContext())
            {
                var settings = ctx.DiscordSettings.Where(x => x.Server != null && x.Server.ServerId == (long)guild.Id).ToList();
                foreach (var setting in settings)
                    _settingCache[(guild, setting.Key)] = setting.Value;
            }
        }

        internal async Task ForceUpdate(IGuild guild, string key)
        {
            _settingCache.Remove((guild, key));
            await GetSettingValue(key, guild);
        }
    }
}
