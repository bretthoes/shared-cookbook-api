using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Repositories
{
    public interface ICookbookInvitationRepository
    {
        Task<List<CookbookInvitation>> GetInvitationsForPerson(int personId);
    }
}