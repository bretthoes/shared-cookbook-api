using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SharedCookbook.Api.Data.Dtos;
using SharedCookbook.Api.Data.Entities;
using SharedCookbook.Api.Repositories.Interfaces;

namespace SharedCookbook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CookbooksController(
    ICookbookRepository cookbookRepository,
    IMapper mapper,
    IValidator<CreateCookbookDto> validator) : ControllerBase
{
    private readonly ICookbookRepository _cookbookRepository = cookbookRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateCookbookDto> _validator = validator;

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
    public ActionResult<CookbookDto> AddCookbook(CreateCookbookDto cookbookDto)
    {
        var validationResult = _validator.Validate(cookbookDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
        }

        var cookbookToAdd = _mapper.Map<Cookbook>(cookbookDto);
        var creator = GetCookbookCreator(cookbookToAdd);

        _cookbookRepository.Add(cookbookToAdd, creator);

        if (!_cookbookRepository.Save())
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var newCookbookDto = _mapper
            .Map<CookbookDto>(_cookbookRepository.GetSingle(cookbookToAdd.CookbookId));

        return newCookbookDto is null
            ? NotFound()
            : CreatedAtAction(
                nameof(GetCookbook),
                new { id = newCookbookDto.CookbookId },
                newCookbookDto);
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

    // Creates a CookbookMember entity for the creator of a new Cookbook.
    // Contains permissions for all actions by default.
    private static CookbookMember GetCookbookCreator(Cookbook cookbook)
    {
        var creatorId = cookbook.CreatorPersonId ?? 0;

        if (creatorId <= 0)
        {
            // TODO handle error generically than this
            throw new Exception("Cookbook creator does not have a valid Id");
        }

        var creator = new CookbookMember
        {
            CookbookMemberId = 0,
            PersonId = creatorId,
            JoinDate = DateTime.UtcNow,
            CookbookId = cookbook.CookbookId,
            CanAddRecipe = true,
            CanDeleteRecipe = true,
            CanEditCookbookDetails = true,
            CanRemoveMember = true,
            CanSendInvite = true,
            CanUpdateRecipe = true,
            Cookbook = cookbook,
        };

        return creator;
    }
}
