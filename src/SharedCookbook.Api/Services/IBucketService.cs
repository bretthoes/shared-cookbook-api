namespace SharedCookbook.Api.Services;

public interface IBucketService
{
    Task<string> UploadFile(IFormFile fileToUpload);
}