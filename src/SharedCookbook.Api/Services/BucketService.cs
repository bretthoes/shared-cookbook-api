using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using SharedCookbook.Api.Data.Options;

namespace SharedCookbook.Api.Services;

public class BucketService(IAmazonS3 client,
    IOptions<BucketOptions> options,
    ILogger<BucketService> logger) : IBucketService
{
    public async Task<string> UploadFile(IFormFile fileToUpload)
    {
        try 
        {
            if (fileToUpload == null || fileToUpload.Length == 0)
            {
                throw new ArgumentException("File to upload cannot be null or empty.");
            }

            var uniqueFileName = Guid.NewGuid().ToString();

            using var newMemoryStream = new MemoryStream();
            fileToUpload.CopyTo(newMemoryStream);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = uniqueFileName,
                BucketName = options.Value.BucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);

            return uniqueFileName;
        }
        catch (Exception e) 
        {
            logger.LogError(e, "{Message}", e.Message);
            return string.Empty;
        }
    }
}