using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SharedCookbook.Api.Data.Dtos;
using SharedCookbook.Api.Data.Entities;
using SharedCookbook.Api.Repositories.Interfaces;
using SharedCookbook.Api.Services;

namespace SharedCookbook.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeopleController(
    IPersonRepository personRepository,
    IAuthService authService,
    IMapper mapper,
    IValidator<AuthenticationDto> validator) : ControllerBase
{
    private readonly IPersonRepository _personRepository = personRepository;
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<AuthenticationDto> _validator = validator;

    [HttpGet("{id:int}", Name = nameof(GetPerson))]
    public ActionResult<PersonDto> GetPerson(int id)
    {
        var person = _personRepository.GetSingle(id);

        return person is null
            ? NotFound()
            : Ok(_mapper.Map<PersonDto>(person));
    }

    [HttpGet("by-email/{email}", Name = nameof(GetPersonByEmail))]
    public ActionResult<PersonDto> GetPersonByEmail(string email)
    {
        var person = _personRepository.GetSingleByEmail(email);

        return person is null
            ? NotFound()
            : Ok(_mapper.Map<PersonDto>(person));
    }

    [HttpPost(Name = nameof(AddPerson))]
    public ActionResult<PersonDto> AddPerson(CreatePersonDto registerDto)
    {
        var validationResult = _validator.Validate(registerDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
        }

        var person = _personRepository.GetSingleByEmail(registerDto.Email);
        if (person is not null)
        {
            return Conflict();
        }

        var personToAdd = _mapper.Map<Person>(registerDto);
        personToAdd.PasswordHash = _authService.HashPassword(registerDto.Password);

        _personRepository.Add(personToAdd);


        if (!_personRepository.Save())
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var newPersonDto = _mapper
            .Map<PersonDto>(_personRepository.GetSingle(personToAdd.PersonId));

        return newPersonDto is null
            ? NotFound()
            : CreatedAtAction(
                nameof(GetPerson),
                new { id = newPersonDto.PersonId },
                newPersonDto);
    }

    [HttpPut("{id:int}", Name = nameof(UpdatePerson))]
    public ActionResult<PersonDto> UpdatePerson(
        int id,
        [FromBody] UpdatePersonDto updatePersonDto)
    {
        if (updatePersonDto is null)
        {
            return BadRequest();
        }

        var existingPerson = _personRepository.GetSingle(id);

        if (existingPerson is null)
        {
            return NotFound();
        }

        _mapper.Map(updatePersonDto, existingPerson);
        _personRepository.Update(existingPerson);

        return _personRepository.Save()
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPatch("{id:int}", Name = nameof(PartiallyUpdatePerson))]
    public ActionResult<PersonDto> PartiallyUpdatePerson(
        int id,
        [FromBody] JsonPatchDocument<UpdatePersonDto> patchDoc)
    {
        if (patchDoc is null || patchDoc.Operations.Count == 0)
        {
            return BadRequest();
        }

        var existingPerson = _personRepository.GetSingle(id);

        if (existingPerson is null)
        {
            return NotFound();
        }

        var updatePersonDto = _mapper.Map<UpdatePersonDto>(existingPerson);

        try
        {
            patchDoc.ApplyTo(updatePersonDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Invalid patch document: {ex.Message}");
        }

        if (!TryValidateModel(updatePersonDto))
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(updatePersonDto, existingPerson);

        var person = _personRepository.Update(existingPerson);

        if (person is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var personDto = _mapper.Map<PersonDto>(person);


        return _personRepository.Save()
            ? Ok(personDto)
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpDelete("{id:int}", Name = nameof(RemovePerson))]
    public ActionResult RemovePerson(int id)
    {
        var person = _personRepository.GetSingle(id);
        if (person is null)
        {
            return NotFound();
        }

        _personRepository.Delete(id);

        return _personRepository.Save()
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPost("login", Name = nameof(Login))]
    public ActionResult<PersonDto> Login([FromBody] LoginDto loginDto)
    {
        var validationResult = _validator.Validate(loginDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
        }

        var person = _personRepository.GetSingleByEmail(loginDto.Email);
        if (person is null)
        {
            return NotFound();
        }

        return _authService.VerifyPassword(loginDto.Password, person.PasswordHash)
            ? Ok(_mapper.Map<PersonDto>(person))
            : Unauthorized();
    }
}