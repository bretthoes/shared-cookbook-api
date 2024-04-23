using SharedCookbook.Api.Data.Entities;

namespace SharedCookbook.Api.Repositories.Interfaces;

public interface ICookbookRepository
{
    Cookbook? GetSingle(int id);
    List<Cookbook> GetCookbooks(int personId);
    void Add(Cookbook cookbook, CookbookMember creator);
    void Delete(int id);
    Cookbook Update(Cookbook cookbook);
    bool Save();
}