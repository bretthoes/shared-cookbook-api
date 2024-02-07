using SharedCookbookApi.Enums;

namespace SharedCookbookApi.Data.Entities;

public class CookbookNotification
{
    public required int CookbookNotificationId { get; set; }

    public int? SenderPersonId { get; set; }

    public int? CookbookId { get; set; }

    public int? RecipeId { get; set; }

    public required CookbookNotificationActionType ActionType { get; set; }

    public required DateTime Created { get; set; }

    public virtual Cookbook? Cookbook { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual Person? SenderPerson { get; set; }
}
