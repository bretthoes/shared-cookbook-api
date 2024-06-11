using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SharedCookbook.Api.Data.Dtos;
using SharedCookbook.Api.Data.Entities;
using SharedCookbook.Api.Repositories.Interfaces;

namespace SharedCookbook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController(
    IRecipeRepository recipeRepository,
    IMapper mapper) : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet("{id:int}", Name = nameof(GetRecipe))]
    public ActionResult<DetailedRecipeDto> GetRecipe(int id)
    {
        var recipeDto = _mapper
            .Map<DetailedRecipeDto>(_recipeRepository.GetSingleDetailed(id));

        return recipeDto is null
            ? NotFound()
            : Ok(recipeDto);
    }

    [HttpGet("by-cookbook/{cookbookId}", Name = nameof(GetRecipes))]
    public ActionResult<List<RecipeDto>> GetRecipes(int cookbookId)
    {
        var recipesDtos = _recipeRepository
            .GetRecipes(cookbookId)
            .Select(_mapper.Map<RecipeDto>)
            .ToList();

        return Ok(recipesDtos);
    }

    [HttpPost(Name = nameof(AddRecipe))]
    public ActionResult<Recipe> AddRecipe(Recipe recipe)
    {
        if (recipe is null)
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
        if (patchDoc?.Operations is null || patchDoc.Operations.Count is 0)
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
