﻿namespace SharedCookbook.Api.Data.Entities;

public class Person
{
    public required int PersonId { get; set; }

    public string? DisplayName { get; set; }

    public required string Email { get; set; }

    public string? ImagePath { get; set; }

    public required string PasswordHash { get; set; }

    public virtual ICollection<Cookbook> Cookbooks { get; set; } = [];

    public virtual ICollection<CookbookMember> CookbookMemberships { get; set; } = [];

    public virtual ICollection<CookbookNotification> CookbookNotifications { get; set; } = [];

    public virtual ICollection<CookbookInvitation> SentInvitations { get; set; } = [];

    public virtual ICollection<CookbookInvitation> ReceivedInvitations { get; set; } = [];

    public virtual ICollection<Recipe> Recipes { get; set; } = [];

    public virtual ICollection<RecipeComment> RecipeComments { get; set; } = [];

    public virtual ICollection<RecipeRating> RecipeRatings { get; set; } = [];
}
