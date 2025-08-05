using FakeItEasy.Sdk;
using FRTools.Core.Data;
using FRTools.Core.Services;
using FRTools.Core.Services.Actions;
using FRTools.Core.Services.Actions.Agents;
using FRTools.Core.Services.Actions.Agents.DiscordActions;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Tests.Services
{
    public class AgentServiceTests
    {
        public static IEnumerable<object[]> AgentData => typeof(AgentData).Assembly
            .GetTypes()
            .Where(x => typeof(AgentData).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(x => new object[] { x, x.BaseType!.GenericTypeArguments.First() });

        private AgentData CreateFakeData(Type agentData)
        {
            if (agentData == typeof(DominanceAgentData))
                return (AgentData)Create.Fake(agentData, x => x.WithArgumentsForConstructor(new object[] { new[] { Flight.Beastclans, Flight.Earth, Flight.Water } }));

            throw new NotImplementedException();
        }

        [Theory]
        [MemberData(nameof(AgentData))]
        public async Task AgentService_With_Registered_Agent_Should_Act(Type agentDataType, Type interfaceType)
        {
            var agentService = new AgentService(A.Fake<ILogger<AgentService>>());
            var fakeAgents = new List<IAgent>();
            for (int i = 0; i < 10; i++)
            {
                var fakeAgent = A.Fake<IAgent>(x => x.Implements(interfaceType));
                agentService.RegisterAgent(fakeAgent);
                fakeAgents.Add(fakeAgent);
            }

            var agentData = CreateFakeData(agentDataType);
            A.CallTo(() => agentData.AgentType).Returns(interfaceType);

            await agentService.Act(agentData);

            foreach (var fakeAnnouncer in fakeAgents)
                A.CallTo(() => fakeAnnouncer.Act(null!)).WithAnyArguments().MustHaveHappened();
        }

        [Theory]
        [MemberData(nameof(AgentData))]
        public async Task AgentService_With_Registered_Agent_Should_Gracefully_Log_Error(Type agentDataType, Type interfaceType)
        {
            var fakeLogger = A.Fake<ILogger<AgentService>>();
            var agentService = new AgentService(fakeLogger);
            var fakeAgent = A.Fake<IAgent>(x => x.Implements(interfaceType));
            agentService.RegisterAgent(fakeAgent);
            var agentData = CreateFakeData(agentDataType);
            A.CallTo(() => agentData.AgentType).Returns(interfaceType);
            A.CallTo(() => fakeAgent.Act(null!)).WithAnyArguments().Throws<Exception>();

            await agentService.Act(agentData);

            A.CallTo(() => fakeAgent.Act(null!)).WithAnyArguments().MustHaveHappened();
            A.CallTo(fakeLogger).Where(x => x.Method.Name == "Log" && x.GetArgument<LogLevel>(0) == LogLevel.Error).MustHaveHappened();
        }

        [Theory]
        [MemberData(nameof(AgentData))]
        public async Task DiscordAnnouncer_Should_Announce_When_Interface_Matches_Data(Type agentDataType, Type interfaceType)
        {
            var fakeDiscordDominanceActions = A.Fake<IDiscordDominanceActions>();
            var discordAgent = new DiscordAgent(fakeDiscordDominanceActions);

            var agentData = CreateFakeData(agentDataType);
            await discordAgent.Act(agentData);

            if (interfaceType.IsAssignableFrom(typeof(DiscordAgent)))
            {
                if (interfaceType == typeof(IDominanceAgent))
                {
                    A.CallTo(() => fakeDiscordDominanceActions.PerformDominanceTasks(null!)).WithAnyArguments().MustHaveHappened();
                }
                else
                    throw new Exception("Unknown interface?");
            }
            else
            {
                A.CallTo(() => fakeDiscordDominanceActions.PerformDominanceTasks(null!)).WithAnyArguments().MustNotHaveHappened();
            }
        }
    }
}
