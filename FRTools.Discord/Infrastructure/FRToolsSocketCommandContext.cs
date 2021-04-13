using Discord.Commands;
using Discord.WebSocket;

namespace FRTools.Discord.Infrastructure
{
    public class FRToolsSocketCommandContext : SocketCommandContext
    {
        public bool AutomatedCommand { get; set; } = false;

        public FRToolsSocketCommandContext(DiscordSocketClient client, SocketUserMessage msg) : base(client, msg)
        {
        }
    }
}
