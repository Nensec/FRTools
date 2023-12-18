using FRTools.Core.Services.DiscordModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FRTools.Core.Services.Discord.Commands
{
    public abstract class DiscordCommand
    {
        protected Dictionary<string, Action<AppCommandOption>> Registrations { get; } = new Dictionary<string, Action<AppCommandOption>>();
        protected ILogger Logger { get; }

        protected DiscordCommand(ILogger logger)
        {
            Logger = logger;
        }

        public abstract AppCommand Command { get; }

        public Task ExecuteAsync(AppCommand command)
        {
            return Task.Run(() => Logger.LogInformation($"Command received:\n{JsonConvert.SerializeObject(command)}"));
        }

        public string CommandName { get => Command.name; }

        public void RegisterExecute(AppCommandOption command, Action<AppCommandOption> action)
        {
            Registrations.Add(command.name, action);
        }
    }

    public static class DiscordCommandExtentions
    {
        public static AppCommandOption RegisterExecute(this AppCommandOption command, DiscordCommand discordCommand, Action<AppCommandOption> action)
        {
            discordCommand.RegisterExecute(command, action);
            return command;
        }
    }
}
