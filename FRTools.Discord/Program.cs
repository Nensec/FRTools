using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace FRTools.Discord
{
    class Program
    {
        private static DiscordSocketClient client;
        private static IUnityContainer _container = new UnityContainer();
        private CommandService _commandService;
        private readonly Dictionary<ulong, GuildHandler> _handlers = new Dictionary<ulong, GuildHandler>();


        static void Main()
        {
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
            new Program().Start(client).GetAwaiter().GetResult();
        }

        private static Task Client_Log(LogMessage msg)
        {
            return Task.Run(() => Console.WriteLine($"[{msg.Severity}] {msg.Message}"));
        }

        public async Task Start(DiscordSocketClient client)
        {
            _container.RegisterFactory<CommandService>(GetCommandService);

            _commandService = _container.Resolve<CommandService>();
        }

        private CommandService GetCommandService(IUnityContainer container)
        {
            var commandService = new CommandService();

            return commandService;
        }
    }
}