using FRTools.Core.Services.Actions;

namespace FRTools.Core.Services.Interfaces
{
    public interface IAgentService
    {
        Task Act(AgentData agentData);
        void RegisterAgent(IAgent agent);
    }
}