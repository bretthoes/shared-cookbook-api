using Microsoft.EntityFrameworkCore;
using SharedCookbookApi.Data;
using SharedCookbookApi.Data.Entities;
using System.Data;

namespace SharedCookbookApi.Repositories;

public class CookbookRepository : ICookbookRepository
{
    private readonly SharedCookbookContext _context;

    public CookbookRepository(SharedCookbookContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateCookbook(Cookbook cookbook)
    {
        _context.Cookbooks.Add(cookbook);
        return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<Cookbook?> GetCookbook(int id)
    {
        return await _context.Cookbooks.FindAsync(id);  
    }
      
    public async Task<List<Cookbook>> GetCookbooks(int personId)
    {
        var cookbooks = await _context.Cookbooks
            .Where(c => _context.CookbookMembers
            .Any(cm => cm.PersonId == personId
                && cm.CookbookId == c.CookbookId))
            .ToListAsync();

        return cookbooks ?? [];
    }

    public async Task<bool> UpdateCookbook(int id, Cookbook cookbook)
    {   
        if (id != cookbook.CookbookId)
        {
            throw new ArgumentException("ID mismatch");
        }

        _context.Entry(cookbook).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CookbookExists(id))
            {
                throw new KeyNotFoundException("Cookbook not found");
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

    public async Task<bool> DeleteCookbook(int id)
    {
        var cookbook = await _context.Cookbooks.FindAsync(id) 
            ?? throw new KeyNotFoundException("Cookbook not found");
        _context.Cookbooks.Remove(cookbook);
        return await _context.SaveChangesAsync() > 0;
    }

    private bool CookbookExists(int id)
    {
        return _context.Cookbooks.Any(e => e.CookbookId == id);
    }
}
