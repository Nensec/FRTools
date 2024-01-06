using FRTools.Core.Services.Discord.Commands;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IDiscordRequestService
    {
        Task<DiscordInteractionResponse> ExecuteInteraction(DiscordInteractionRequest interaction);
        void RegisterCommand(DiscordCommand command);
        Task RegisterAllCommands();
        Task ExecuteDeferedInteraction(DiscordInteractionRequest interaction);
    }
}