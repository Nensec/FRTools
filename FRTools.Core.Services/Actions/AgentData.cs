using FRTools.Core.Data;
using FRTools.Core.Services.Announce;

namespace FRTools.Core.Services.Actions
{
    public abstract class AgentData
    {
        public abstract Type AgentType { get; }
    }

    public abstract class AgentData<T> : AgentData
    {
        public override Type AgentType => typeof(T);
    }

    public class DominanceAgentData : AgentData<IDominanceAgent>
    {
        public Flight[] Flights { get; }

        public DominanceAgentData(Flight[] flights)
        {
            Flights = flights;
        }
    }
}
