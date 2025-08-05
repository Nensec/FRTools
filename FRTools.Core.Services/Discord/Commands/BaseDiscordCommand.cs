using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands
{
    public abstract class BaseDiscordCommand
    {
        protected IDiscordInteractionService DiscordInteractionService { get; }

        protected ILogger Logger { get; }
        protected Dictionary<string, Action> CommandActions = new Dictionary<string, Action>();

        protected BaseDiscordCommand(IDiscordInteractionService discordInteractionService, ILogger logger)
        {
            DiscordInteractionService = discordInteractionService;
            Logger = logger;
        }

        public string CommandName { get => Command.Name; }

        public virtual string[]? ComponentInteractionCommandNames { get; }

        public abstract AppCommand Command { get; }

        public virtual Task<DiscordInteractionResponse> Execute(DiscordInteractionRequest interaction) => Task.FromResult<DiscordInteractionResponse>(new DiscordInteractionResponse.DefferedContentResponse());

        public virtual Task DeferedExecute(DiscordInteractionRequest interaction) => Task.CompletedTask;
    }
}
