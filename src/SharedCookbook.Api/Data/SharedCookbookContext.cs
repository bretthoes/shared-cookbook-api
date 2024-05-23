using Microsoft.EntityFrameworkCore;
using SharedCookbook.Api.Data.Entities;
using SharedCookbookApi.Data.Maps;

namespace SharedCookbook.Api.Data;

public partial class SharedCookbookContext : DbContext
{
    public SharedCookbookContext()
    {
    }

    public SharedCookbookContext(DbContextOptions<SharedCookbookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cookbook> Cookbooks { get; set; }

    public virtual DbSet<CookbookInvitation> CookbookInvitations { get; set; }

    public virtual DbSet<CookbookMember> CookbookMembers { get; set; }

    public virtual DbSet<CookbookNotification> CookbookNotifications { get; set; }

    public virtual DbSet<IngredientCategory> IngredientCategories { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeComment> RecipeComments { get; set; }

    public virtual DbSet<RecipeDirection> RecipeDirections { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public virtual DbSet<RecipeRating> RecipeRatings { get; set; }

    public virtual DbSet<RecipeNutrition> RecipeNutrition { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CookbookMap());
        builder.ApplyConfiguration(new CookbookInvitationMap());
        builder.ApplyConfiguration(new CookbookMemberMap());
        builder.ApplyConfiguration(new CookbookNotificationMap());
        builder.ApplyConfiguration(new IngredientCategoryMap());
        builder.ApplyConfiguration(new PersonMap());
        builder.ApplyConfiguration(new RecipeMap());
        builder.ApplyConfiguration(new RecipeCommentMap());
        builder.ApplyConfiguration(new RecipeDirectionMap());
        builder.ApplyConfiguration(new RecipeIngredientMap());
        builder.ApplyConfiguration(new RecipeRatingMap());
        builder.ApplyConfiguration(new RecipeNutritionMap());

    }
}
