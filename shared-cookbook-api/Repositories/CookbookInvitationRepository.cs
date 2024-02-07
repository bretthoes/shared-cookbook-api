using Microsoft.EntityFrameworkCore;
using SharedCookbookApi.Data;
using SharedCookbookApi.Data.Entities;
using SharedCookbookApi.Enums;

namespace SharedCookbookApi.Repositories
{
    public class CookbookInvitationRepository : ICookbookInvitationRepository
    {
        private readonly SharedCookbookContext _context;

        public CookbookInvitationRepository(SharedCookbookContext context)
        {
            _context = context;
        }

        public async Task<List<CookbookInvitation>> GetInvitationsForPerson(int personId)
        {
            var invitations = await _context.CookbookInvitations
                .Where(ci => ci.RecipientPersonId == personId
                    && ci.InvitationStatus == CookbookInvitationStatus.Sent)
                .OrderByDescending(ci => ci.SentDate)
                .ToListAsync();

            return invitations ?? [];
        }
    }
}
