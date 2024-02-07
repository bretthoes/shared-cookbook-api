using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<Person>> PostPerson(RegisterDto registerDto)
    {
        if (registerDto == null)
        {
            return BadRequest();
        }

        var person = await _personRepository.CreatePerson(registerDto);

        return person == null
            ? BadRequest()
            : CreatedAtAction(nameof(GetPerson), new { id = person.PersonId }, person);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(int id)
    {
        var person = await _personRepository.GetPerson(id);

        return person == null
            ? NotFound()
            : Ok(person);
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
        var person = await GetPerson(id);

        if (person == null)
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
    public async Task<ActionResult<Person>> Login(LoginDto loginDto)
    {
        var person = await _personRepository.Login(loginDto);

        return person == null
            ? BadRequest()
            : CreatedAtAction(nameof(GetPerson), new { id = person.PersonId }, person);
    }
}

// TODO refactor to separate folder
public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}