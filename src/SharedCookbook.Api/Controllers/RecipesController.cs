using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using shared_cookbook_api.Repositories.Interfaces;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Controllers;

public class RecipesController(
    IRecipeRepository recipeRepository,
    IMapper mapper) : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet("{id:int}", Name = nameof(GetRecipe))]
    public ActionResult<Cookbook> GetRecipe(int id)
    {
        var recipe = _recipeRepository.GetSingle(id);

        return recipe == null
            ? NotFound()
            : Ok(recipe);
    }

    [HttpGet("by-cookbook/{cookbookId}", Name = nameof(GetRecipes))]
    public ActionResult<List<Cookbook>> GetRecipes(int cookbookId)
    {
        var recipes = _recipeRepository.GetRecipes(cookbookId);

        return Ok(recipes);
    }

    [HttpPost(Name = nameof(AddRecipe))]
    public ActionResult<Recipe> AddRecipe(Recipe recipe)
    {
        if (recipe == null)
        {
            return BadRequest();
        }

        _recipeRepository.Add(recipe);

        if (!_recipeRepository.Save())
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var newRecipe = _recipeRepository.GetSingle(recipe.RecipeId);

        return newRecipe is null
            ? NotFound()
            : CreatedAtAction(
                nameof(GetRecipe),
                new { id = newRecipe.RecipeId },
                newRecipe);
    }

    [HttpPut("{id:int}", Name = nameof(UpdateRecipe))]
    public ActionResult UpdateRecipe(
        int id,
        [FromBody] Recipe recipe)
    {
        if (recipe is null)
        {
            return BadRequest();
        }

        var existingRecipe = _recipeRepository.GetSingle(id);

        if (existingRecipe is null)
        {
            return NotFound();
        }

        return _recipeRepository.Save()
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateRecipe))]
    public ActionResult<Recipe> PartiallyUpdateRecipe(
        int id,
        [FromBody] JsonPatchDocument<Recipe> patchDoc)
    {
        if (patchDoc is null || patchDoc.Operations.Count == 0)
        {
            return BadRequest();
        }

        var existingRecipe = _recipeRepository.GetSingle(id);

        if (existingRecipe is null)
        {
            return NotFound();
        }

        try
        {
            patchDoc.ApplyTo(existingRecipe);
        }
        catch (Exception ex)
        {
            return BadRequest($"Invalid patch document: {ex.Message}");
        }

        if (!TryValidateModel(existingRecipe))
        {
            return BadRequest(ModelState);
        }

        var recipe = _recipeRepository.Update(existingRecipe);

        return _recipeRepository.Save()
            ? Ok(recipe)
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpDelete("{id:int}", Name = nameof(RemoveRecipe))]
    public ActionResult RemoveRecipe(int id)
    {
        var recipe = _recipeRepository.GetSingle(id);
        if (recipe is null)
        {
            return NotFound();
        }

        _recipeRepository.Delete(id);

        return _recipeRepository.Save()
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
}
