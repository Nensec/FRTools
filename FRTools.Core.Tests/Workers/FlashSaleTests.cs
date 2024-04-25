using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Functions.Workers;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Tests.Workers
{
    public class FlashSaleTests
    {
        [Fact]

        public async Task FlashSale_Should_Retrieve_FlashSale_And_Announce_It()
        {
            var fakeHtmlService = A.Fake<IHtmlService>();
            A.CallTo(() => fakeHtmlService.LoadHtmlPage(string.Empty)).WithAnyArguments().Returns(LoadFakeDocument("TestData/FlashSalePage.html"));
            var fakeDataContext = A.Fake<DataContext>(x => x.WithArgumentsForConstructor(new object[] { new DbContextOptionsBuilder<DataContext>().Options }));
            A.CallTo(() => fakeDataContext.FRItemFlashSales).Returns(TestHelpers.CreateFakeDbSet<FRItemFlashSale>());

            var fakeAnnouncerService = A.Fake<IAnnounceService>();

            var flashSaleFunction = new FlashSaleTrackerFunction(fakeDataContext, A.Fake<IFRUserService>(), A.Fake<IFRItemService>(), fakeAnnouncerService, fakeHtmlService, A.Fake<ILogger<FlashSaleTrackerFunction>>());

            await flashSaleFunction.FlashTracker(null);
            A.CallTo(() => fakeAnnouncerService.Announce(null!)).WhenArgumentsMatch(x => x[0] is FlashSaleAnnounceData newItemsData).MustHaveHappened();
        }

        [Fact]
        public async Task FlashSale_Should_Retrieve_Items_And_Follow_Breadcrum_Trail()
        {
            var fakePageSequence = new[] { LoadFakeDocument("TestData/FlashSalePageFlashless.html"), LoadFakeDocument("TestData/FlashSalePage.html") };
            var fakeHtmlService = A.Fake<IHtmlService>();
            A.CallTo(() => fakeHtmlService.LoadHtmlPage(FRHelpers.MarketplaceUrl)).Returns(LoadFakeDocument("TestData/FlashSalePageFlashless.html"));
            A.CallTo(() => fakeHtmlService.LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, "apparel"))).ReturnsNextFromSequence(fakePageSequence);
            var fakeDataContext = A.Fake<DataContext>(x => x.WithArgumentsForConstructor(new object[] { new DbContextOptionsBuilder<DataContext>().Options }));
            A.CallTo(() => fakeDataContext.FRItemFlashSales).Returns(TestHelpers.CreateFakeDbSet<FRItemFlashSale>());

            var flashSaleFunction = new FlashSaleTrackerFunction(fakeDataContext, A.Fake<IFRUserService>(), A.Fake<IFRItemService>(), A.Fake<IAnnounceService>(), fakeHtmlService, A.Fake<ILogger<FlashSaleTrackerFunction>>());

            await flashSaleFunction.FlashTracker(null);
            A.CallTo(() => fakeHtmlService.LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, "apparel"))).MustHaveHappenedTwiceExactly();
        }

        public static IEnumerable<object[]> FlashSaleTestData =>
            new[]
            {
                new object[] { null!, new FRItem{ FRId = 1 } },
                new object[] { new FRItem{ FRId = 1 }, null! }
            };

        [Theory]
        [MemberData(nameof(FlashSaleTestData))]
        public async Task FlashSale_Should_Retrieve_FlashSale_Fetch_Item_And_Update_If_Needed(FRItem? getItem, FRItem? fetchItem)
        {
            var fakeHtmlService = A.Fake<IHtmlService>();
            A.CallTo(() => fakeHtmlService.LoadHtmlPage(string.Empty)).WithAnyArguments().Returns(LoadFakeDocument("TestData/FlashSalePage.html"));
            var fakeDataContext = A.Fake<DataContext>(x => x.WithArgumentsForConstructor(new object[] { new DbContextOptionsBuilder<DataContext>().Options }));
            A.CallTo(() => fakeDataContext.FRItemFlashSales).Returns(TestHelpers.CreateFakeDbSet<FRItemFlashSale>());
            var fakeItemService = A.Fake<IFRItemService>();
            A.CallTo(() => fakeItemService.GetItem(1)).WithAnyArguments().Returns(getItem);
            A.CallTo(() => fakeItemService.FetchItemFromFR(1, "skins")).WithAnyArguments().Returns(fetchItem);

            var flashSaleFunction = new FlashSaleTrackerFunction(fakeDataContext, A.Fake<IFRUserService>(), fakeItemService, A.Fake<IAnnounceService>(), fakeHtmlService, A.Fake<ILogger<FlashSaleTrackerFunction>>());

            await flashSaleFunction.FlashTracker(null);

            A.CallTo(() => fakeItemService.GetItem(1)).WithAnyArguments().MustHaveHappened();
            if (getItem == null)
                A.CallTo(() => fakeItemService.FetchItemFromFR(1, "skins")).WithAnyArguments().MustHaveHappened();
            else
                A.CallTo(() => fakeItemService.FetchItemFromFR(1, "skins")).WithAnyArguments().MustNotHaveHappened();
        }

        static HtmlDocument LoadFakeDocument(string htmlDoc)
        {
            var document = new HtmlDocument();
            document.LoadHtml(File.ReadAllText(htmlDoc));
            return document;
        }
    }
}
