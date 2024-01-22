using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands
{
    public abstract class BaseDiscordCommand
    {
        protected IDiscordService DiscordService { get; }

        protected ILogger Logger { get; }
        protected Dictionary<string, Action> CommandActions = new Dictionary<string, Action>();

        protected BaseDiscordCommand(IDiscordService discordService, ILogger logger)
        {
            DiscordService = discordService;
            Logger = logger;
        }

        public string CommandName { get => Command.Name; }

        public abstract AppCommand Command { get; }

        public abstract Task DeferedExecute(DiscordInteractionRequest interaction);
    }
}
