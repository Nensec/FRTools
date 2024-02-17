using FakeItEasy.Sdk;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Announce.Announcers;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Tests.Services
{
    public class AnnounceServiceTests
    {
        public static IEnumerable<object[]> AnnouncerData => typeof(AnnounceData).Assembly
            .GetTypes()
            .Where(x => typeof(AnnounceData).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(x => new object[] { x, x.BaseType!.GenericTypeArguments.First() });

        private AnnounceData CreateFakeData(Type announceData)
        {
            if (announceData == typeof(DominanceAnnounceData))
                return (AnnounceData)Create.Fake(announceData, x => x.WithArgumentsForConstructor(new object[] { new[] { Flight.Beastclans, Flight.Earth, Flight.Water } }));
            else if (announceData == typeof(FlashSaleAnnounceData))
                return (AnnounceData)Create.Fake(announceData, x => x.WithArgumentsForConstructor(new object[] { A.Fake<FRItem>(x => x.ConfigureFake(item => item.Name = "Fake Item")), string.Empty }));
            else if (announceData == typeof(NewItemsAnnounceData))
                return (AnnounceData)Create.Fake(announceData, x => x.WithArgumentsForConstructor(new object[] { A.CollectionOfFake<FRItem>(10, (x, i) => x.ConfigureFake(item => { item.Name = "Fake Item"; item.FRId = i; })) }));

            throw new NotImplementedException();
        }

        private IConfigService CreateFakeConfig()
        {
            var configService = A.Fake<IConfigService>();
            A.CallTo(() => configService.GetAllConfig(null!)).WithAnyArguments().Returns(new[]
            {
                ("GUILDCONFIG_DEFAULTWEBHOOK", string.Empty, (ulong)1)
            });
            A.CallTo(() => configService.GetConfigValue(null!, 0)).WithAnyArguments().Returns((string)null!);

            return configService;
        }

        [Theory]
        [MemberData(nameof(AnnouncerData))]
        public async Task AnnounceService_With_Registered_Announcer_Should_Announce(Type announceDataType, Type interfaceType)
        {
            var announceService = new AnnounceService(A.Fake<ILogger<AnnounceService>>());
            var fakeAnnouncers = new List<IAnnouncer>();
            for (int i = 0; i < 10; i++)
            {
                var fakeAnnouncer = A.Fake<IAnnouncer>(x => x.Implements(interfaceType));
                announceService.RegisterAnnouncer(fakeAnnouncer);
                fakeAnnouncers.Add(fakeAnnouncer);
            }

            var announceData = CreateFakeData(announceDataType);
            A.CallTo(() => announceData.AnnouncerType).Returns(interfaceType);

            await announceService.Announce(announceData);

            foreach (var fakeAnnouncer in fakeAnnouncers)
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
            var announceData = CreateFakeData(announceDataType);
            A.CallTo(() => announceData.AnnouncerType).Returns(interfaceType);
            A.CallTo(() => fakeAnnouncer.Announce(null!)).WithAnyArguments().Throws<Exception>();

            await announceService.Announce(announceData);

            A.CallTo(() => fakeAnnouncer.Announce(null!)).WithAnyArguments().MustHaveHappened();
            A.CallTo(fakeLogger).Where(x => x.Method.Name == "Log" && x.GetArgument<LogLevel>(0) == LogLevel.Error).MustHaveHappened();
        }

        [Theory]
        [MemberData(nameof(AnnouncerData))]
        public async Task DiscordAnnouncer_Should_Announce_When_Interface_Matches_Data(Type announceDataType, Type interfaceType)
        {
            var fakeDiscordService = A.Fake<IDiscordService>();
            var discordAnnouncer = new DiscordWebhookAnnouncer(CreateFakeConfig(), fakeDiscordService, A.Fake<IItemAssetDataService>(), A.Fake<IFRUserService>(), A.Fake<ILogger<DiscordWebhookAnnouncer>>());

            var announceData = CreateFakeData(announceDataType);
            await discordAnnouncer.Announce(announceData);

            if (interfaceType.IsAssignableFrom(typeof(DiscordWebhookAnnouncer)))
                A.CallTo(() => fakeDiscordService.PostMessageToWebhook(null!, null!)).WithAnyArguments().MustHaveHappened();
            else
                A.CallTo(() => fakeDiscordService.PostMessageToWebhook(null!, null!)).WithAnyArguments().MustNotHaveHappened();
        }

        [Theory]
        [MemberData(nameof(AnnouncerData))]
        public async Task TumblrAnnouncer_Should_Announce_When_Interface_Matches_Data(Type announceDataType, Type interfaceType)
        {
            var tumblrService = A.Fake<ITumblrService>();
            var tumblrAnnouncer = new TumblrAnnouncer(A.Fake<IFRUserService>(), tumblrService, A.Fake<ILogger<TumblrAnnouncer>>());

            var announceData = CreateFakeData(announceDataType);
            await tumblrAnnouncer.Announce(announceData);

            if (interfaceType.IsAssignableFrom(typeof(TumblrAnnouncer)))
                A.CallTo(() => tumblrService.CreatePost(null!, null!)).WithAnyArguments().MustHaveHappened();
            else
                A.CallTo(() => tumblrService.CreatePost(null!, null!)).WithAnyArguments().MustNotHaveHappened();
        }
    }
}
