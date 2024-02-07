using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Data.Maps;

public class IngredientCategoryMap : IEntityTypeConfiguration<IngredientCategory>
{
    public void Configure(EntityTypeBuilder<IngredientCategory> builder)
    {
        builder.ToTable("ingredient_category");

        builder.HasKey(ic => ic.IngredientCategoryId)
            .HasName("PK_ingredient_category_id");

        builder.HasIndex(
            ic => ic.RecipeId,
            "IX_ingredient__category_recipe_id");

        builder.Property(ic => ic.IngredientCategoryId)
            .HasColumnName("ingredient_category_id")
            .IsRequired();
        builder.Property(ic => ic.RecipeId)
            .HasColumnName("recipe_id");
        builder.Property(ic => ic.Title)
            .HasMaxLength(255)
            .HasColumnName("title")
            .IsRequired();

        builder.HasOne(ic => ic.Recipe)
            .WithMany(r => r.IngredientCategories)
            .HasForeignKey(d => d.RecipeId)
            .HasConstraintName("FK_ingredient_category__recipe_id")
            .IsRequired();
    }
}
