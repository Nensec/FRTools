using FRTools.Core.Services.Actions.Agents.DiscordActions;

namespace FRTools.Core.Services.Actions.Agents
{
    public class DiscordAgent : IDominanceAgent
    {
        private readonly IDiscordDominanceActions _discordDominanceActions;

        public DiscordAgent(IDiscordDominanceActions discordDominanceActions)
        {
            _discordDominanceActions = discordDominanceActions;
        }

        public Task Act(AgentData agentData)
        {
            switch (agentData)
            {
                case DominanceAgentData dominanceAgentData:
                    return _discordDominanceActions.PerformDominanceTasks(dominanceAgentData);
                default:
                    return Task.CompletedTask;
            }
        }
    }
}