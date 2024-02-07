using Microsoft.AspNetCore.Mvc;
using SharedCookbookApi.Data.Entities;
using SharedCookbookApi.Repositories;

namespace SharedCookbookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CookbooksController : ControllerBase
{
    private readonly ICookbookRepository _cookbookRepository;

    public CookbooksController(ICookbookRepository cookbookRepository)
    {
        _cookbookRepository = cookbookRepository;
    }

    [HttpPost]
    public async Task<ActionResult<Cookbook>> CreateCookbook(Cookbook cookbook)
    {
        if (cookbook == null) 
        {
            return BadRequest();
        }

        bool created = await _cookbookRepository.CreateCookbook(cookbook);

        return created 
            ? CreatedAtAction(
                nameof(GetCookbook), 
                new { id = cookbook.CookbookId }, 
                cookbook)
            : BadRequest();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cookbook>> GetCookbook(int id)
    {
        var cookbook = await _cookbookRepository.GetCookbook(id);

        return cookbook == null 
            ? NotFound() 
            : Ok(cookbook);
    }

    [HttpGet("person/{personId}")]
    public async Task<ActionResult<List<Cookbook>>> ListCookbooks(int personId)
    {
        var cookbooks = await _cookbookRepository.GetCookbooks(personId);

        return cookbooks.Count == 0
            ? NotFound()
            : Ok(cookbooks);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCookbook(int id, Cookbook cookbook)
    {
        if (id != cookbook.CookbookId)
        {
            return BadRequest();
        }

        bool updated = await _cookbookRepository.UpdateCookbook(id, cookbook);

        return updated 
            ? NoContent() 
            : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCookbook(int id)
    {
        var cookbook = await GetCookbook(id);

        if (cookbook == null)
        {
            return NotFound();
        }

        bool deleted = await _cookbookRepository.DeleteCookbook(id);

        return deleted 
            ? NoContent() 
            : NotFound();
    }
}
