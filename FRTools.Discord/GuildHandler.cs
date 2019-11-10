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
    public class GuildHandler
    {
        private readonly SocketGuild _guild;
        private readonly IUnityContainer _container;
        private readonly IDiscordClient _client;
        private readonly CommandService _globalCommandService;

        public GuildHandler(SocketGuild guild, IUnityContainer container, IDiscordClient client, CommandService globalCommandService)
        {
            _guild = guild;
            _container = container;
            _client = client;
            _globalCommandService = globalCommandService;
        }
    }
}
