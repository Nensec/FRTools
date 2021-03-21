using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FRTools.Data;
using FRTools.Discord.Handlers;
using FRTools.Discord.Infrastructure;

namespace FRTools.Discord.Modules
{
    [RequireOwner]
    [Name("admin")]
    [NoLog]
    public class AdminModule : BaseModule
    {
        public AdminModule(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
        {
        }

        [Name("Set Config"), Command("setconfig")]
        public Task SetConfig(string key, [Remainder] string value)
        {
            if (Context.Channel is IDMChannel)
                return SetGlobalConfig(key, value);

            SettingManager.SetSettingValue(key, value, Context.Guild);
            return ReplyAsync($"Updated Key `{key}` with value `{value}` for guild `{Context.Guild.Name}`");
        }

        [Name("Set Global Config"), Command("setglobalconfig")]
        public Task SetGlobalConfig(string key, [Remainder] string value)
        {
            SettingManager.SetSettingValue(key, value);
            return ReplyAsync($"Updated Key `{key}` with value `{value}` for `Global`");
        }

        [Name("Sync server"), Command("syncserver")]
        public async Task SyncServer(ulong? serverId)
        {
            SocketGuild guild = null;
            if (serverId != null)
            {
                var g = Context.Client.Guilds.FirstOrDefault(x => x.Id == serverId);
                if (g == null)
                {
                    await ReplyAsync($"I am not in server `{serverId}`");
                }
                else
                    guild = g;
            }
            else
                guild = Context.Guild;
            if (guild != null)
            {
                await ReplyAsync($"Starting manual sync of server `{guild.Name}`");
                try
                {
                    await UserHandler.SyncServer(guild, Context);
                    await ReplyAsync("Sync finished");
                }
                catch (Exception ex)
                {
                    await ReplyAsync($"Error: {ex}");
                }
            }
        }
    }
}
