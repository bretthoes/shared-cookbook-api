namespace SharedCookbook.Api.Data.Options;

public class BucketOptions
{
    public required string BucketName { get; init; }
    public required string AwsAccessKeyId { get; init; }
    public required string AwsSecretAccessKey { get; init; }
    public required string Region { get; init; }
    public required string ToolkitArtifactGuid { get; init; }
}
