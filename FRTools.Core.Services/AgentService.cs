using FRTools.Core.Services.Actions;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services
{
    public class AgentService : IAgentService
    {
        private readonly ILogger<AgentService> _logger;
        private readonly List<IAgent> _agents = [];

        public AgentService(ILogger<AgentService> logger)
        {
            _logger = logger;
        }

        public async Task Act(AgentData agentData)
        {
            foreach (var agent in _agents.Where(x => agentData.AgentType.IsAssignableFrom(x.GetType())))
            {
                try
                {
                    await agent.Act(agentData);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Something went wrong attempting to run agent {0} using {1}: {2}", agentData.GetType().Name, agent.GetType().Name, ex.ToString());
                }
            }
        }

        public void RegisterAgent(IAgent agent)
        {
            _agents.Add(agent);
        }
    }
}