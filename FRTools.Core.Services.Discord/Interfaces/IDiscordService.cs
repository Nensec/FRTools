using FRTools.Core.Services.DiscordModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IDiscordService
    {
        Task ExecuteInteraction(AppCommand command);
        Task RegisterCommand(AppCommand command);
        Task RegisterAllCommands();
    }
}