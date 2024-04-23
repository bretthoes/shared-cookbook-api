using SharedCookbook.Api.Enums;

namespace SharedCookbook.Api.Data.Entities;

public class CookbookInvitation
{
    public required int CookbookInvitationId { get; set; }

    public required int CookbookId { get; set; }

    public int? SenderPersonId { get; set; }

    public int? RecipientPersonId { get; set; }

    public required CookbookInvitationStatus InvitationStatus { get; set; }

    public required DateTime SentDate { get; set; }

    public DateTime? ResponseDate { get; set; }

    public required virtual Cookbook Cookbook { get; set; }

    public virtual Person? Recipient { get; set; }

    public virtual Person? Sender { get; set; }
}
