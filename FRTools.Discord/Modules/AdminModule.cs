using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FRTools.Common;
using FRTools.Data;
using FRTools.Data.Messages;
using FRTools.Discord.Handlers;
using FRTools.Discord.Infrastructure;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Discord.Modules
{
    [RequireOwner]
    [Name("Admin")]
    [NoLog]
    [Group("admin")]
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
        public async Task SyncServer(ulong? serverId = null)
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

        [Name("Servers"), Command("servers")]
        public async Task Servers(ulong? serverId = null)
        {
            var guilds = Context.Client.Guilds;
            var sb = new StringBuilder();
            sb.AppendLine("I an im the following servers:");
            sb.AppendLine("```");
            foreach (var guild in guilds.Select((x, i) => (x, i)))
                sb.AppendLine($"{guild.i + 1} - ({guild.x.Id}) {guild.x.Name} ");
            sb.AppendLine("```");
            await ReplyAsync(sb.ToString());
        }

        [Name("Bot permissions"), Command("perms")]
        public async Task BotPermissions(ulong? serverId = null)
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
                await ReplyAsync($"In server `{guild.Name}` I have these permissions: {string.Join(", ", guild.CurrentUser.GuildPermissions.ToList().Select(x => $"`{x}`"))}");
            }
        }

        [Name("Fetch item"), Command("fetchitem")]
        public async Task FetchItem(int frId)
        {
            if (DbContext.FRItems.FirstOrDefault(x => x.FRId == frId) != null)
            {
                await ReplyAsync("Item already in database");
                return;
            }

            await ReplyAsync($"Fetching item: {frId}");

            var item = FRHelpers.FetchItem(frId);
            if (item != null)
            {
                await ReplyAsync($"Found item, adding: {item.Name}");
                DbContext.FRItems.Add(item);
                await DbContext.SaveChangesAsync();
                var _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
                await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NewItemMessage(MessageCategory.ItemFetcher, item)))));
                await _serviceBus.CloseAsync();
            }
            else
                await ReplyAsync("Item not found");
        }

        [Name("Update item"), Command("updateitem")]
        public async Task UpdateItem(int frId)
        {
            var existingItem = DbContext.FRItems.FirstOrDefault(x => x.FRId == frId);
            if (existingItem != null)
                await ReplyAsync($"Updating item: {frId}");
            else
                await ReplyAsync($"Item not in database, fetching item: {frId}");

            var item = FRHelpers.FetchItem(frId);
            if (item != null)
            {
                await ReplyAsync($"Found item, {(existingItem != null ? "updating" : "adding")}: {item.Name}");
                if(existingItem != null)
                    DbContext.FRItems.Remove(existingItem);
                DbContext.FRItems.Add(item);
                await DbContext.SaveChangesAsync();
                var _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
                await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NewItemMessage(MessageCategory.ItemFetcher, item)))));
                await _serviceBus.CloseAsync();
            }
            else
                await ReplyAsync("Item not found");
        }

        [Name("Update item asseturl"), Command("updateitemass")]
        public async Task UpdateItemAssetUrl(int frId, string assetUrl)
        {
            var item = DbContext.FRItems.FirstOrDefault(x => x.FRId == frId);
            if (item == null)
            {
                await ReplyAsync("Item does not exist");
                return;
            }

            item.AssetUrl = assetUrl;
            DbContext.SaveChanges();
            await ReplyAsync("Updated item");
        }
    }
}
