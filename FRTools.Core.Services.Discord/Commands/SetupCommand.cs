using FRTools.Core.Data;
using FRTools.Core.Services.DiscordModels;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands
{
    public class SetupCommand : DiscordCommand
    {
        public SetupCommand(ILogger<SetupCommand> logger) : base(logger)
        {
        }

        public override AppCommand Command => new AppCommand
        {
            name = "setup",
            description = "Contains commands to setup your FRTools experience",
            options = new[]
            {
                new AppCommandOption
                {
                    type = AppCommandOptionType.SUB_COMMAND_GROUP,
                    name = "items",
                    options = new[]
                    {
                        new AppCommandOption
                        {
                            type = AppCommandOptionType.SUB_COMMAND,
                            name = "channel",
                            options = new[]
                            {
                                //new AppCommandOption
                                //{
                                //    type = AppCommandOptionType.CHANNEL,
                                //    name = "channel",
                                //    description = "The channel that will show new items"
                                //},
                                new AppCommandOption
                                {
                                    type = AppCommandOptionType.STRING,
                                    name = "webhook",
                                    description = "The webhook that will post new items, if you do not know how to make a webhook run /help webhook"
                                }
                            }
                        },
                        new AppCommandOption
                        {
                            type = AppCommandOptionType.STRING,
                            name = "types",
                            description = "The item types that will be announced"
                        }
                    }
                },
                new AppCommandOption
                {
                    type = AppCommandOptionType.SUB_COMMAND_GROUP,
                    name = "dominance",
                    options = new[]
                    {
                        new AppCommandOption
                        {
                            type = AppCommandOptionType.SUB_COMMAND,
                            name = "channel",
                            options = new[]
                            {
                                //new AppCommandOption
                                //{
                                //    type = AppCommandOptionType.CHANNEL,
                                //    name = "channel",
                                //    description = "The channel that will show dominance results"
                                //},
                                new AppCommandOption
                                {
                                    type = AppCommandOptionType.STRING,
                                    name = "channel",
                                    description = "The webhook that will post dominance results, if you do not know how to make a webhook run /help webhook"
                                }
                            }.Concat(Enum.GetValues<Flight>().Select(x =>
                            new AppCommandOption
                            {
                                type = AppCommandOptionType.ROLE,
                                name = x.ToString().ToLower(),
                                description = $"The role used to ping {x} members"
                            }
                            )).ToArray()
                        }
                    }
                },
                new AppCommandOption
                {
                    type = AppCommandOptionType.SUB_COMMAND_GROUP,
                    name = "flash_sale",
                    options = new[]
                    {
                        new AppCommandOption
                        {
                            type = AppCommandOptionType.SUB_COMMAND,
                            options = new[]
                            {
                                //new AppCommandOption
                                //{
                                //    type = AppCommandOptionType.CHANNEL,
                                //    name = "channel",
                                //    description = "The channel that will show new flash sales"
                                //},
                                new AppCommandOption
                                {
                                    type = AppCommandOptionType.STRING,
                                    name = "channel",
                                    description = "The webhook that will post new flash sales, if you do not know how to make a webhook run /help webhook"
                                }
                            }
                        }
                    }
                }
            }
        };
    }
}
