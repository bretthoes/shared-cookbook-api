using SharedCookbookApi.Data.Entities;

namespace shared_cookbook_api.Repositories.Interfaces;

public interface IRecipeRepository
{
    Recipe? GetSingle(int id);
    List<Recipe> GetRecipes(int cookbookId);
    void Add(Recipe recipe);
    void Delete(int id);
    Recipe Update(Recipe recipe);
    bool Save();
}
