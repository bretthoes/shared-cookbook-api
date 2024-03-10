using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories;

public interface IPersonRepository
{
    Person? GetSingle(int id);
    Person? GetSingleByEmail(string email);
    void Add(Person person);
    void Delete(int id);
    Person Update(Person person);
    bool Save();
}