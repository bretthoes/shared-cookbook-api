using Microsoft.AspNetCore.Mvc;
using shared_cookbook_api.Data.Dtos;
using SharedCookbookApi.Data.Entities;
using SharedCookbookApi.Repositories;

namespace SharedCookbookApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly IPersonRepository _personRepository;

    public PeopleController(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PersonDto>> PostPerson(RegisterDto registerDto)
    {
        if (registerDto is null)
        {
            return BadRequest();
        }

        var personDto = await _personRepository.CreatePerson(registerDto);

        return personDto is null
            ? BadRequest()
            : CreatedAtAction(nameof(GetPerson), new { id = personDto.PersonId }, personDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> GetPerson(int id)
    {
        var personDto = await _personRepository.GetPerson(id);

        return personDto is null
            ? NotFound()
            : Ok(personDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(int id, Person person)
    {
        if (id != person.PersonId)
        {
            return BadRequest();
        }

        bool updated = await _personRepository.UpdatePerson(id, person);

        return updated
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var personDto = await GetPerson(id);

        if (personDto is null)
        {
            return NotFound();
        }

        bool deleted = await _personRepository.DeletePerson(id);

        return deleted
            ? NoContent()
            : NotFound();
    }

    // authentication endpoints

    [HttpPost("login")]
    public async Task<ActionResult<PersonDto>> Login(LoginDto loginDto)
    {
        var personDto = await _personRepository.Login(loginDto);

        return personDto is null
            ? BadRequest()
            : CreatedAtAction(nameof(GetPerson), new { id = personDto.PersonId }, personDto);
    }
}
