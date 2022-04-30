using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using FRTools.Data;
using FRTools.Data.Messages;
using FRTools.Discord.Handlers;
using FRTools.Discord.Infrastructure;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
        private static DiscordSocketClient _client;
        private static readonly IUnityContainer _container = new UnityContainer();
        private static CommandService _commandService;
        private static readonly Dictionary<ulong, GuildHandler> _handlers = new Dictionary<ulong, GuildHandler>();
        private static readonly UnityServiceProvider _unityServiceProvider = new UnityServiceProvider(_container);
        private static IQueueClient _serviceBus;
        private static ILogger _logger;
#if DEBUG
        private static readonly bool _isDebug = true;
#else
        private static readonly bool _isDebug = false;
#endif

        static async Task Main()
        {
            _logger = new ApplicationInsightsLogger($"Discord{(_isDebug ? " (DEBUG)" : "")}", new TelemetryClient(new TelemetryConfiguration(ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"])), new ApplicationInsightsLoggerOptions { TrackExceptionsAsExceptionTelemetry = true });

            using (var ctx = new DataContext())
                ctx.Database.Initialize(false);

            _client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = _isDebug ? LogSeverity.Debug : LogSeverity.Info });
            _client.Log += Client_Log;
            _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);

            _container.RegisterInstance(_commandService = new CommandService());
            _container.RegisterInstance<IDiscordClient>(_client);
            _container.RegisterInstance(_client);
            _container.RegisterInstance<IServiceProvider>(_unityServiceProvider);
            _container.RegisterSingleton<SettingManager>();
            _container.RegisterInstance(new InteractiveService(_client, new InteractiveServiceConfig { DefaultTimeout = TimeSpan.FromSeconds(15) }));
            _container.RegisterInstance(_serviceBus);
            _container.RegisterType<DataContext>();
            _container.RegisterInstance(_logger);

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _unityServiceProvider);

            _client.MessageReceived += Client_MessageReceived;
            _client.JoinedGuild += async guild => _handlers.Add(guild.Id, await CreateHandler(guild));
            _client.LeftGuild += async guild => await (_handlers.TryGetValue(guild.Id, out var handler) ? Task.Run(() => _handlers.Remove(guild.Id)) : Task.CompletedTask).ConfigureAwait(false);
            _client.GuildAvailable += guild => _handlers.TryGetValue(guild.Id, out var handler) ? handler.Available() : Task.Run(() => _handlers.Add(guild.Id, handler = _container.Resolve<GuildHandler>(new ParameterOverride("guild", guild)))).ContinueWith(x => handler.Available());
            _client.GuildUnavailable += async guild => await (_handlers.TryGetValue(guild.Id, out var handler) ? handler.Unavailable() : Task.CompletedTask).ConfigureAwait(false);
            _client.RoleCreated += async role => await (_handlers.TryGetValue(role.Guild.Id, out var handler) ? handler.HandleRoleCreated(role) : Task.CompletedTask).ConfigureAwait(false);
            _client.RoleDeleted += async role => await (_handlers.TryGetValue(role.Guild.Id, out var handler) ? handler.HandleRoleDeleted(role) : Task.CompletedTask).ConfigureAwait(false);
            _client.RoleUpdated += async (roleOld, roleNew) => await (_handlers.TryGetValue(roleOld.Guild.Id, out var handler) ? handler.HandleRoleUpdate(roleOld, roleNew) : Task.CompletedTask).ConfigureAwait(false);
            _client.GuildMemberUpdated += async (userOld, userNew) => await (_handlers.TryGetValue(userOld.Guild.Id, out var handler) ? handler.HandleMemberUpdate(userOld, userNew) : Task.CompletedTask).ConfigureAwait(false);
            _client.GuildUpdated += async (guildOld, guildNew) => await (_handlers.TryGetValue(guildOld.Id, out var handler) ? handler.HandleUpdateGuild(guildOld, guildNew) : Task.CompletedTask).ConfigureAwait(false);
            _client.UserJoined += async user => await (_handlers.TryGetValue(user.Guild.Id, out var handler) ? handler.HandleUserJoin(user) : Task.CompletedTask).ConfigureAwait(false);
            _client.UserLeft += async user => await (_handlers.TryGetValue(user.Guild.Id, out var handler) ? handler.HandleUserLeave(user) : Task.CompletedTask).ConfigureAwait(false);
            _client.UserBanned += async (user, guild) => await (_handlers.TryGetValue(guild.Id, out var handler) ? handler.HandleUserBanned(user) : Task.CompletedTask).ConfigureAwait(false);
            _client.UserUnbanned += async (user, guild) => await (_handlers.TryGetValue(guild.Id, out var handler) ? handler.HandleUserUnbanned(user) : Task.CompletedTask).ConfigureAwait(false);
            _client.ChannelCreated += async channel => await (channel is SocketGuildChannel guildChannel && _handlers.TryGetValue(guildChannel.Guild.Id, out var handler) ? handler.HandleChannelCreated(guildChannel) : Task.CompletedTask).ConfigureAwait(false);
            _client.ChannelDestroyed += async channel => await (channel is SocketGuildChannel guildChannel && _handlers.TryGetValue(guildChannel.Guild.Id, out var handler) ? handler.HandleChannelRemoved(guildChannel) : Task.CompletedTask).ConfigureAwait(false);
            _client.ChannelUpdated += async (channelOld, channelNew) => await (channelOld is SocketGuildChannel guildChannelOld && _handlers.TryGetValue(guildChannelOld.Guild.Id, out var handler) ? handler.HandleChannelUpdated(guildChannelOld, channelNew as SocketGuildChannel) : Task.CompletedTask).ConfigureAwait(false);
            _client.MessageUpdated += async (messageOld, messageNew, channel) => await ((await messageOld.GetOrDownloadAsync())?.CreatedAt.AddMinutes(1) > messageNew?.EditedTimestamp ? Client_MessageReceived(messageNew) : Task.CompletedTask).ConfigureAwait(false);
            _client.ReactionAdded += async (message, channel, reaction) => await (channel is SocketGuildChannel guildChannel && _handlers.TryGetValue(guildChannel.Guild.Id, out var handler) ? handler.HandleReactionAdded(message, guildChannel, reaction) : Task.CompletedTask).ConfigureAwait(false);
            _client.ReactionRemoved += async (message, channel, reaction) => await (channel is SocketGuildChannel guildChannel && _handlers.TryGetValue(guildChannel.Guild.Id, out var handler) ? handler.HandleReactionRemoved(message, guildChannel, reaction) : Task.CompletedTask).ConfigureAwait(false);
            _client.ReactionsCleared += async (message, channel) => await (channel is SocketGuildChannel guildChannel && _handlers.TryGetValue(guildChannel.Guild.Id, out var handler) ? handler.HandleReactionsCleared(message, guildChannel) : Task.CompletedTask).ConfigureAwait(false);
            _client.UserVoiceStateUpdated += async (user, stateOld, stateNew) => await ((user is SocketGuildUser guildUser) ? _handlers.TryGetValue(guildUser.Guild.Id, out var handler) ? handler.HandlerUserVoiceUpdated(guildUser, stateOld, stateNew) : Task.CompletedTask : Task.CompletedTask).ConfigureAwait(false);
            _client.Ready += () => Task.Run(() => _serviceBus.RegisterMessageHandler(ServiceBusMessageHandler, new MessageHandlerOptions(ServiceBusExceptionHandler) { AutoComplete = false, MaxConcurrentCalls = 1 }));

            await _client.LoginAsync(TokenType.Bot, ConfigurationManager.AppSettings["DiscordToken"]);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static async Task<GuildHandler> CreateHandler(SocketGuild guild)
        {
            var handler = _container.Resolve<GuildHandler>(new ParameterOverride("guild", guild));
            await handler.Available();
            return handler;
        }


        private static Task ServiceBusExceptionHandler(ExceptionReceivedEventArgs ex) => Task.Run(() => _logger.LogError(ex.Exception, ex.Exception.Message));

        private static async Task ServiceBusMessageHandler(Message msg, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Information, $"SB message received: {msg.MessageId}");
            var message = JsonConvert.DeserializeObject<GenericMessage>(Encoding.UTF8.GetString(msg.Body));
            if (message.MessageType != nameof(GenericMessage))
                message = (GenericMessage)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(msg.Body), Assembly.GetAssembly(typeof(GenericMessage)).DefinedTypes.FirstOrDefault(x => x.Name == message.MessageType));

            using (var ctx = new DataContext())
            {
                try
                {
                    foreach (var handler in _handlers.Values.ToList())
                    {
                        try
                        {
                            if (message is NewItemMessage newItemMessage)
                                await handler.HandleNewItemUpdate(newItemMessage, ctx);
                            if (message is FlashSaleMessage flashSaleMessage)
                                await handler.HandleFlashSaleUpdate(flashSaleMessage, ctx);
                            else
                            {
                                if (message.Source == MessageCategory.DominanceTracker)
                                    await handler.HandleDominanceUpdate(message);
                                if (message.Source == MessageCategory.SettingUpdated && (long)handler.Guild.Id == message.DiscordServer)
                                    await handler.HandleSettingUpdate(message);
                                if (message.Source == MessageCategory.DiscordTestMessage && (long)handler.Guild.Id == message.DiscordServer)
                                    await handler.HandleTestMessage(message);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to handle {message.MessageType} for server {handler.Guild.Id} event");
                        }
                    }
                }
                finally
                {
                    await ctx.SaveChangesAsync();
                }
            }

            await _serviceBus.CompleteAsync(msg.SystemProperties.LockToken);
        }

        private static Task Client_Log(LogMessage msg)
        {
            var logger = _container.Resolve<ILogger>();
            if (msg.Exception != null)
                logger.LogError(msg.Exception, msg.Message);
            else
                logger.Log(msg.Severity.ToLogLevel(), msg.Message);
            Console.WriteLine($"[{msg.Severity}] {msg.Message}");
            return Task.CompletedTask;
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
                if ((((IUserMessage)msg).HasStringPrefix(prefix, ref argPos) && !char.IsNumber(msg.Content[argPos])) || ((IUserMessage)msg).HasMentionPrefix(_client.CurrentUser, ref argPos))
                {
                    if (msg is IUserMessage && msg.Author.Id != _client.CurrentUser.Id)
                    {
                        var context = new SocketCommandContext(_client, msg as SocketUserMessage);
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