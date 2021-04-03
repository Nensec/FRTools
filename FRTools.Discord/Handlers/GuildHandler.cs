using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FRTools.Data;
using FRTools.Data.DataModels.DiscordModels;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Data.Messages;
using FRTools.Discord.Infrastructure;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.Discord.Handlers
{
    [DiscordSettingCategory("ITEMDB", "Item database")]
    [DiscordSettingCategory("FLASHSALE", "Flash sale announce")]
    [DiscordSetting("GUILDCONFIG_PREFIX", typeof(string), "Command prefix", "The prefix used by the bot to listen to commands")]
    [DiscordSetting("GUILDCONFIG_ANN_CHANNEL", typeof(ITextChannel), "Announcement channel", "The channel the bot will post announcement messages")]
    [DiscordSetting("GUILDCONFIG_FLASHSALE_ANNNEWITEM", typeof(bool), "Announce new items", "Should the bot announce new flash sales added to the market place on Flight Rising? These announcements are posted in $<GUILD:GUILDCONFIG_ANN_CHANNEL>", Category = "FLASHSALE")]
    [DiscordSetting("GUILDCONFIG_ITEMDB_ANNNEWITEM", typeof(bool), "Announce new items", "Should the bot announce new items added to Flight Rising?", Category = "ITEMDB")]
    [DiscordSetting("GUILDCONFIG_ITEMDB_NEWITEMTYPES", typeof(FRItemCategory[]), "New item types", "Which item types should be announced", Category = "ITEMDB", Order = 2)]
    [DiscordSetting("GUILDCONFIG_ITEMDB_USEANNCHANNEL", typeof(bool), "Use announcement channel", "Use the bot's overall announcement channel, or set up an alternative", Category = "ITEMDB", Order = 2)]
    [DiscordSetting("GUILDCONFIG_ITEMDB_ANN_CHANNEL", typeof(ITextChannel), "Alternative announcement channel", "If $<GUILD:GUILDCONFIG_ITEMDB_USEANNCHANNEL> is set to false, this channel will be used to announce new items", Category = "ITEMDB", Order = 3)]
    public class GuildHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _globalCommandService;
        private readonly SettingManager _settingManager;
        private readonly IServiceProvider _serviceProvider;

        public SocketGuild Guild { get; }

        public GuildHandler(SocketGuild guild, DiscordSocketClient client, CommandService globalCommandService, SettingManager settingManager, IServiceProvider serviceProvider)
        {
            Guild = guild;
            _client = client;
            _globalCommandService = globalCommandService;
            _settingManager = settingManager;
            _serviceProvider = serviceProvider;

            settingManager.GetAllGuildSettings(guild);
        }

        internal async Task HandleRoleCreated(SocketRole role)
        {
            using (var ctx = new DataContext())
            {
                ctx.DiscordServers.SingleOrDefault(x => x.ServerId == (long)role.Guild.Id)?.Roles.Add(new DiscordRole
                {
                    RoleId = (long)role.Id,
                    Name = role.Name,
                    DiscordPermissions = (long)role.Permissions.RawValue,
                    Color = role.Color.RawValue.ToString()
                });
                await ctx.SaveChangesAsync();
            }
        }

        internal async Task HandleRoleDeleted(SocketRole role)
        {
            using (var ctx = new DataContext())
            {
                var roles = ctx.DiscordRoles.Where(x => x.RoleId == (long)role.Id);
                ctx.DiscordRoles.RemoveRange(roles);
                await ctx.SaveChangesAsync();
            }
        }

        internal async Task HandleRoleUpdate(SocketRole roleOld, SocketRole roleNew)
        {
            using (var ctx = new DataContext())
            {
                DiscordRole role = null;
                var roles = ctx.DiscordRoles.Where(x => x.RoleId == (long)roleNew.Id).ToList();
                if (roles.Count > 1)
                {
                    role = roles[0];
                    ctx.DiscordRoles.RemoveRange(roles.Skip(1));
                }
                else
                    role = roles.FirstOrDefault();

                if (role != null)
                {
                    role.Name = roleNew.Name;
                    role.Color = roleNew.Color.RawValue.ToString();
                    role.DiscordPermissions = (long)roleNew.Permissions.RawValue;
                    await ctx.SaveChangesAsync();
                }
            }
        }

        internal async Task HandleMemberUpdate(SocketGuildUser userOld, SocketGuildUser userNew)
        {
            using (var ctx = new DataContext())
            {
                var dbServer = ctx.DiscordServers.SingleOrDefault(x => x.ServerId == (long)userNew.Guild.Id);
                var dbServerUser = dbServer.Users.FirstOrDefault(x => x.User.UserId == (long)userNew.Id);
                if (dbServerUser == null)
                {
                    var dbUser = ctx.DiscordUsers.FirstOrDefault(x => x.UserId == (long)userNew.Id);
                    if (dbUser == null)
                        ctx.DiscordUsers.Add(dbUser = new DiscordUser { UserId = (long)userNew.Id });
                    dbServer.Users.Add(dbServerUser = new DiscordServerUser { User = dbUser });
                }
                dbServerUser.Nickname = userNew.Nickname;
                dbServerUser.Roles.Clear();
                dbServerUser.Roles = dbServer.Roles.Where(x => userNew.Roles.Any(r => (long)r.Id == x.RoleId)).ToList();
                dbServerUser.User.Username = userNew.Username;
                dbServerUser.User.Discriminator = userNew.DiscriminatorValue;
                await ctx.SaveChangesAsync();
            }
        }

        internal async Task HandleUpdateGuild(SocketGuild guildOld, SocketGuild guildNew)
        {
            using (var ctx = new DataContext())
            {
                var guild = ctx.DiscordServers.SingleOrDefault(x => x.ServerId == (long)guildNew.Id);
                guild.Name = guildNew.Name;
                await ctx.SaveChangesAsync();
            }
        }

        internal async Task HandleChannelCreated(SocketGuildChannel guildChannel)
        {
            using (var ctx = new DataContext())
            {
                ctx.DiscordServers.SingleOrDefault(x => x.ServerId == (long)guildChannel.Guild.Id).Channels.Add(new DiscordChannel
                {
                    ChannelId = (long)guildChannel.Id,
                    Name = guildChannel.Name
                });
                await ctx.SaveChangesAsync();
            }
        }

        internal async Task HandleChannelRemoved(SocketGuildChannel guildChannel)
        {
            using (var ctx = new DataContext())
            {
                var channels = ctx.DiscordChannels.Where(x => x.ChannelId == (long)guildChannel.Id);
                ctx.DiscordChannels.RemoveRange(channels);
                await ctx.SaveChangesAsync();
            }
        }

        internal async Task HandleChannelUpdated(SocketGuildChannel guildChannelOld, SocketGuildChannel guildChannelNew)
        {
            using (var ctx = new DataContext())
            {
                DiscordChannel channel = null;
                var channels = ctx.DiscordChannels.Where(x => x.ChannelId == (long)guildChannelNew.Id).ToList();
                if (channels.Count > 1)
                {
                    channel = channels[0];
                    ctx.DiscordChannels.RemoveRange(channels.Skip(1));
                }
                else
                    channel = channels.FirstOrDefault();

                if (channel != null)
                {
                    channel.Name = guildChannelNew.Name;
                    ctx.SaveChanges();
                }
                else
                    await HandleChannelCreated(guildChannelNew);
            }
        }

        internal async Task HandleMessage(SocketMessage msg)
        {
            int argPos = 0;
            var prefix = _settingManager.GetSettingValue("GUILDCONFIG_PREFIX", Guild) ?? "$";

#if DEBUG
            prefix = "!!!";
#endif

            if (msg is IUserMessage message && !message.Author.IsBot && message.Author.Id != _client.CurrentUser.Id)
            {
                var context = new FRToolsSocketCommandContext(_client, msg as SocketUserMessage);

                if (message.HasStringPrefix(prefix, ref argPos) && !char.IsNumber(message.Content[argPos]) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
                {
                    if (message.Content.Substring(argPos) == "help")
                    {
                        await context.Channel.SendMessageAsync(embed: new EmbedBuilder().WithDescription($"Please see the following link for help. [click here]({ConfigurationManager.AppSettings["WebsiteBaseURL"]}/discord/help)").Build());
                        return;
                    }
                    var result = await _globalCommandService.ExecuteAsync(context, argPos, _serviceProvider);
                    if (!result.IsSuccess)
                    {
                        if (result.Error == CommandError.UnknownCommand)
                        {
                            await msg.Channel.SendMessageAsync($"Unknown command")
                                .ContinueWith(x => x.Result.DelayedDelete(TimeSpan.FromSeconds(10)));
                        }
                        else
                        {
                            await msg.Channel.SendMessageAsync($"```{result.ErrorReason}```").ContinueWith(x => x.Result.DelayedDelete(TimeSpan.FromSeconds(10)));
                        }
                    }
                }
                else if (message.Content.StartsWith("https://www1.flightrising.com/"))
                {
                    // Handle FR links as commands
                    if (bool.TryParse(_settingManager.GetSettingValue("LOOKUP_AUTO_LINK", Guild), out var dragonAutoLink) && dragonAutoLink)
                    {
                        context.AutomatedCommand = true;
                        var dragonLink = Regex.Match(message.Content, @".+/dragon/(\d+)");
                        if (dragonLink.Success)
                        {
                            await _globalCommandService.ExecuteAsync(context, $"lookup dragon {dragonLink.Groups[1].Value}", _serviceProvider);
                            return;
                        }

                        var itemLink = Regex.Match(message.Content, @".+/game-database/item/(\d+)");
                        if (itemLink.Success)
                        {
                            await _globalCommandService.ExecuteAsync(context, $"lookup item {itemLink.Groups[1].Value}", _serviceProvider);
                            return;
                        }
                    }
                }
            }
        }

        internal async Task HandleUserJoin(SocketGuildUser user)
        {
            using (var ctx = new DataContext())
            {
                var dbServer = ctx.DiscordServers.SingleOrDefault(x => x.ServerId == (long)user.Guild.Id);
                var dbServerUser = dbServer.Users.FirstOrDefault(x => x.User.UserId == (long)user.Id);
                if (dbServerUser == null)
                {
                    var dbUser = ctx.DiscordUsers.FirstOrDefault(x => x.UserId == (long)user.Id);
                    if (dbUser == null)
                        ctx.DiscordUsers.Add(dbUser = new DiscordUser { UserId = (long)user.Id });
                    dbServer.Users.Add(dbServerUser = new DiscordServerUser { User = dbUser });
                }
                dbServerUser.Nickname = user.Nickname;
                dbServerUser.Roles = dbServer.Roles.Where(x => user.Roles.Any(r => (long)r.Id == x.RoleId)).ToList();
                dbServerUser.User.Username = user.Username;
                dbServerUser.User.Discriminator = user.DiscriminatorValue;
                await ctx.SaveChangesAsync();
            }
        }

        internal async Task HandleFlashSaleUpdate(FlashSaleMessage flashSaleMessage)
        {
            // Check if setting is turned on
            if (bool.TryParse(_settingManager.GetSettingValue("GUILDCONFIG_FLASHSALE_ANNNEWITEM", Guild), out var shouldAnnounce) && shouldAnnounce)
            {
                // Get the channel to announce item in
                ISocketMessageChannel annChannel = null;
                var annChannelId = _settingManager.GetSettingValue("GUILDCONFIG_ANN_CHANNEL", Guild);
                if (annChannelId != null)
                    annChannel = Guild.GetChannel(ulong.Parse(annChannelId)) as ISocketMessageChannel;

                if (annChannel != null)
                {
                    var embed = await ItemHandler.CreateItemEmbed(flashSaleMessage.Item, Guild, _settingManager, true);
                    embed.Embed.Title = "New flash sale found! - " + embed.Embed.Title;
                    embed.Embed.Color = new global::Discord.Color(243, 181, 144);
                    await annChannel.SendFilesAsync(embed.Files, embed: embed.Embed.Build());
                }
            }
        }

        internal async Task HandleUserLeave(SocketGuildUser user)
        {
            using (var ctx = new DataContext())
            {
                var dbServer = ctx.DiscordServers.SingleOrDefault(x => x.ServerId == (long)user.Guild.Id);
                var dbServerUser = dbServer.Users.FirstOrDefault(x => x.User.UserId == (long)user.Id);
                if (dbServerUser == null)
                    return;

                ctx.DiscordServerUsers.Remove(dbServerUser);
                await ctx.SaveChangesAsync();
            }
        }

        internal async Task HandleTestMessage(GenericMessage testMessage)
        {
            var channelId = _settingManager.GetSettingValue(testMessage.Message, Guild);
            if (channelId != null)
            {
                var channel = Guild.GetChannel(ulong.Parse(channelId)) as ISocketMessageChannel;
                if (channel != null)
                {
                    try
                    {
                        await channel.SendMessageAsync("This is a test message");
                        try
                        {
                            await channel.SendMessageAsync(embed: new EmbedBuilder().WithDescription("This is an embed test message").Build());
                        }
                        catch (Exception ex)
                        {
                            await channel.SendMessageAsync($"Embed test failed: {ex}");
                        }
                        try
                        {
                            using (var stream = new MemoryStream())
                            {
                                using (var writer = new StreamWriter(stream, System.Text.Encoding.UTF8, 1024, true))
                                {
                                    writer.WriteLine("This is generated test content");
                                }
                                stream.Position = 0;
                                await channel.SendFileAsync(stream, "test.txt", "This is a file test");
                            }
                        }
                        catch (Exception ex)
                        {
                            await channel.SendMessageAsync($"File attachment test failed: {ex}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Bot has no permission at all to send a message to {channel.Name} ({channel.Id}): {ex}");
                    }
                }
            }
        }

        internal async Task HandleDominanceUpdate(GenericMessage dominanceUpdate)
        {
            if (dominanceUpdate.Message == "Updated")
            {
                try
                {
                    await DominanceHandler.UpdateGuild(_settingManager, Guild, Guild.GetUser(_client.CurrentUser.Id));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Could not update dominance for server '{Guild.Id}' ({Guild.Name})");
                    Trace.WriteLine(ex.Message);
                }
            }
        }

        internal async Task HandleNewItemUpdate(NewItemMessage newItemMessage)
        {
            // Check if setting is turned on
            if (bool.TryParse(_settingManager.GetSettingValue("GUILDCONFIG_ITEMDB_ANNNEWITEM", Guild), out var shouldAnnounce) && shouldAnnounce)
            {
                // Check if item is in list of announceables
                var itemTypesToAnnounce = (_settingManager.GetSettingValue("GUILDCONFIG_ITEMDB_NEWITEMTYPES", Guild) ?? "").Split(',').Select(x => (FRItemCategory)int.Parse(x)).ToList();
                if (itemTypesToAnnounce.Contains(newItemMessage.Item.ItemCategory))
                {
                    // Get the channel to announce item in
                    ISocketMessageChannel annChannel = null;
                    if (bool.TryParse(_settingManager.GetSettingValue("GUILDCONFIG_ITEMDB_USEANNCHANNEL", Guild), out var useGeneralAnnChannel) && useGeneralAnnChannel)
                    {
                        var annChannelId = _settingManager.GetSettingValue("GUILDCONFIG_ANN_CHANNEL", Guild);
                        if (annChannelId != null)
                            annChannel = Guild.GetChannel(ulong.Parse(annChannelId)) as ISocketMessageChannel;
                    }
                    else
                    {
                        var annNewItemChannelId = _settingManager.GetSettingValue("GUILDCONFIG_ITEMDB_ANN_CHANNEL", Guild);
                        if (annNewItemChannelId != null)
                            annChannel = Guild.GetChannel(ulong.Parse(annNewItemChannelId)) as ISocketMessageChannel;
                    }

                    if (annChannel != null)
                    {
                        var embed = await ItemHandler.CreateItemEmbed(newItemMessage.Item, Guild, _settingManager);
                        embed.Embed.Title = "New item found! - " + embed.Embed.Title;
                        await annChannel.SendFilesAsync(embed.Files, embed: embed.Embed.Build());
                    }
                }
            }
        }

        internal async Task HandleSettingUpdate(GenericMessage settingUpdate)
        {
            await Task.Run(() => _settingManager.ForceUpdate(Guild, settingUpdate.Message));
        }

        private static object _syncLock = new object();
        internal Task Available()
        {
            _ = Task.Run(() =>
           {
               lock (_syncLock)
               {
                   _ = UserHandler.SyncServer(Guild);
               }
           });

            return Task.CompletedTask;
        }

        #region Unused
        internal Task Unavailable() => Task.CompletedTask;
        internal Task HandleReactionsCleared(Cacheable<IUserMessage, ulong> message, SocketGuildChannel guildChannel) => throw new NotImplementedException();
        internal Task HandleUserBanned(SocketUser user) => Task.CompletedTask;
        internal Task HandleUserUnbanned(SocketUser user) => Task.CompletedTask;
        internal Task HandleReactionAdded(Cacheable<IUserMessage, ulong> message, SocketGuildChannel guildChannel, SocketReaction reaction) => Task.CompletedTask;
        internal Task HandleReactionRemoved(Cacheable<IUserMessage, ulong> message, SocketGuildChannel guildChannel, SocketReaction reaction) => Task.CompletedTask;
        internal Task HandlerUserVoiceUpdated(SocketGuildUser guildUser, SocketVoiceState stateOld, SocketVoiceState stateNew) => Task.CompletedTask;
        #endregion
    }
}
