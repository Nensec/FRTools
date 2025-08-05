namespace FRTools.Core.Services.Actions
{
    public interface IAgent
    {
        Task Act(AgentData agentData);
    }

    public interface IDominanceAgent : IAgent { }
}
