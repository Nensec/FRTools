using System.ComponentModel;
using System.Reflection;
using FRTools.Core.Data;
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

        public ConfigCommand(IConfigService configService, IDiscordInteractionService discordInteractionService, ILogger<ConfigCommand> logger) : base(discordInteractionService, logger)
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
            Options =
            [
                ChannelsConfig,
                FilterConfig,
                DominanceConfig
            ]
        };

        public override string[]? ComponentInteractionCommandNames =>
        [
            "new_item_categories",
            //"dominance_setup"
        ];

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
                    Options =
                    [
                        new AppCommandOption
                        {
                            Name = "channel",
                            Description = "The default channel to announce to, requires bot to have MANAGE_WEBHOOKS",
                            Type = AppCommandOptionType.CHANNEL,
                            ChannelTypes = [ChannelType.GUILD_TEXT],
                            Required = true
                        }
                    ]
                },
                new AppCommandOption
                {
                    Name = "default_webhook",
                    Description = "The default announcement webhook",
                    Type = AppCommandOptionType.SUB_COMMAND,
                    Options =
                    [
                        new AppCommandOption
                        {
                            Name = "webhook",
                            Description = "The default webhook to announce to",
                            Type = AppCommandOptionType.STRING,
                        }
                    ]
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
                        Options =
                        [
                            new AppCommandOption
                            {
                                Name = "channel",
                                Description = $"The channel to announce {x} to, requires bot to have MANAGE_WEBHOOKS",
                                Type = AppCommandOptionType.CHANNEL,
                                ChannelTypes = [ChannelType.GUILD_TEXT],
                                Required = true
                            }
                        ]
                    },
                    new AppCommandOption
                    {
                        Name = $"{x.Replace(' ', '_')}_webhook",
                        Description = $"Override's the default webhook for {x}",
                        Type = AppCommandOptionType.SUB_COMMAND,
                        Options =
                        [
                            new AppCommandOption
                            {
                                Name = "webhook",
                                Description = $"The webhook to announce {x} to",
                                Type = AppCommandOptionType.STRING,
                            }
                        ]
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
            Options =
            [
                new AppCommandOption
                {
                    Name = "new_item_categories",
                    Description = "Returns a menu to pick which categories you want. Enable none to show all items",
                    Type = AppCommandOptionType.SUB_COMMAND
                }
            ]
        };

        private AppCommandOption DominanceConfig => new AppCommandOption
        {
            Name = "dominance",
            Description = "Configure various dominance related settings for this server",
            Type = AppCommandOptionType.SUB_COMMAND_GROUP,
            Options =
            [
                //new AppCommandOption
                //{
                //    Name = "dom_setup",
                //    Description = "Runs a setup that helps you configure all the things!",
                //    Type = AppCommandOptionType.SUB_COMMAND
                //},
                new AppCommandOption
                {
                    Name = "dom_enable",
                    Description = "Enables dominance announcements, if roles are set this will also assign dominance role",
                    Type = AppCommandOptionType.SUB_COMMAND
                },
                new AppCommandOption
                {
                    Name = "dom_disable",
                    Description = "Disables dominance announcements and role assingement",
                    Type = AppCommandOptionType.SUB_COMMAND
                },
                new AppCommandOption
                {
                    Name = "dom_clear",
                    Description = "Clears all role assignments, does not disable announcing on its own",
                    Type = AppCommandOptionType.SUB_COMMAND
                },
                new AppCommandOption
                {
                    Name = "dom_role",
                    Description = "Sets the role for Dominance",
                    Type = AppCommandOptionType.SUB_COMMAND,
                    Options =
                    [
                        new AppCommandOption
                        {
                            Name = "role",
                            Description = "The role for Dominance",
                            Type = AppCommandOptionType.ROLE,
                            Required = true
                        }
                    ]
                },
                new AppCommandOption
                {
                    Name = "flight_roles",
                    Description = "Flight Roles",
                    Type = AppCommandOptionType.SUB_COMMAND,
                    Options = Enum.GetValues<Flight>().Except([Flight.Beastclans]).Select(x =>
                        new AppCommandOption
                        {
                            Name = $"{x.ToString().ToLower()}_role",
                            Description = $"Role to for {x}",
                            Type = AppCommandOptionType.ROLE,
                            Required = true
                        }).ToArray()
                }
            ]
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
                    //case "dominance" when commandGroup.Options.First().Name == "setup":
                    //    return HandleDominanceSetupMenuRequest(interaction);
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
                    //case "setup":
                    //    return HandleDominanceSetupMenuResponse(interaction);
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
                case "dominance":
                    return HandleDominanceConfig(commandGroup, interaction);
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
                    Components =
                    [
                        new DiscordInteractionResponseActionRowComponent
                        {
                            Components =
                            [
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
                            ]
                        }
                    ]
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

        //private async Task<DiscordInteractionResponse> HandleDominanceSetupMenuRequest(DiscordInteractionRequest interaction)
        //{
        //    return new DiscordInteractionResponse.ContentResponse
        //    {
        //        Data = new DiscordInteractionResponseData
        //        {
        //            Flags = MessageFlags.EPHEMERAL,
        //            Content = "Please select the roles for your server",
        //            Components =
        //            new[] {
        //                new DiscordInteractionResponseActionRowComponent
        //                {
        //                    Components =
        //                    [
        //                        new DiscordInteractionResponseSelectComponent.Role
        //                        {
        //                            Placeholder = "Dominance",
        //                            Custom_id = "setup_dominance",
        //                            MaxValues = 1,
        //                        }
        //                    ]
        //                }
        //            }.Concat(Enum.GetValues<Flight>().Except([Flight.Beastclans]).Select(x =>                    
        //                new DiscordInteractionResponseActionRowComponent
        //                {
        //                    Components =
        //                    [
        //                        new DiscordInteractionResponseSelectComponent.Role
        //                        {
        //                            Placeholder = $"{x}",
        //                            Custom_id = $"setup_{x.ToString().ToLower()}",
        //                            MaxValues = 1,
        //                        }
        //                    ]
        //                }
        //            ))
        //        }
        //    };
        //}

        //private async Task<DiscordInteractionResponse> HandleDominanceSetupMenuResponse(DiscordInteractionRequest interaction)
        //{
            
        //}

        private async Task HandleDominanceConfig(DiscordInteractionRequestOptionData commandData, DiscordInteractionRequest interaction)
        {
            var command = commandData.Options.First();

            if (command.Name == "dom_enable")
            {
                await _configService.AddOrUpdateConfig("GUILDCONFIG_DOMINANCE", $"{true}", interaction.GuildId);
                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"Dominance role assignement has been enabled."
                });
            }

            if (command.Name == "dom_disable")
            {
                await _configService.AddOrUpdateConfig("GUILDCONFIG_DOMINANCE", $"{false}", interaction.GuildId);
                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"Dominance role assignement has been disabled."
                });
            }

            if (command.Name == "dom_clear")
            {
                await _configService.RemoveConfig("GUILDCONFIG_DOMINANCE_ROLE", interaction.GuildId);
                var flights = Enum.GetValues<Flight>().Except([Flight.Beastclans]);
                foreach (var flight in flights)
                {
                    await _configService.RemoveConfig($"GUILDCONFIG_{flight.ToString().ToUpper()}_ROLE", interaction.GuildId);
                }
                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"Dominance role assignements have been cleared."
                });
            }

            if (command.Name == "dom_role")
            {
                var role = command.Options.First();
                await _configService.AddOrUpdateConfig("GUILDCONFIG_DOMINANCE_ROLE", (string)role.Value, interaction.GuildId);

                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"Dom role has been updated to <@&{role.Value}>."
                });
                return;
            }

            if (command.Name == "flight_roles")
            {
                var flights = Enum.GetValues<Flight>().Except([Flight.Beastclans]);
                foreach (var flight in flights)
                {
                    var role = command.Options[$"{flight.ToString().ToLower()}_role"];
                    await _configService.AddOrUpdateConfig($"GUILDCONFIG_{flight.ToString().ToUpper()}_ROLE", (string)role.Value, interaction.GuildId);
                }

                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"Flight roles have been updated to the following:\n" + string.Join("\n", flights.Select(x => $"- {x}: <@&{command.Options[$"{x.ToString().ToLower()}_role"].Value}>"))
                });
                return;
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
                            await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                            {
                                Content = $"I do not have the required `MANAGE_WEBHOOKS` permission on this server to do that, adjust my permissions or manually create a webhook and add it using `/config announce {command.Name.Replace("channel", "webhook")}` instead."
                            });
                        }
                        catch { }
                        return;
                    }

                    webhookUrl = await DiscordInteractionService.CreateWebhook($"FRTools_{ParseCommandName(command.Name)}", ulong.Parse((string)command.Options.First().Value));
                }
                else
                {
                    webhookUrl = (string)command.Options.First().Value;
                }

                await _configService.AddOrUpdateConfig($"GUILDCONFIG_{ParseCommandName(command.Name).ToUpper()}WEBHOOK", webhookUrl, interaction.GuildId);
                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"Configured the webhook for `{ParseCommandName(command.Name)}` as `{webhookUrl}`."
                });
            }
            else // getter
            {
                try
                {
                    var configValue = await _configService.GetConfigValue($"GUILDCONFIG_{ParseCommandName(command.Name).ToUpper()}WEBHOOK", interaction.GuildId);
                    await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = $"The webhook currently configured for `{ParseCommandName(command.Name)}` is `{configValue}`."
                    });
                }
                catch
                {
                    await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
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
