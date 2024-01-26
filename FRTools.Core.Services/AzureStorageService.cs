using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using FRTools.Core.Services.Interfaces;

namespace FRTools.Core.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly BlobServiceClient _client;

        public AzureStorageService()
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _client = new BlobServiceClient(connectionString);
        }

        public async Task<bool> DeleteFile(string path)
        {
            var containerClient = GetBlobContainerClient(path, out var filePath);

            var result = await containerClient.DeleteBlobIfExistsAsync(filePath);

            return result.Value;
        }

        public async Task<Stream> GetFile(string path)
        {
            var containerClient = GetBlobContainerClient(path, out var filePath);
            var blobClient = containerClient.GetBlobClient(filePath);

            var stream = await blobClient.OpenReadAsync(new BlobOpenReadOptions(false));

            return stream;
        }

        public async Task<string> CreateOrUpdateFile(string path, Stream stream)
        {
            var containerClient = GetBlobContainerClient(path, out var filePath);
            var blobClient = containerClient.GetBlobClient(filePath);

            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.AbsolutePath;
        }

        public async Task<bool> Exists(string path)
        {
            var containerClient = GetBlobContainerClient(path, out var filePath);
            var blobClient = containerClient.GetBlobClient(filePath);

            var response = await blobClient.ExistsAsync();

            return response.Value;
        }

        private BlobContainerClient GetBlobContainerClient(string path, out string filePath)
        {
            var containerName = path.Split(Path.DirectorySeparatorChar)[0];
            filePath = Path.Combine(Path.GetDirectoryName(string.Join(Path.DirectorySeparatorChar.ToString(), path.Split(Path.DirectorySeparatorChar).Skip(1))) ?? string.Empty, Path.GetFileName(path));

            return _client.GetBlobContainerClient(containerName);
        }
    }
}
