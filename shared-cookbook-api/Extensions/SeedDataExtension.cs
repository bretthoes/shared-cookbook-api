using SharedCookbookApi.Data;
using SharedCookbookApi.Services;

namespace SharedCookbookApi.Extensions;

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

