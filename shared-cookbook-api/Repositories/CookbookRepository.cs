using shared_cookbook_api.Repositories.Interfaces;
using SharedCookbookApi.Data;
using SharedCookbookApi.Data.Entities;
using System.Data;

namespace SharedCookbookApi.Repositories;

public class CookbookRepository(SharedCookbookContext context) : ICookbookRepository
{
    private readonly SharedCookbookContext _context = context;

    public Cookbook? GetSingle(int id)
    {
        return _context.Cookbooks.Find(id);
    }

    public List<Cookbook> GetCookbooks(int personId)
    {
        var cookbooks = _context.Cookbooks
            .Where(c => _context.CookbookMembers
                .Any(cm => cm.PersonId == personId
                      && cm.CookbookId == c.CookbookId))
            .ToList();

        return cookbooks ?? [];
    }

    public void Add(Cookbook cookbook)
    {
        _context.Cookbooks.Add(cookbook);
    }

    public void Delete(int id)
    {
        var cookbook = GetSingle(id);
        if (cookbook != null)
        {
            _context.Cookbooks.Remove(cookbook);
        }
    }

    public Cookbook Update(Cookbook cookbook)
    {
        _context.Cookbooks.Update(cookbook);
        return cookbook;
    }

    public bool Save()
    {
        return _context.SaveChanges() >= 0;
    }
}
