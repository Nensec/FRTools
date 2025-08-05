using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands.Dominance
{
    public class DominanceCommand : BaseDiscordCommand
    {
        private readonly IConfigService _configService;

        public DominanceCommand(IConfigService configService, IDiscordInteractionService discordInteractionService, ILogger<ConfigCommand> logger) : base(discordInteractionService, logger)
        {
            _configService = configService;
        }

        public override AppCommand Command => new AppCommand
        {
            Name = "dominance",
            Description = "Commands related to dominance on Flight Rising.",
            DmPermission = false,
            Type = AppCommandType.CHAT_INPUT,
            Options =
            [
                new AppCommandOption
                {
                    Name = "opt_in",
                    Description = "Opts in to getting assigned the Dominance role when your flight wins dominance",
                    Type = AppCommandOptionType.SUB_COMMAND

                },
                new AppCommandOption
                {
                    Name = "opt_out",
                    Description = "Opts out to getting assigned the Dominance role when your flight wins dominance",
                    Type = AppCommandOptionType.SUB_COMMAND
                }
            ]
        };

        public override async Task DeferedExecute(DiscordInteractionRequest interaction)
        {
            var command = interaction.Data.Options.First();
            var domOptInConfig = (await _configService.GetConfigValue("GUILDCONFIG_DOMINANCE_OPTIN", interaction.GuildId))?.Split(';').ToList() ?? [];
            var domRoleConfig = await _configService.GetConfigValue("GUILDCONFIG_DOMINANCE_ROLE", interaction.GuildId);

            if (command.Name == "opt_in")
            {
                if (domOptInConfig.Contains(interaction.Member.User.Id.ToString()))
                {
                    await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = $"You are already opted in."
                    });
                    return;
                }

                domOptInConfig.Add(interaction.Member.User.Id.ToString());

                await _configService.AddOrUpdateConfig("GUILDCONFIG_DOMINANCE_OPTIN", string.Join(';', domOptInConfig), interaction.GuildId);
                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"You have opted in to be given the <&{domRoleConfig}> role whenever your your flight wins dominance."
                });
                return;
            }

            if (command.Name == "opt_out")
            {
                if (!domOptInConfig.Contains(interaction.Member.User.Id.ToString()))
                {
                    await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = $"You are not opted in."
                    });
                    return;
                }

                domOptInConfig.Remove(interaction.Member.User.Id.ToString());

                await _configService.AddOrUpdateConfig("GUILDCONFIG_DOMINANCE_OPTIN", string.Join(';', domOptInConfig), interaction.GuildId);
                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"You have opted in to be given the <&{domRoleConfig}> role whenever your your flight wins dominance."
                });
                return;
            }
        }
    }
}
