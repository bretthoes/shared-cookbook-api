using SharedCookbookApi.Data.Entities;

namespace shared_cookbook_api.Repositories.Interfaces;

public interface ICookbookRepository
{
    Cookbook? GetSingle(int id);
    List<Cookbook> GetCookbooks(int personId);
    void Add(Cookbook cookbook, CookbookMember creator);
    void Delete(int id);
    Cookbook Update(Cookbook cookbook);
    bool Save();
}