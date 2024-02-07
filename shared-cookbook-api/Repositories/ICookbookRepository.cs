using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories;

public interface ICookbookRepository
{
    Task<bool> CreateCookbook(Cookbook cookbook);
    Task<Cookbook?> GetCookbook(int id);
    Task<List<Cookbook>> GetCookbooks(int personId);
    Task<bool> UpdateCookbook(int id, Cookbook cookbook);
    Task<bool> DeleteCookbook(int id);
}
