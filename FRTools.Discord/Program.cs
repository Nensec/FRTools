using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using FRTools.Data;
using FRTools.Data.Messages;
using FRTools.Discord.Infrastructure;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using Unity.Resolution;

namespace FRTools.Discord
{
    class Program
    {
        private static DiscordSocketClient client;
        private static IUnityContainer _container = new UnityContainer();
        private static CommandService _commandService;
        private static readonly Dictionary<ulong, GuildHandler> _handlers = new Dictionary<ulong, GuildHandler>();
        private static readonly UnityServiceProvider _unityServiceProvider = new UnityServiceProvider(_container);
        private static IQueueClient _serviceBus;

        static async Task Main()
        {
            using (var ctx = new DataContext())
                ctx.Database.Initialize(false);

            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel =
#if DEBUG
                LogSeverity.Debug
#else
                LogSeverity.Debug
#endif

            });
            client.Log += Client_Log;            
            _container.RegisterInstance(_commandService = new CommandService());
            _container.RegisterInstance<IDiscordClient>(client);
            _container.RegisterInstance(client);
            _container.RegisterInstance<IServiceProvider>(_unityServiceProvider);
            _container.RegisterSingleton<SettingManager>();
            _container.RegisterInstance(new InteractiveService(client, new InteractiveServiceConfig { DefaultTimeout = TimeSpan.FromSeconds(15) }));
            _container.RegisterType<DataContext>();

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _unityServiceProvider);

            client.MessageReceived += Client_MessageReceived;
            client.JoinedGuild += guild => Task.Run(() => _handlers.Add(guild.Id, _container.Resolve<GuildHandler>(new ParameterOverride("guild", guild))));
            client.LeftGuild += guild => Task.Run(() => _handlers.Remove(guild.Id));
            client.GuildAvailable += guild => _handlers.TryGetValue(guild.Id, out var handler) ? handler.Available() : Task.Run(() => _handlers.Add(guild.Id, handler = _container.Resolve<GuildHandler>(new ParameterOverride("guild", guild)))).ContinueWith(x => handler.Available());
            client.GuildUnavailable += async guild => await (_handlers.TryGetValue(guild.Id, out var handler) ? handler.Unavailable() : Task.CompletedTask);
            client.RoleCreated += async role => await (_handlers.TryGetValue(role.Guild.Id, out var handler) ? handler.HandleRoleCreated(role) : Task.CompletedTask);
            client.RoleDeleted += async role => await (_handlers.TryGetValue(role.Guild.Id, out var handler) ? handler.HandleRoleDeleted(role) : Task.CompletedTask);
            client.RoleUpdated += async (roleOld, roleNew) => await (_handlers.TryGetValue(roleOld.Guild.Id, out var handler) ? handler.HandleRoleUpdate(roleOld, roleNew) : Task.CompletedTask);
            client.GuildMemberUpdated += async (userOld, userNew) => await (_handlers.TryGetValue(userOld.Guild.Id, out var handler) ? handler.HandleMemberUpdate(userOld, userNew) : Task.CompletedTask);
            client.GuildUpdated += async (guildOld, guildNew) => await (_handlers.TryGetValue(guildOld.Id, out var handler) ? handler.HandleUpdateGuild(guildOld, guildNew) : Task.CompletedTask);
            client.UserJoined += async user => await (_handlers.TryGetValue(user.Guild.Id, out var handler) ? handler.HandleUserJoin(user) : Task.CompletedTask);
            client.UserLeft += async user => await (_handlers.TryGetValue(user.Guild.Id, out var handler) ? handler.HandleUserLeave(user) : Task.CompletedTask);
            client.UserBanned += async (user, guild) => await (_handlers.TryGetValue(guild.Id, out var handler) ? handler.HandleUserBanned(user) : Task.CompletedTask);
            client.UserUnbanned += async (user, guild) => await (_handlers.TryGetValue(guild.Id, out var handler) ? handler.HandleUserUnbanned(user) : Task.CompletedTask);
            client.ChannelCreated += async channel => await (channel is SocketGuildChannel guildChannel && _handlers.TryGetValue(guildChannel.Guild.Id, out var handler) ? handler.HandleChannelCreated(guildChannel) : Task.CompletedTask);
            client.ChannelDestroyed += async channel => await (channel is SocketGuildChannel guildChannel && _handlers.TryGetValue(guildChannel.Guild.Id, out var handler) ? handler.HandleChannelRemoved(guildChannel) : Task.CompletedTask);
            client.ChannelUpdated += async (channelOld, channelNew) => await (channelOld is SocketGuildChannel guildChannelOld && _handlers.TryGetValue(guildChannelOld.Guild.Id, out var handler) ? handler.HandleChannelUpdated(guildChannelOld, channelNew as SocketGuildChannel) : Task.CompletedTask);
            client.MessageUpdated += async (messageOld, messageNew, channel) => await ((await messageOld.GetOrDownloadAsync())?.CreatedAt.AddMinutes(1) > messageNew?.EditedTimestamp ? Client_MessageReceived(messageNew) : Task.CompletedTask);
            client.ReactionAdded += async (message, channel, reaction) => await (channel is SocketGuildChannel guildChannel && _handlers.TryGetValue(guildChannel.Guild.Id, out var handler) ? handler.HandleReactionAdded(message, guildChannel, reaction) : Task.CompletedTask);
            client.ReactionRemoved += async (message, channel, reaction) => await (channel is SocketGuildChannel guildChannel && _handlers.TryGetValue(guildChannel.Guild.Id, out var handler) ? handler.HandleReactionRemoved(message, guildChannel, reaction) : Task.CompletedTask);
            client.ReactionsCleared += async (message, channel) => await (channel is SocketGuildChannel guildChannel && _handlers.TryGetValue(guildChannel.Guild.Id, out var handler) ? handler.HandleReactionsCleared(message, guildChannel) : Task.CompletedTask);
            //client.UserUpdated += Client_UserUpdated;
            //client.Disconnected += async ex => await Client_Disconnected(ex);
            //client.Connected += async () => await Client_Connected();
            client.GuildMemberUpdated += async (userOld, userNew) => await (_handlers.TryGetValue(userOld.Guild.Id, out var handler) ? handler.HandleUserUpdated(userOld, userNew) : Task.CompletedTask);
            client.UserVoiceStateUpdated += async (user, stateOld, stateNew) => await ((user is SocketGuildUser guildUser) ? _handlers.TryGetValue(guildUser.Guild.Id, out var handler) ? handler.HandlerUserVoiceUpdated(guildUser, stateOld, stateNew) : Task.CompletedTask : Task.CompletedTask);

            await client.LoginAsync(TokenType.Bot, ConfigurationManager.AppSettings["DiscordToken"]);
            await client.StartAsync();

            _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
            _serviceBus.RegisterMessageHandler(ServiceBusMessageHandler, new MessageHandlerOptions(ServiceBusExceptionHandler) { AutoComplete = false, MaxConcurrentCalls = 1 });

            await Task.Delay(-1);
        }

        private async static Task ServiceBusExceptionHandler(ExceptionReceivedEventArgs ex)
        {
            Console.WriteLine(ex.Exception.ToString());
        }

        private async static Task ServiceBusMessageHandler(Message msg, CancellationToken cancellationToken)
        {
            var genericMessage = JsonConvert.DeserializeObject<GenericMessage>(Encoding.UTF8.GetString(msg.Body));
            if(genericMessage.MessageType != nameof(GenericMessage))
            {
                var customMessage = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(msg.Body), Assembly.GetAssembly(typeof(GenericMessage)).GetType(nameof(genericMessage.MessageType)));
                foreach (var handler in _handlers.Values)
                {
                    // Handle custom message
                }
            }
            else
            {
                foreach (var handler in _handlers.Values)
                {
                    if (genericMessage.Category == MessageCategory.DominanceTracker)
                        await handler.HandleDominanceUpdate(genericMessage);
                }
            }

            await _serviceBus.CompleteAsync(msg.SystemProperties.LockToken);
        }

        private static Task Client_Log(LogMessage msg)
        {
            return Task.Run(() => Console.WriteLine($"[{msg.Severity}] {msg.Message}"));
        }

        private static async Task Client_MessageReceived(SocketMessage msg)
        {
            if (msg.Channel is IGuildChannel channel && _handlers.TryGetValue(channel.GuildId, out var handler))            
                await handler.HandleMessage(msg);            
            else
            {
                var argPos = 0;
                var prefix = "$";
#if DEBUG
                prefix = "!!";
#endif
                if ((((IUserMessage)msg).HasStringPrefix(prefix, ref argPos) && !char.IsNumber(msg.Content[argPos])) || ((IUserMessage)msg).HasMentionPrefix(client.CurrentUser, ref argPos))
                {
                    if (msg is IUserMessage && msg.Author.Id != client.CurrentUser.Id)
                    {
                        var context = new SocketCommandContext(client, msg as SocketUserMessage);
                        var command = _commandService.ExecuteAsync(context, argPos, _unityServiceProvider);
                        var result = await command;

                        if (!result.IsSuccess)
                        {
                            if (result.Error == CommandError.UnknownCommand)
                            {
                                await msg.Channel.SendMessageAsync($"Unknown command")
                                    .ContinueWith(x => x.Result.DelayedDelete(TimeSpan.FromSeconds(10)));
                            }
                            else
                            {
                                await msg.Channel.SendMessageAsync($"```{result.ErrorReason}```");
                            }
                        }
                    }
                }
            }
        }
    }
}