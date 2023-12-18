using FRTools.Core.Services.DiscordModels;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands
{
    public class HelpCommand : DiscordCommand
    {
        public HelpCommand(ILogger<HelpCommand> logger) : base(logger)
        {
        }

        public override AppCommand Command => new AppCommand
        {
            name = "help",
            description = "Contains commands to help you executing commands",
            options = new[]
            {
                new AppCommandOption
                {
                    type = AppCommandOptionType.STRING,
                    name = "webhook",
                    description = "Provides help on how to create a webhook"
                }.RegisterExecute(this, x =>
                {
                    
                })
            }
        };
    }
}
