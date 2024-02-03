using FakeItEasy.Sdk;
using FRTools.Core.Services;
using FRTools.Core.Services.Announce;
using Microsoft.Extensions.Logging;

namespace FRTools.Test.Services
{
    public class AnnounceServiceTests
    {
        public static IEnumerable<object[]> AnnouncerData => typeof(AnnounceData).Assembly
            .GetTypes()
            .Where(x => typeof(AnnounceData).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(x => new object[] { x, x.BaseType!.GenericTypeArguments.First() });

        [Theory]
        [MemberData(nameof(AnnouncerData))]
        public async Task AnnounceService_With_Registered_Announcer_Should_Announce(Type announceDataType, Type interfaceType)
        {
            var announceService = new AnnounceService(A.Fake<ILogger<AnnounceService>>());
            var fakeAnnouncers = new List<IAnnouncer>();
            for(int i = 0; i < 10; i++)
            {
                var fakeAnnouncer = A.Fake<IAnnouncer>(x => x.Implements(interfaceType));
                announceService.RegisterAnnouncer(fakeAnnouncer);
                fakeAnnouncers.Add(fakeAnnouncer);
            }

            var announceData = (AnnounceData)Create.Fake(announceDataType);
            A.CallTo(() => announceData.AnnouncerType).Returns(interfaceType);

            await announceService.Announce(announceData);

            foreach(var fakeAnnouncer in fakeAnnouncers)
                A.CallTo(() => fakeAnnouncer.Announce(null!)).WithAnyArguments().MustHaveHappened();
        }

        [Theory]
        [MemberData(nameof(AnnouncerData))]
        public async Task AnnounceService_With_Registered_Announcer_Should_Gracefully_Log_Error(Type announceDataType, Type interfaceType)
        {
            var fakeLogger = A.Fake<ILogger<AnnounceService>>();
            var announceService = new AnnounceService(fakeLogger);
            var fakeAnnouncer = A.Fake<IAnnouncer>(x => x.Implements(interfaceType));
            announceService.RegisterAnnouncer(fakeAnnouncer);
            var announceData = (AnnounceData)Create.Fake(announceDataType);            
            A.CallTo(() => announceData.AnnouncerType).Returns(interfaceType);
            A.CallTo(() => fakeAnnouncer.Announce(null!)).WithAnyArguments().Throws<Exception>();

            await announceService.Announce(announceData);

            A.CallTo(() => fakeAnnouncer.Announce(null!)).WithAnyArguments().MustHaveHappened();
            A.CallTo(fakeLogger).Where(x => x.Method.Name == "Log" && x.GetArgument<LogLevel>(0) == LogLevel.Error).MustHaveHappened();
        }
    }
}
