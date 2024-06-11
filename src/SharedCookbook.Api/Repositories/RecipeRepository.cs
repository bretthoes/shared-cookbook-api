using Microsoft.EntityFrameworkCore;
using SharedCookbook.Api.Data;
using SharedCookbook.Api.Data.Entities;
using SharedCookbook.Api.Repositories.Interfaces;

namespace SharedCookbook.Api.Repositories;

public class RecipeRepository(SharedCookbookContext context) : IRecipeRepository
{
    private readonly SharedCookbookContext _context = context;

    public Recipe? GetSingle(int id)
    {
        return _context.Recipes.Find(id);
    }

    public Recipe? GetSingleDetailed(int id)
    {
        return _context.Recipes
           .Include(r => r.Nutrition)
           .Include(r => r.IngredientCategories)
           .Include(r => r.RecipeComments)
           .Include(r => r.RecipeDirections)
           .Include(r => r.RecipeIngredients)
           .Include(r => r.RecipeRatings)
           .SingleOrDefault(r => r.RecipeId == id);
    }

    public List<Recipe> GetRecipes(int cookbookId)
    {
        var recipes = _context.Recipes
            .Where(r => r.CookbookId == cookbookId)
            .Include(r => r.Author)
            .Include(r => r.RecipeRatings)
            .ToList();

        return recipes ?? [];
    }

    public void Add(Recipe recipe)
    {
        _context.Recipes.Add(recipe);
    }

    public void Delete(int id)
    {
        var recipe = GetSingle(id);
        if (recipe is not null)
        {
            _context.Recipes.Remove(recipe);
        }
    }

    public Recipe Update(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
        return recipe;
    }

    public bool Save()
    {
        return _context.SaveChanges() >= 0;
    }
}
