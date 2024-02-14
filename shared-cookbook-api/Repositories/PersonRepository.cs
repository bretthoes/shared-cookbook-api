using AutoMapper;
using Microsoft.EntityFrameworkCore;
using shared_cookbook_api.Data.Dtos;
using SharedCookbookApi.Data;
using SharedCookbookApi.Data.Entities;
using SharedCookbookApi.Services;

namespace SharedCookbookApi.Repositories;

public class PersonRepository : IPersonRepository
{

    private readonly SharedCookbookContext _context;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public PersonRepository(SharedCookbookContext context, IAuthService authService, IMapper mapper)
    {
        _context = context;
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<PersonDto?> CreatePerson(RegisterDto registerDto)
    {
        var person = new Person
        {
            PersonId = 0,
            Email = registerDto.Email,
            DisplayName = registerDto.Email,
            PasswordHash = _authService.HashPassword(registerDto.Password)
    };

        _context.People.Add(person);
        return await _context.SaveChangesAsync() >= 0
            ? _mapper.Map<PersonDto>(person)
            : null;
    }

    public async Task<PersonDto?> GetPerson(int id)
    {
        var person = await _context.People.FindAsync(id);

        return person is null
            ? null
            : _mapper.Map<PersonDto>(person);
    }

    public async Task<bool> UpdatePerson(int id, Person person)
    {
        if (id != person.PersonId)
        {
            throw new ArgumentException("ID mismatch");
        }

        _context.Entry(person).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PersonExists(id))
            {
                throw new KeyNotFoundException("Person not found");
            }
            else
            {
                throw;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeletePerson(int id)
    {
        var person = await _context.People.FindAsync(id) 
            ?? throw new KeyNotFoundException("Person not found");
        _context.People.Remove(person);
        return await _context.SaveChangesAsync() >= 0;
    }
    
    // authentication endpoints
    public async Task<PersonDto?> Login(LoginDto loginDto)
    {
        var person = await GetPerson(loginDto.Email);

        if (person is null)
        {
            // TODO if Email not found; return bad request
            return null;
        }

        var result = _authService.ValidatePassword(
            loginDto.Password,
            person.PasswordHash ?? string.Empty);
        // TODO if result false, return unauthorized
        return result
            ? _mapper.Map<PersonDto>(person)
            : null;
    }

    private bool PersonExists(int id)
    {
        return _context.People.Any(e => e.PersonId == id);
    }

    private async Task<Person?> GetPerson(string email)
    {
           return await _context.People.SingleOrDefaultAsync(p => p.Email == email);
    }
}
