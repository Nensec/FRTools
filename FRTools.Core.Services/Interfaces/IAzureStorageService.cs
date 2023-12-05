namespace FRTools.Core.Services.Interfaces
{
    public interface IAzureStorageService
    {
        Task<string> CreateFile(string path, Stream stream);
        Task<bool> DeleteFile(string path);
        Task<bool> Exists(string path);
        Task<Stream> GetFile(string path);
    }
}