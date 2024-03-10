using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories;

public interface IRecipeRepository
{
    Recipe? GetSingle(int id);
    List<Recipe> GetRecipes(int cookbookId);
    void Add(Recipe recipe);
    void Delete(int id);
    Recipe Update(Recipe recipe);
    bool Save();
}
