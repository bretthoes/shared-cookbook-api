using SharedCookbookApi.Data;

namespace SharedCookbookApi.Services;

public interface ISeedDataService
{
    void Initialize(SharedCookbookContext context);
}
