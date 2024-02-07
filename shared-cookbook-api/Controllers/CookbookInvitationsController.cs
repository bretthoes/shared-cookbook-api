using Microsoft.AspNetCore.Mvc;
using SharedCookbookApi.Data.Entities;
using SharedCookbookApi.Repositories;

namespace SharedCookbookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CookbookInvitationsController : ControllerBase
{
    private readonly ICookbookInvitationRepository _invitationRepository;

    public CookbookInvitationsController(ICookbookInvitationRepository invitationRepository)
    {
        _invitationRepository = invitationRepository;
    }


   [HttpGet("person/{personId}/invitations")]
    public async Task<ActionResult<List<CookbookInvitation>>> GetInvitations(int personId)
    {
        var invitations = await _invitationRepository.GetInvitationsForPerson(personId);

        if (invitations == null || !invitations.Any())
        {
            return NotFound("No invitations found for the user.");
        }

        return Ok(invitations);
    }
}
