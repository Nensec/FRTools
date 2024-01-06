using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using FRTools.Core.Services.DiscordModels;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands
{
    public class FetchDominanceCommand : DiscordCommand
    {
        public FetchDominanceCommand(ILogger<FetchDominanceCommand> logger) : base(logger)
        {
        }

        public override AppCommand Command { get; } = new AppCommand
        {
            Name = "dragon",
            Type = AppCommandType.CHAT_INPUT,
            Description = "Looks up a dragon's information",
        };

        public override Task<DiscordInteractionResponse> Execute(DiscordInteractionRequest interaction)
        {
            throw new NotImplementedException();
        }
    }
}
