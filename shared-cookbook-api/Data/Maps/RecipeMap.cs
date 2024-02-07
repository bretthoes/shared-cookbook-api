using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Data.Maps;

public class RecipeMap : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasKey(r => r.RecipeId)
            .HasName("PK_recipe_id");

        builder.ToTable("recipe");

        builder.HasIndex(
            r => r.CookbookId,
            "IX_recipe__cookbook_id");

        builder.Property(r => r.RecipeId)
            .HasColumnName("recipe_id")
            .IsRequired();
        builder.Property(r => r.CookbookId)
            .HasColumnName("cookbook_id")
            .IsRequired();
        builder.Property(r => r.Title)
            .HasMaxLength(255)
            .HasColumnName("title")
            .IsRequired();
        builder.Property(r => r.PersonId)
            .HasColumnName("person_id");
        builder.Property(r => r.Summary)
            .HasColumnName("summary");
        builder.Property(r => r.ImagePath)
            .HasMaxLength(255)
            .HasColumnName("image_path");
        builder.Property(r => r.VideoPath)
            .HasMaxLength(255)
            .HasColumnName("video_path");

        builder.HasOne(r => r.Cookbook)
            .WithMany(c => c.Recipes)
            .HasForeignKey(r => r.CookbookId)
            .HasConstraintName("FK_recipe__cookbook_id")
            .IsRequired();
        builder.HasOne(r => r.Author)
            .WithMany(a => a.Recipes)
            .HasForeignKey(r => r.PersonId)
            .HasConstraintName("FK_recipe__person_id");
        builder.HasMany(r => r.CookbookNotifications)
            .WithOne(cn => cn.Recipe)
            .HasForeignKey(cn => cn.RecipeId)
            .HasConstraintName("FK_cookbook_notification__recipe_id");
        builder.HasMany(r => r.IngredientCategories)
            .WithOne(ic => ic.Recipe)
            .HasForeignKey(cn => cn.RecipeId)
            .HasConstraintName("FK_ingredient_category__recipe_id");
        builder.HasMany(r => r.RecipeComments)
            .WithOne(rc => rc.Recipe)
            .HasForeignKey(rc => rc.RecipeId)
            .HasConstraintName("FK_recipe_comment__recipe_id");
        builder.HasMany(r => r.RecipeDirections)
            .WithOne(ic => ic.Recipe)
            .HasForeignKey(cn => cn.RecipeId)
            .HasConstraintName("FK_recipe_direction__recipe_id");
        builder.HasMany(r => r.RecipeIngredients)
            .WithOne(ic => ic.Recipe)
            .HasForeignKey(cn => cn.RecipeId)
            .HasConstraintName("FK_recipe_ingredient__recipe_id");
        builder.HasMany(r => r.RecipeRatings)
            .WithOne(ic => ic.Recipe)
            .HasForeignKey(cn => cn.RecipeId)
            .HasConstraintName("FK_recipe_rating__recipe_id");
    }
}
