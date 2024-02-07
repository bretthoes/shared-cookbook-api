using Microsoft.EntityFrameworkCore;
using SharedCookbookApi.Data;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly SharedCookbookContext _context;

    public RecipeRepository(SharedCookbookContext context)
    {
        _context = context;
    }

    public async Task<Recipe> CreateRecipe(Recipe Recipe)
    {
        _context.Recipes.Add(Recipe);
        await _context.SaveChangesAsync();
        return Recipe;
    }

    public async Task<List<Recipe>> GetRecipesInCookbook(int cookbookId)
    {
        var recipes = await _context.Recipes
            .Where(r => r.CookbookId == cookbookId)
            .OrderBy(r => r.Title)
            .ToListAsync();

        return recipes ?? [];
    }

    public async Task<Recipe?> GetRecipe(int id)
    {
        return await _context.Recipes.FindAsync(id);
    }

    public async Task<bool> UpdateRecipe(int id, Recipe Recipe)
    {
        if (id != Recipe.RecipeId)
        {
            return false;
        }

        _context.Entry(Recipe).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RecipeExists(id))
            {
                throw new KeyNotFoundException("Recipe not found");
            }
            else
            {
                throw;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task DeleteRecipe(int id)
    {
        var Recipe = await _context.Recipes.FindAsync(id);
        if (Recipe == null)
        {
            throw new KeyNotFoundException("Recipe not found");
        }

        _context.Recipes.Remove(Recipe);
        await _context.SaveChangesAsync();
    }

    private bool RecipeExists(int id)
    {
        return _context.Recipes.Any(e => e.RecipeId == id);
    }
}
