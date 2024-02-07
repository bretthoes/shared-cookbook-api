using Microsoft.AspNetCore.Mvc;
using SharedCookbookApi.Data.Entities;
using SharedCookbookApi.Repositories;

namespace SharedCookbookApi.Controllers;

public class RecipesController : Controller
{
    private readonly IRecipeRepository _recipeRepository;

    public RecipesController(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetRecipe(int id)
    {
        var recipe = await _recipeRepository.GetRecipe(id);

        if (recipe == null)
        {
            return NotFound();
        }

        return Ok(recipe);
    }

    [HttpGet("person/{personId}/recipes")]
    public async Task<ActionResult<List<Recipe>>> GetRecipesInCookbook(int cookbookId)
    {
        var recipes = await _recipeRepository.GetRecipesInCookbook(cookbookId);

        if (recipes == null || !recipes.Any())
        {
            return NotFound();
        }

        return Ok(recipes);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
    {
        if (id != recipe.RecipeId)
        {
            return BadRequest();
        }

        bool updated = await _recipeRepository.UpdateRecipe(id, recipe);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }


    [HttpPost]
    public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
    {
        await _recipeRepository.CreateRecipe(recipe);
        return CreatedAtAction("GetRecipe", new { id = recipe.RecipeId }, recipe);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var recipe = await GetRecipe(id);

        if (recipe == null)
        {
            return NotFound();
        }

        await _recipeRepository.DeleteRecipe(id);
        return NoContent();
    }
}
