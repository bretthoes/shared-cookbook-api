using SharedCookbook.Api.Data;
using SharedCookbook.Api.Services;

namespace SharedCookbook.Api.Extensions;

public static class SeedDataExtension
{
    public static void SeedData(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SharedCookbookContext>();
            var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();

            seedDataService.Initialize(dbContext);
            dbContext.SaveChanges();
        }
    }
}

