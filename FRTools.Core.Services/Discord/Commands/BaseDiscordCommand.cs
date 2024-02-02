using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
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

        public virtual string[]? ComponentInteractionCommandNames { get; }

        public abstract AppCommand Command { get; }

        public virtual Task<DiscordInteractionResponse> Execute(DiscordInteractionRequest interaction) => Task.FromResult<DiscordInteractionResponse>(new DiscordInteractionResponse.DefferedContentResponse());

        public abstract Task DeferedExecute(DiscordInteractionRequest interaction);
    }
}
