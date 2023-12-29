using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands
{
    public abstract class DiscordCommand
    {
        protected ILogger Logger { get; }
        protected Dictionary<string, Action> CommandActions = new Dictionary<string, Action>();

        protected DiscordCommand(ILogger logger)
        {
            Logger = logger;
            Command = CreateCommand();
        }

        public AppCommand Command { get; }
        public string CommandName { get => Command.Name; }

        public abstract AppCommand CreateCommand();

        public abstract Task<DiscordInteractionResponse> Execute(DiscordInteractionRequest interaction);

        public virtual Task DeferedExecute(DiscordInteractionRequest interaction) => Task.CompletedTask;
    }
}
