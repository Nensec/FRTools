using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace FRTools.Common
{
    public class AzureImageService
    {
        public async Task DeleteImage(string path)
        {
            var container = GetStorageContainer(path);

            var fileName = Path.GetFileName(path);
            var reference = container.GetBlockBlobReference(fileName);
            if (reference.Exists())
            {
                await reference.DeleteAsync().ConfigureAwait(false);
            }
        }

        public async Task<Stream> GetImage(string path, bool cache = true)
        {
            var resultStream = new MemoryStream();

            var directory = GetStorageContainer(path);

            var fileName = Path.GetFileName(path);

            var reference = directory.GetBlockBlobReference(fileName);

            if (reference.Exists())
            {
                await reference.DownloadToStreamAsync(resultStream).ConfigureAwait(false);
                resultStream.Position = 0;
                return resultStream;
            }
            throw new FileNotFoundException($"'{path}' was not found");
        }

        public async Task<string> WriteImage(string path, Stream stream)
        {
            var directory = GetStorageContainer(path);

            var fileName = Path.GetFileName(path);
            var reference = directory.GetBlockBlobReference(fileName);
            reference.Properties.ContentType = MimeMapping.GetMimeMapping(fileName);

            await reference.UploadFromStreamAsync(stream).ConfigureAwait(false);

            return WebUtility.UrlDecode(reference.Uri.AbsolutePath);
        }

        public async Task<string> Exists(string path)
        {
            var directory = GetStorageContainer(path);

            var fileName = Path.GetFileName(path);
            var reference = directory.GetBlockBlobReference(fileName);
            return await reference.ExistsAsync().ConfigureAwait(false) ? WebUtility.UrlDecode(reference.Uri.AbsolutePath) : null;
        }

        private CloudBlobDirectory GetStorageContainer(string path)
        {
            var credentials = ConfigurationManager.AppSettings["AzureCredentials"];

            var storageAccount = CloudStorageAccount.Parse(credentials);
            var blobClient = storageAccount.CreateCloudBlobClient();

            var directoryPath = Path.GetDirectoryName(string.Join(Path.DirectorySeparatorChar.ToString(), path.Split(Path.DirectorySeparatorChar).Skip(1)));

            var container = blobClient.GetContainerReference(path.Split(Path.DirectorySeparatorChar)[0]);
            return container.GetDirectoryReference(directoryPath);
        }
    }
}
