using SharedCookbook.Api.Data;

namespace SharedCookbook.Api.Services;

public interface ISeedDataService
{
    void Initialize(SharedCookbookContext context);
}
