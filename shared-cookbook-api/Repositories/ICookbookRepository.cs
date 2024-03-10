using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories;

public interface ICookbookRepository
{
    Cookbook? GetSingle(int id);

    List<Cookbook> GetCookbooks(int personId);
    void Add(Cookbook cookbook);
    void Delete(int id);
    Cookbook Update(Cookbook cookbook);
    bool Save();
}