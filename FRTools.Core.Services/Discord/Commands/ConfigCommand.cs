using System.ComponentModel;
using System.Reflection;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Announce.Announcers;
using FRTools.Core.Services.Discord.DiscordModels;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands
{
    public class ConfigCommand : BaseDiscordCommand
    {
        private readonly IConfigService _configService;

        private readonly string[] _discordAnnouncers;

        public ConfigCommand(IConfigService configService, IDiscordService discordService, ILogger<ConfigCommand> logger) : base(discordService, logger)
        {
            _configService = configService;
            _discordAnnouncers = typeof(AnnounceData).Assembly
                .GetTypes()
                .Where(x => typeof(AnnounceData).IsAssignableFrom(x) && !x.IsAbstract)
                .Where(x => x.BaseType!.GenericTypeArguments.First().IsAssignableFrom(typeof(DiscordWebhookAnnouncer)))
                .Select(x => x.GetCustomAttribute<DescriptionAttribute>()!.Description)
                .ToArray();
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
                ChannelsConfig,
                FilterConfig
            }
        };

        public override string[]? ComponentInteractionCommandNames => new[]
        {
            "new_item_categories"
        };

        private AppCommandOption ChannelsConfig => new AppCommandOption
        {
            Name = "channels",
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
            }.Concat(_discordAnnouncers.SelectMany(x =>
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
        };

        private AppCommandOption FilterConfig => new AppCommandOption
        {
            Name = "filters",
            Description = "Configure various filters for this server",
            Type = AppCommandOptionType.SUB_COMMAND_GROUP,
            Options = new[]
            {
                new AppCommandOption
                {
                    Name = "new_item_categories",
                    Description = "Returns a menu to pick which categories you want. Enable none to show all items",
                    Type = AppCommandOptionType.SUB_COMMAND
                }
            }
        };

        public override Task<DiscordInteractionResponse> Execute(DiscordInteractionRequest interaction)
        {
            if (interaction.Type == InteractionType.APPLICATION_COMMAND)
            {
                var commandGroup = interaction.Data.Options.First();
                switch (commandGroup.Name)
                {
                    case "filters" when commandGroup.Options.First().Name == "new_item_categories":
                        return HandleNewItemCategoriesMenuRequest(interaction);
                    default:
                        return Task.FromResult<DiscordInteractionResponse>(new DiscordInteractionResponse.EphemeralDeferredContentResponse());
                }
            }
            else
            {
                switch (interaction.Data.CustomId)
                {
                    case "new_item_categories":
                        return HandleNewItemCategoriesMenuResponse(interaction);
                    default:
                        return Task.FromResult<DiscordInteractionResponse>(new DiscordInteractionResponse.EphemeralDeferredContentResponse());
                }
            }
        }

        public override Task DeferedExecute(DiscordInteractionRequest interaction)
        {
            var commandGroup = interaction.Data.Options.First();

            switch (commandGroup.Name)
            {
                case "channels":
                    return HandleAnnounceConfig(commandGroup, interaction);
                default:
                    return Task.CompletedTask;
            }
        }

        private async Task<DiscordInteractionResponse> HandleNewItemCategoriesMenuRequest(DiscordInteractionRequest interaction)
        {
            var guildSpecificSettings = await _configService.GetConfigValue("GUILDCONFIG_ANNOUNCE_NEWITEMTYPES", interaction.GuildId);
            var currentEnabledCategories = guildSpecificSettings?.Split(',').Select(x => Enum.Parse<FRItemCategory>(x)) ?? Enumerable.Empty<FRItemCategory>();

            return new DiscordInteractionResponse.ContentResponse
            {
                Data = new DiscordInteractionResponseData
                {
                    Flags = MessageFlags.EPHEMERAL,
                    Content = "Please select the categories you wish to show, disabling (or enabling) all categories means all categories will be shown and no filter will be used",
                    Components = new[]
                    {
                        new DiscordInteractionResponseActionRowComponent
                        {
                            Components = new[]
                            {
                                new DiscordInteractionResponseSelectComponent.String
                                {
                                    Custom_id = "new_item_categories",
                                    Options = Enum.GetValues<FRItemCategory>().Select(x => new DiscordInteractionSelectOption
                                    {
                                        Label = x.ToString(),
                                        Default = !currentEnabledCategories.Any() || currentEnabledCategories.Contains(x),
                                        Value = x.ToString()
                                    }),
                                    MinValues = 0,
                                    MaxValues = Enum.GetValues<FRItemCategory>().Length
                                }
                            }
                        }
                    }
                }
            };
        }
        private async Task<DiscordInteractionResponse> HandleNewItemCategoriesMenuResponse(DiscordInteractionRequest interaction)
        {
            if (interaction.Data.Values?.Any() == true)
            {
                var categoryChoices = interaction.Data.Values.Select(x => Enum.Parse<FRItemCategory>(x));
                await _configService.AddOrUpdateConfig("GUILDCONFIG_ANNOUNCE_NEWITEMTYPES", string.Join(',', categoryChoices.Select(x => (int)x)), interaction.GuildId);
            }
            else
                await _configService.RemoveConfig("GUILDCONFIG_ANNOUNCE_NEWITEMTYPES", interaction.GuildId);

            return new DiscordInteractionResponse.ContentResponse
            {
                Data = new DiscordInteractionResponseData
                {
                    Content = "Changes have been saved!",
                    Flags = MessageFlags.EPHEMERAL
                }
            };
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
                                Content = $"I do not have the required `MANAGE_WEBHOOKS` permission on this server to do that, adjust my permissions or manually create a webhook and add it using `/config announce {command.Name.Replace("channel", "webhook")}` instead."
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
                    Content = $"Configured the webhook for `{ParseCommandName(command.Name)}` as `{webhookUrl}`."
                });
            }
            else // getter
            {
                try
                {
                    var configValue = await _configService.GetConfigValue($"GUILDCONFIG_{ParseCommandName(command.Name).ToUpper()}WEBHOOK", interaction.GuildId);
                    await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = $"The webhook currently configured for `{ParseCommandName(command.Name)}` is `{configValue}`."
                    });
                }
                catch
                {
                    await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = $"No setting stored yet."
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
