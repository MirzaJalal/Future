namespace Bangla.Services.ProductAPI.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string blobUrl);
    }
}
