﻿using shared_cookbook_api.Repositories.Interfaces;
using SharedCookbookApi.Data;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories;

public class RecipeRepository(SharedCookbookContext context) : IRecipeRepository
{
    private readonly SharedCookbookContext _context = context;

    public Recipe? GetSingle(int id)
    {
        return _context.Recipes.Find(id);
    }

    public List<Recipe> GetRecipes(int cookbookId)
    {
        var recipes = _context.Recipes
            .Where(r => r.CookbookId == cookbookId)
            .OrderBy(r => r.Title)
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
        if (recipe != null)
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
