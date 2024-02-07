namespace SharedCookbookApi.Data.Entities;

public class Cookbook
{
    public required int CookbookId { get; set; }

    public int? CreatorPersonId { get; set; }

    public required string Title { get; set; }

    public string? CoverPattern { get; set; }

    public string? CoverFont { get; set; }

    public string? CoverColor { get; set; }

    public string? CoverAccentColor { get; set; }

    public string? ImagePath { get; set; }

    public virtual Person? Creator { get; set; }

    public virtual ICollection<CookbookInvitation> CookbookInvitations { get; set; } = [];

    public virtual ICollection<CookbookMember> CookbookMembers { get; set; } = [];

    public virtual ICollection<CookbookNotification> CookbookNotifications { get; set; } = [];

    public virtual ICollection<Recipe> Recipes { get; set; } = [];
}
