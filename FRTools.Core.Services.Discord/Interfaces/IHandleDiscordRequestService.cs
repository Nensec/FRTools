using FRTools.Core.Services.Discord.Commands;
using FRTools.Core.Services.Discord.DiscordModels.RequestModels;
using FRTools.Core.Services.Discord.DiscordModels.ResponseModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IHandleDiscordRequestService
    {
        Task<DiscordInteractionResponse> ExecuteInteraction(DiscordInteractionRequest interaction);
        void RegisterCommand(DiscordCommand command);
        Task RegisterAllCommands();
        Task ExecuteDeferedInteraction(DiscordInteractionRequest interaction);
    }
}