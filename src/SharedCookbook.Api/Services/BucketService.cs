using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using SharedCookbook.Api.Data.Options;

namespace SharedCookbook.Api.Services;

public class BucketService(IAmazonS3 client, IOptions<BucketOptions> options) : IBucketService
{
    public async Task UploadFile(IFormFile fileToUpload)
    {
        if (fileToUpload == null || fileToUpload.Length == 0)
        {
            throw new ArgumentException("File to upload cannot be null or empty.");
        }

        var keyName = fileToUpload.FileName;

        using var newMemoryStream = new MemoryStream();
        fileToUpload.CopyTo(newMemoryStream);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = keyName,
            BucketName = options.Value.BucketName,
            CannedACL = S3CannedACL.PublicRead
        };

        var fileTransferUtility = new TransferUtility(client);
        await fileTransferUtility.UploadAsync(uploadRequest);
    }
}