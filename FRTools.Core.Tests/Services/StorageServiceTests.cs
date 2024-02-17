using Azure.Storage.Blobs;
using FRTools.Core.Services;
using Microsoft.Extensions.Azure;

namespace FRTools.Core.Tests.Services
{
    public class StorageServiceTests
    {
        private class FakeBlobServiceData
        {
            public IAzureClientFactory<BlobServiceClient> FakeAzureClientFactory { get; }
            public BlobServiceClient FakeServiceClient { get; }
            public BlobContainerClient FakeContainerClient { get; }
            public BlobClient FakeBlobClient { get; }

            public FakeBlobServiceData(IAzureClientFactory<BlobServiceClient> fakeAzureClientFactory, BlobServiceClient fakeServiceClient, BlobContainerClient fakeContainerClient, BlobClient fakeBlobClient)
            {
                FakeAzureClientFactory = fakeAzureClientFactory;
                FakeServiceClient = fakeServiceClient;
                FakeContainerClient = fakeContainerClient;
                FakeBlobClient = fakeBlobClient;
            }
        }

        private FakeBlobServiceData CreateFakeData()
        {
            var fakeClientFactory = A.Fake<IAzureClientFactory<BlobServiceClient>>();
            var fakeServiceClient = A.Fake<BlobServiceClient>();
            var fakeContainerClient = A.Fake<BlobContainerClient>();
            var fakeBlobClient = A.Fake<BlobClient>();

            A.CallTo(() => fakeClientFactory.CreateClient(null!)).WithAnyArguments().Returns(fakeServiceClient);
            A.CallTo(() => fakeServiceClient.GetBlobContainerClient(null!)).WithAnyArguments().Returns(fakeContainerClient);
            A.CallTo(() => fakeContainerClient.GetBlobClient(null!)).WithAnyArguments().Returns(fakeBlobClient);
            A.CallTo(() => fakeBlobClient.Uri).Returns(new Uri("http://fake.uri.to/a.file.png"));

            return new FakeBlobServiceData(fakeClientFactory, fakeServiceClient, fakeContainerClient, fakeBlobClient);
        }

        [Fact]
        public async Task Get_Blob_Test()
        {
            var fakeClientData = CreateFakeData();
            var storageService = new AzureStorageService(fakeClientData.FakeAzureClientFactory);

            var blob = await storageService.GetFile(@"a\fake\path\to\a\file.png");

            A.CallTo(() => fakeClientData.FakeContainerClient.GetBlobClient(null!)).WithAnyArguments().MustHaveHappened();
            A.CallTo(() => fakeClientData.FakeBlobClient.OpenReadAsync(null!, default)).WithAnyArguments().MustHaveHappened();
        }

        [Fact]
        public async Task Delete_Blob_Test()
        {
            var fakeClientData = CreateFakeData();
            var storageService = new AzureStorageService(fakeClientData.FakeAzureClientFactory);

            var deleted = await storageService.DeleteFile(@"a\fake\path\to\a\file.png");

            A.CallTo(() => fakeClientData.FakeContainerClient.DeleteBlobIfExistsAsync(null!, Azure.Storage.Blobs.Models.DeleteSnapshotsOption.None, null, default)).WithAnyArguments().MustHaveHappened();
        }

        [Fact]
        public async Task Create_Or_Update_Blob_Test()
        {
            var fakeClientData = CreateFakeData();
            var storageService = new AzureStorageService(fakeClientData.FakeAzureClientFactory);

            var blob = await storageService.CreateOrUpdateFile(@"a\fake\path\to\a\file.png", null!);

            A.CallTo(() => fakeClientData.FakeContainerClient.GetBlobClient(null!)).WithAnyArguments().MustHaveHappened();
            A.CallTo(() => fakeClientData.FakeBlobClient.UploadAsync((Stream)null!, true, default)).WhenArgumentsMatch(x => (bool)x[1]! == true).MustHaveHappened();
        }
        [Fact]
        public async Task Exists_Blob_Test()
        {
            var fakeClientData = CreateFakeData();
            var storageService = new AzureStorageService(fakeClientData.FakeAzureClientFactory);

            var exists = await storageService.Exists(@"a\fake\path\to\a\file.png");

            A.CallTo(() => fakeClientData.FakeContainerClient.GetBlobClient(null!)).WithAnyArguments().MustHaveHappened();
            A.CallTo(() => fakeClientData.FakeBlobClient.ExistsAsync(default)).WithAnyArguments().MustHaveHappened();
        }
    }
}
