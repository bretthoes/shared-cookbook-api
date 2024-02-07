using SharedCookbookApi.Controllers;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories;

public interface IPersonRepository
{
    Task<Person?> CreatePerson(RegisterDto registerDto);
    Task<Person?> GetPerson(int id);
    Task<bool> UpdatePerson(int id, Person person);
    Task<bool> DeletePerson(int id);
    Task<Person?> Login(LoginDto loginDto);
}
