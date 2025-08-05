using FRTools.Core.Data;
using FRTools.Core.Services.Actions;
using FRTools.Core.Services.Actions.Agents.DiscordActions;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands.DominanceAssign
{
    public class DominanceAssignCommand : BaseDiscordCommand
    {
        private readonly IConfigService _configService;
        private readonly IDiscordDominanceActions _discordDominanceActions;
        private readonly DataContext _dataContext;
        private readonly ILogger<ConfigCommand> _logger;

        public DominanceAssignCommand(IConfigService configService, IDiscordInteractionService discordInteractionService, IDiscordDominanceActions discordDominanceActions, DataContext dataContext, ILogger<ConfigCommand> logger) : base(discordInteractionService, logger)
        {
            _configService = configService;
            _discordDominanceActions = discordDominanceActions;
            _dataContext = dataContext;
            _logger = logger;
        }

        public override AppCommand Command => new AppCommand
        {
            Name = "dominance_assign",
            Description = "Commands related to dominance on Flight Rising.",
            DmPermission = false,
            Type = AppCommandType.CHAT_INPUT,
            DefaultMemberPermissions = Permissions.ADMINISTRATOR
        };

        public override async Task DeferedExecute(DiscordInteractionRequest interaction)
        {
            var guildDom = await _configService.GetConfigValue("GUILDCONFIG_DOMINANCE", interaction.GuildId);
            if (guildDom != $"{true}")
            {
                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = "Dominance is not enable on this server."
                });
                return;
            }


            await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
            {
                Content = "Assigning or removing Dominance role from users.."
            });

            try
            {
                var lastDominance = _dataContext.FRDominances.OrderByDescending(x => x.Timestamp).First();
                await _discordDominanceActions.AssignDominanceRole(new DominanceAgentData([(Flight)lastDominance.First, (Flight)lastDominance.Second, (Flight)lastDominance.Third]), interaction.GuildId);
                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = "Dominance role is now assigned to everyone that belongs to the winning flight."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing command.");

                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = "Something went wrong assigning the dominance role to users. Double check if I have the `MANAGE_ROLES` permission!"
                });
            }
        }
    }
}
