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

        public DominanceAssignCommand(IConfigService configService, IDiscordInteractionService discordInteractionService, IDiscordDominanceActions discordDominanceActions, DataContext dataContext, ILogger<ConfigCommand> logger) : base(discordInteractionService, logger)
        {
            _configService = configService;
            _discordDominanceActions = discordDominanceActions;
            _dataContext = dataContext;
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

            var lastDominance = _dataContext.FRDominances.OrderByDescending(x => x.Timestamp).First();
            await _discordDominanceActions.AssignDominanceRole(new DominanceAgentData([(Flight)lastDominance.First, (Flight)lastDominance.Second, (Flight)lastDominance.Third]), interaction.GuildId);
        }
    }
}
