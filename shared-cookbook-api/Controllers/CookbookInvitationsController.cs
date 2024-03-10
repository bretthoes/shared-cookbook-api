using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using shared_cookbook_api.Repositories.Interfaces;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CookbookInvitationsController(
    ICookbookInvitationRepository invitationRepository,
    IMapper mapper) : ControllerBase
{
    private readonly ICookbookInvitationRepository _invitationRepository = invitationRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet("{id:int}", Name = nameof(GetInvitation))]
    public ActionResult<CookbookInvitation> GetInvitation(int id)
    {
        var invitation = _invitationRepository.GetSingle(id);

        return invitation == null
            ? NotFound()
            : Ok(invitation);
    }

    [HttpGet("by-person/{personId}", Name = nameof(GetInvitations))]
    public ActionResult<List<CookbookInvitation>> GetInvitations(int personId)
    {
        var invitations = _invitationRepository.GetInvitations(personId);

        return invitations.Count == 0
            ? NotFound()
            : Ok(invitations);
    }

    [HttpPost(Name = nameof(AddInvitation))]
    public ActionResult<CookbookInvitation> AddInvitation(CookbookInvitation invitation)
    {
        if (invitation == null)
        {
            return BadRequest();
        }

        _invitationRepository.Add(invitation);

        if (!_invitationRepository.Save())
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var newInvitation = _invitationRepository.GetSingle(invitation.CookbookInvitationId);

        return newInvitation is null
            ? NotFound()
            : CreatedAtAction(
                nameof(GetInvitation),
                new { id = newInvitation.CookbookId },
                newInvitation);
    }

    [HttpPut("{id:int}", Name = nameof(UpdateInvitation))]
    public ActionResult UpdateInvitation(
        int id,
        [FromBody] CookbookInvitation invitation)
    {
        if (invitation is null)
        {
            return BadRequest();
        }

        var existingInvitation = _invitationRepository.GetSingle(id);

        if (existingInvitation is null)
        {
            return NotFound();
        }

        return _invitationRepository.Save()
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateInvitation))]
    public ActionResult<CookbookInvitation> PartiallyUpdateInvitation(
        int id,
        [FromBody] JsonPatchDocument<CookbookInvitation> patchDoc)
    {
        if (patchDoc is null)
        {
            return BadRequest();
        }

        var existingInvitation = _invitationRepository.GetSingle(id);

        if (existingInvitation is null)
        {
            return NotFound();
        }

        patchDoc.ApplyTo(existingInvitation);

        if (!TryValidateModel(existingInvitation))
        {
            return BadRequest(ModelState);
        }

        _invitationRepository.Update(existingInvitation);

        return _invitationRepository.Save()
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpDelete("{id:int}", Name = nameof(RemoveInvitation))]
    public ActionResult RemoveInvitation(int id)
    {
        var invitation = _invitationRepository.GetSingle(id);
        if (invitation is null)
        {
            return NotFound();
        }

        _invitationRepository.Delete(id);

        return _invitationRepository.Save()
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
}
