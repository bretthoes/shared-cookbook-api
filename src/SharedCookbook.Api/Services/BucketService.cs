using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using SharedCookbook.Api.Data.Options;

namespace SharedCookbook.Api.Services;

public class BucketService(
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

            var uniqueFileName = Guid.NewGuid().ToString() + fileToUpload.FileName;

            using var newMemoryStream = new MemoryStream();
            fileToUpload.CopyTo(newMemoryStream);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = uniqueFileName,
                BucketName = options.Value.BucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            // TODO inject client into service
            var credentials = new BasicAWSCredentials(options.Value.AwsAccessKeyId, options.Value.AwsSecretAccessKey);
            using var client = new AmazonS3Client(credentials, RegionEndpoint.USEast2);

            using var fileTransferUtility = new TransferUtility(client);
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