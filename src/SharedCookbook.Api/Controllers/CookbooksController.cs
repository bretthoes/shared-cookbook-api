using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedCookbook.Api.Data.Dtos;
using SharedCookbook.Api.Data.Entities;
using SharedCookbook.Api.Repositories.Interfaces;

namespace SharedCookbook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CookbooksController(
    ICookbookRepository cookbookRepository,
    IWebHostEnvironment hostingEnvironment,
    IMapper mapper,
    IValidator<CreateCookbookDto> validator,
    ILogger<CookbooksController> logger) : ControllerBase
{
    private readonly ICookbookRepository _cookbookRepository = cookbookRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateCookbookDto> _validator = validator;
    private readonly ILogger <CookbooksController> _logger = logger;

    [HttpGet("{id:int}", Name = nameof(GetCookbook))]
    public ActionResult<CookbookDto> GetCookbook(int id)
    {
        var cookbookDto = _mapper
            .Map<CookbookDto>(_cookbookRepository.GetSingle(id));

        return cookbookDto is null
            ? NotFound()
            : Ok(cookbookDto);
    }

    [HttpGet("by-person/{personId}", Name = nameof(GetCookbooks))]
    public ActionResult<List<CookbookDto>> GetCookbooks(int personId)
    {
        var cookbookDtos = _cookbookRepository
            .GetCookbooks(personId)
            .Select(_mapper.Map<CookbookDto>)
            .ToList();

        return Ok(cookbookDtos);
    }

    [HttpPost(Name = nameof(AddCookbook))]
    public ActionResult<CookbookDto> AddCookbook([FromForm] CreateCookbookDto cookbookDto)
    {
        var validationResult = _validator.Validate(cookbookDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
        }

        // Save the cover image file
        var coverImagePath = "";
        if (cookbookDto.Cover != null)
        {
            var uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + cookbookDto.Cover.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                cookbookDto.Cover.CopyTo(fileStream);
            }

            coverImagePath = "/uploads/" + uniqueFileName;
        }

        var cookbookToAdd = _mapper.Map<Cookbook>(cookbookDto);
        cookbookToAdd.ImagePath = coverImagePath;

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
    public ActionResult<CookbookDto> PartiallyUpdateCookbook(
        int id,
        [FromBody] JsonPatchDocument<CookbookDto> patchDoc)
    {
        if (patchDoc?.Operations is null || patchDoc.Operations.Count is 0)
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
            patchDoc.ApplyTo(_mapper.Map<CookbookDto>(existingCookbook));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
            return BadRequest();
        }

        if (!TryValidateModel(existingCookbook))
        {
            return BadRequest(ModelState);
        }

        var cookbook = _cookbookRepository.Update(existingCookbook);

        return cookbook is not null && _cookbookRepository.Save()
            ? Ok(_mapper.Map<CookbookDto>(cookbook))
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
    private CookbookMember GetCookbookCreator(Cookbook cookbook)
    {
        var creatorId = cookbook.CreatorPersonId ?? 0;

        if (creatorId is 0 or -1)
        {
            _logger.LogError("New cookbook contained invalid creatorId: {cookbook}", JsonConvert.SerializeObject(cookbook));
            throw new ArgumentException("New cookbook contained invalid creatorId.");
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
