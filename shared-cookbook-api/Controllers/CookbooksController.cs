﻿using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using shared_cookbook_api.Repositories.Interfaces;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CookbooksController(
    ICookbookRepository cookbookRepository,
    IMapper mapper) : ControllerBase
{
    private readonly ICookbookRepository _cookbookRepository = cookbookRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet("{id:int}", Name = nameof(GetCookbook))]
    public ActionResult<Cookbook> GetCookbook(int id)
    {
        var cookbook = _cookbookRepository.GetSingle(id);

        return cookbook == null 
            ? NotFound() 
            : Ok(cookbook);
    }

    [HttpGet("by-person/{personId}", Name = nameof(GetCookbooks))]
    public ActionResult<List<Cookbook>> GetCookbooks(int personId)
    {
        var cookbooks = _cookbookRepository.GetCookbooks(personId);

        return Ok(cookbooks);
    }

    [HttpPost(Name = nameof(AddCookbook))]
    public ActionResult<Cookbook> AddCookbook(Cookbook cookbook)
    {
        if (cookbook == null)
        {
            return BadRequest();
        }

        _cookbookRepository.Add(cookbook);

        if (!_cookbookRepository.Save())
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var newCookbook = _cookbookRepository.GetSingle(cookbook.CookbookId);

        return newCookbook is null
            ? NotFound()
            : CreatedAtAction(
                nameof(GetCookbook),
                new { id = newCookbook.CookbookId },
                newCookbook);
    }

    [HttpPut("{id:int}", Name = nameof(UpdateCookbook))]
    public ActionResult UpdateCookbook(
        int id,
        [FromBody] Cookbook cookbook)
    {
        if (cookbook is null)
        {
            return BadRequest();
        }

        var existingCookbook = _cookbookRepository.GetSingle(id);

        if (existingCookbook is null)
        {
            return NotFound();
        }

        return _cookbookRepository.Save()
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateCookbook))]
    public ActionResult<Cookbook> PartiallyUpdateCookbook(
        int id,
        [FromBody] JsonPatchDocument<Cookbook> patchDoc)
    {
        if (patchDoc is null || patchDoc.Operations.Count == 0)
        {
            return BadRequest();
        }

        var existingCookbook = _cookbookRepository.GetSingle(id);

        if (existingCookbook is null)
        {
            return NotFound();
        }

        try
        {
            patchDoc.ApplyTo(existingCookbook);
        }
        catch (Exception ex)
        {
            return BadRequest($"Invalid patch document: {ex.Message}");
        }

        if (!TryValidateModel(existingCookbook))
        {
            return BadRequest(ModelState);
        }

        var cookbook = _cookbookRepository.Update(existingCookbook);

        return _cookbookRepository.Save()
            ? Ok(cookbook)
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpDelete("{id:int}", Name = nameof(RemoveCookbook))]
    public ActionResult RemoveCookbook(int id)
    {
        var cookbook = _cookbookRepository.GetSingle(id);
        if (cookbook is null)
        {
            return NotFound();
        }

        _cookbookRepository.Delete(id);

        return _cookbookRepository.Save()
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
}
