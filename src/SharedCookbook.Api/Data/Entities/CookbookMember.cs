namespace SharedCookbook.Api.Data.Entities;

public class CookbookMember
{
    public required int CookbookMemberId { get; set; }

    public required int PersonId { get; set; }

    public required int CookbookId { get; set; }

    public required bool CanAddRecipe { get; set; }

    public required bool CanUpdateRecipe { get; set; }

    public required bool CanDeleteRecipe { get; set; }

    public required bool CanSendInvite { get; set; }

    public required bool CanRemoveMember { get; set; }

    public required bool CanEditCookbookDetails { get; set; }

    public required DateTime JoinDate { get; set; }

    public required virtual Cookbook Cookbook { get; set; }

    public virtual Person? Person { get; set; }
}
