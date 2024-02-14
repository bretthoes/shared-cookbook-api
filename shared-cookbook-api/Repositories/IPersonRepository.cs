using shared_cookbook_api.Data.Dtos;
using SharedCookbookApi.Controllers;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories;

public interface IPersonRepository
{
    Task<PersonDto?> CreatePerson(RegisterDto registerDto);
    Task<PersonDto?> GetPerson(int id);
    Task<bool> UpdatePerson(int id, Person person);
    Task<bool> DeletePerson(int id);
    Task<PersonDto?> Login(LoginDto loginDto);
}
