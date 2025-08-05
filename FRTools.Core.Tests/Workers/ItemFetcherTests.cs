using System.Text;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Functions.Workers;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Tests.Workers
{
    public class ItemFetcherTests
    {
        [Fact]
        public async Task Item_Fetcher_Should_Get_LastRun_Json_Data_From_Storage()
        {
            var fakeAzureStorage = A.Fake<IAzureStorageService>();
            A.CallTo(() => fakeAzureStorage.Exists("general-data\\item-fetch\\lastrun.json")).Returns(true);
            A.CallTo(() => fakeAzureStorage.GetFile("general-data\\item-fetch\\lastrun.json")).Returns(new MemoryStream(Encoding.UTF8.GetBytes("{\"Count\":2,\"LastSuccess\":\"2024-02-16T21:45:05.4983511Z\"}")));

            // Not including this in this test makes FakeItEasy return a non-null FRItem which makes the function loop endlessly expecting a null item eventually
            var fakeItemService = A.Fake<IFRItemService>();
            A.CallTo(() => fakeItemService.FetchItemFromFR(0, "skins")).WithAnyArguments().Returns((FRItem)null!);

            var itemFetcherFunction = new ItemFetcherFunction(fakeAzureStorage, fakeItemService, A.Fake<IAzurePipelineService>(), A.Fake<IAnnounceService>(), A.Fake<ILogger<ItemFetcherFunction>>());

            await itemFetcherFunction.ItemFetcher(null!);

            A.CallTo(() => fakeAzureStorage.Exists("general-data\\item-fetch\\lastrun.json")).MustHaveHappened();
            A.CallTo(() => fakeAzureStorage.GetFile("general-data\\item-fetch\\lastrun.json")).MustHaveHappened();
        }

        [Fact]
        public async Task Item_Fetcher_Should_Fetch_Items_And_Announce_Them()
        {
            var fakeItems = Enumerable.Range(0, 10).Select(x => new FRItem { FRId = x });
            var fakeItemSequence = fakeItems.Concat([(null!), null!, null!]).ToArray();

            var fakeItemService = A.Fake<IFRItemService>();
            A.CallTo(() => fakeItemService.FetchItemFromFR(0, "skins")).WithAnyArguments().ReturnsNextFromSequence(fakeItemSequence);
            var fakeAnnouncerService = A.Fake<IAnnounceService>();

            var itemFetcherFunction = new ItemFetcherFunction(A.Fake<IAzureStorageService>(), fakeItemService, A.Fake<IAzurePipelineService>(), fakeAnnouncerService, A.Fake<ILogger<ItemFetcherFunction>>());

            await itemFetcherFunction.ItemFetcher(null!);

            A.CallTo(() => fakeItemService.FetchItemFromFR(0, string.Empty)).WithAnyArguments().MustHaveHappened(fakeItemSequence.Count(), Times.Exactly);
            A.CallTo(() => fakeAnnouncerService.Announce(null!)).WhenArgumentsMatch(x => x[0] is NewItemsAnnounceData newItemsData).MustHaveHappened();
        }

        [Fact]
        public async Task Item_Fetcher_Should_Fetch_Items_And_When_No_Items_Found_Should_Not_Announce()
        {
            var fakeItems = Enumerable.Empty<FRItem>();
            var fakeItemSequence = fakeItems.Concat([(null!), null!, null!]).ToArray();

            var fakeItemService = A.Fake<IFRItemService>();
            A.CallTo(() => fakeItemService.FetchItemFromFR(0, "skins")).WithAnyArguments().ReturnsNextFromSequence(fakeItemSequence);
            var fakeAnnouncerService = A.Fake<IAnnounceService>();

            var itemFetcherFunction = new ItemFetcherFunction(A.Fake<IAzureStorageService>(), fakeItemService, A.Fake<IAzurePipelineService>(), fakeAnnouncerService, A.Fake<ILogger<ItemFetcherFunction>>());

            await itemFetcherFunction.ItemFetcher(null!);

            A.CallTo(() => fakeItemService.FetchItemFromFR(0, string.Empty)).WithAnyArguments().MustHaveHappened(fakeItemSequence.Count(), Times.Exactly);
            A.CallTo(() => fakeAnnouncerService.Announce(null!)).WhenArgumentsMatch(x => x[0] is NewItemsAnnounceData newItemsData).MustNotHaveHappened();
        }

        [Theory]
#if !DEBUG
        // Fake items
        [InlineData(FRItemCategory.Skins, "Fake Female Only", "Accent: Fake Skin", true)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Primary Fake Gene: Fake", true)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Secondary Fake Gene: Fake", true)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Tertiary Fake Gene: Fake", true)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Primary Gaoler Gene: Fake", true)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Secondary Gaoler Gene: Fake", true)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Tertiary Gaoler Gene: Fake", true)]
#endif
        // Real item data
        [InlineData(FRItemCategory.Skins, "Gaoler Female Only", "Accent: Gaoler Skin", false)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Primary Auraboa Gene: Vipera", false)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Secondary Auraboa Gene: Weaver", false)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Tertiary Auraboa Gene: Stained", false)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Primary Auraboa Gene: Love", false)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Secondary Auraboa Gene: Affection", false)]
        [InlineData(FRItemCategory.Trinket, "Specialty Item", "Tertiary Auraboa Gene: Crystalline", false)]
        public async Task Item_Fetcher_Should_Call_Pipeline_When_Unknown_Data_Is_Found(FRItemCategory itemCategory, string itemType, string itemName, bool result)
        {
            var fakeItems = Enumerable.Range(0, 1).Select(x => new FRItem { FRId = x, ItemCategory = itemCategory, ItemType = itemType, Name = itemName });
            var fakeItemSequence = fakeItems.Concat([(null!), null!, null!]).ToArray();

            var fakeItemService = A.Fake<IFRItemService>();
            A.CallTo(() => fakeItemService.FetchItemFromFR(0, "skins")).WithAnyArguments().ReturnsNextFromSequence(fakeItemSequence);
            var fakePipeLineService = A.Fake<IAzurePipelineService>();

            var itemFetcherFunction = new ItemFetcherFunction(A.Fake<IAzureStorageService>(), fakeItemService, fakePipeLineService, A.Fake<IAnnounceService>(), A.Fake<ILogger<ItemFetcherFunction>>());

            await itemFetcherFunction.ItemFetcher(null!);

            A.CallTo(() => fakeItemService.FetchItemFromFR(0, string.Empty)).WithAnyArguments().MustHaveHappened(fakeItemSequence.Count(), Times.Exactly);
            A.CallTo(() => fakePipeLineService.TriggerPipeline(string.Empty)).WithAnyArguments().MustHaveHappened(result ? 1 : 0, Times.Exactly);
        }
    }
}
