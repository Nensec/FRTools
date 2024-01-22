using System.ComponentModel;
using System.Reflection;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands
{
    public class ConfigCommand : BaseDiscordCommand
    {
        private readonly IConfigService _configService;

        private readonly string[] _announcers;

        public ConfigCommand(IConfigService configService, IDiscordService discordService, ILogger<ConfigCommand> logger) : base(discordService, logger)
        {
            _configService = configService;
            _announcers = typeof(AnnounceData).Assembly.GetTypes().Where(x => typeof(AnnounceData).IsAssignableFrom(x) && !x.IsAbstract).Select(x => x.GetCustomAttribute<DescriptionAttribute>()!.Description).ToArray();
        }

        public override AppCommand Command => new AppCommand
        {
            Name = "config",
            Description = "Configure FRTools to your liking!",
            DefaultMemberPermissions = Permissions.ADMINISTRATOR,
            DmPermission = false,
            Type = AppCommandType.CHAT_INPUT,
            Options = new[]
            {
                new AppCommandOption
                {
                    Name = "announce",
                    Description = "Configure the webhook(s) to announce messages to",
                    Type = AppCommandOptionType.SUB_COMMAND_GROUP,
                    Options = new[]
                    {
                        new AppCommandOption
                        {
                            Name = "default_channel",
                            Description = "The default announcement channel",
                            Type = AppCommandOptionType.SUB_COMMAND,
                            Options = new[]
                            {
                                new AppCommandOption
                                {
                                    Name = "channel",
                                    Description = "The default channel to announce to, requires bot to have MANAGE_WEBHOOKS",
                                    Type = AppCommandOptionType.CHANNEL,
                                    ChannelTypes = new[]{ ChannelType.GUILD_TEXT },
                                    Required = true
                                }
                            }
                        },
                        new AppCommandOption
                        {
                            Name = "default_webhook",
                            Description = "The default announcement webhook",
                            Type = AppCommandOptionType.SUB_COMMAND,
                            Options = new[]
                            {
                                new AppCommandOption
                                {
                                    Name = "webhook",
                                    Description = "The default webhook to announce to",
                                    Type = AppCommandOptionType.STRING,
                                }
                            }
                        },
                        new AppCommandOption
                        {
                            Name = "remove_default",
                            Description = "Removes the default announcement webhook",
                            Type = AppCommandOptionType.SUB_COMMAND
                        }
                    }.Concat(_announcers.SelectMany(x =>
                        new[]
                        {
                            new AppCommandOption
                            {
                                Name = $"{x.Replace(' ', '_')}_channel",
                                Description = $"Override's the default channel for {x}",
                                Type = AppCommandOptionType.SUB_COMMAND,
                                Options = new[]
                                {
                                    new AppCommandOption
                                    {
                                        Name = "channel",
                                        Description = $"The channel to announce {x} to, requires bot to have MANAGE_WEBHOOKS",
                                        Type = AppCommandOptionType.CHANNEL,
                                        ChannelTypes = new[]{ ChannelType.GUILD_TEXT },
                                        Required = true
                                    }
                                }
                            },
                            new AppCommandOption
                            {
                                Name = $"{x.Replace(' ', '_')}_webhook",
                                Description = $"Override's the default webhook for {x}",
                                Type = AppCommandOptionType.SUB_COMMAND,
                                Options = new[]
                                {
                                    new AppCommandOption
                                    {
                                        Name = "webhook",
                                        Description = $"The webhook to announce {x} to",
                                        Type = AppCommandOptionType.STRING,
                                    }
                                }
                            },
                            new AppCommandOption
                            {
                                Name = $"remove_{x.Replace(' ', '_')}",
                                Description = $"Removes the {x} announcement webhook",
                                Type = AppCommandOptionType.SUB_COMMAND
                            }
                        }
                    ))
                }
            }
        };

        public override Task DeferedExecute(DiscordInteractionRequest interaction)
        {
            var commandGroup = interaction.Data.Options.First();

            switch (commandGroup.Name)
            {
                case "announce":
                    return HandleAnnounceConfig(commandGroup, interaction);
                default:
                    return Task.CompletedTask;
            }
        }

        private async Task HandleAnnounceConfig(DiscordInteractionRequestOptionData commandData, DiscordInteractionRequest interaction)
        {
            var command = commandData.Options.First();

            if (command.Options.Any()) // setter
            {
                string webhookUrl;
                if (command.Options.First().Type == AppCommandOptionType.CHANNEL)
                {
                    if (!interaction.AppPermissions.HasFlag(Permissions.MANAGE_WEBHOOKS))
                    {
                        try
                        {
                            await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                            {
                                Content = $"I do not have the required `MANAGE_WEBHOOKS` permission on this server to do that, adjust my permissions or manually create a webhook and add it using `/config announce {command.Name.Replace("channel", "webhook")}` instead.",
                                Flags = MessageFlags.EPHEMERAL
                            });
                        }
                        catch { }
                        return;
                    }

                    webhookUrl = await DiscordService.CreateWebhook($"FRTools_{ParseCommandName(command.Name)}", ulong.Parse((string)command.Options.First().Value));
                }
                else
                {
                    webhookUrl = (string)command.Options.First().Value;
                }

                await _configService.AddOrUpdateConfig($"GUILDCONFIG_{ParseCommandName(command.Name).ToUpper()}WEBHOOK", webhookUrl, interaction.GuildId);
                await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"Configured the webhook for `{ParseCommandName(command.Name)}` as `{webhookUrl}`.",
                    Flags = MessageFlags.EPHEMERAL
                });
            }
            else // getter
            {
                try
                {
                    var configValue = await _configService.GetConfigValue($"GUILDCONFIG_{ParseCommandName(command.Name).ToUpper()}WEBHOOK", interaction.GuildId);
                    await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = $"The webhook currently configured for `{ParseCommandName(command.Name)}` is `{configValue}`.",
                        Flags = MessageFlags.EPHEMERAL
                    });
                }
                catch
                {
                    await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = $"No setting stored yet.",
                        Flags = MessageFlags.EPHEMERAL
                    });
                }
            }
        }

        private string ParseCommandName(string commandName)
        {
            return commandName.Replace("_", "").Replace("channel", "").Replace("webhook", "");
        }
    }
}
