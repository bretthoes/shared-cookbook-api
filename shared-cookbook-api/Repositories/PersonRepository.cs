using SharedCookbookApi.Data;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories;

public class PersonRepository(SharedCookbookContext context) : IPersonRepository
{

    private readonly SharedCookbookContext _context = context;

    public Person? GetSingle(int id)
    {
        return _context.People.Find(id);
    }

    public Person? GetSingleByEmail(string email)
    {
        return _context.People.SingleOrDefault(x => x.Email == email);
    }

    public void Add(Person person)
    {
        _context.People.Add(person);
    }

    public void Delete(int id)
    {
        var person = GetSingle(id);
        if (person != null)
        {
            _context.People.Remove(person);
        }
    }

    public Person Update(Person person)
    {
        _context.People.Update(person);
        return person;
    }

    public bool Save()
    {
        return _context.SaveChanges() >= 0;
    }
}
