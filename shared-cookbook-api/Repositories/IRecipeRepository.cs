using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetRecipesInCookbook(int cookbookId);
    Task<Recipe?> GetRecipe(int id);
    Task<bool> UpdateRecipe(int id, Recipe Recipe);
    Task<Recipe> CreateRecipe(Recipe Recipe);
    Task DeleteRecipe(int id);
}
