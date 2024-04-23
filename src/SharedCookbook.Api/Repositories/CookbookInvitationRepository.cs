using SharedCookbook.Api.Data;
using SharedCookbook.Api.Data.Entities;
using SharedCookbook.Api.Enums;
using SharedCookbook.Api.Repositories.Interfaces;

namespace SharedCookbook.Api.Repositories;

public class CookbookInvitationRepository(SharedCookbookContext context) : ICookbookInvitationRepository
{
    private readonly SharedCookbookContext _context = context;

    public CookbookInvitation? GetSingle(int id)
    {
        return _context.CookbookInvitations.Find(id);
    }

    public List<CookbookInvitation> GetInvitations(int personId)
    {
        var invitations = _context.CookbookInvitations
            .Where(ci => ci.RecipientPersonId == personId
                && ci.InvitationStatus == CookbookInvitationStatus.Sent)
            .OrderByDescending(ci => ci.SentDate)
            .ToList();

        return invitations ?? [];
    }

    public void Add(CookbookInvitation invitation)
    {
        _context.CookbookInvitations.Add(invitation);
    }

    public void Delete(int id)
    {
        var invitation = GetSingle(id);
        if (invitation != null)
        {
            _context.CookbookInvitations.Remove(invitation);
        }
    }

    public CookbookInvitation Update(CookbookInvitation invitation)
    {
        _context.CookbookInvitations.Update(invitation);
        return invitation;
    }

    public bool Save()
    {
        return _context.SaveChanges() >= 0;
    }
}