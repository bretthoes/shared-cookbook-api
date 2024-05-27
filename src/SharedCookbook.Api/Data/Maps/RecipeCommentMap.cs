﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedCookbook.Api.Data.Entities;

namespace SharedCookbookApi.Data.Maps;

public class RecipeCommentMap : IEntityTypeConfiguration<RecipeComment>
{
    public void Configure(EntityTypeBuilder<RecipeComment> builder)
    {
        builder.ToTable("recipe_comment");

        builder.HasKey(rc => rc.RecipeCommentId)
            .HasName("PK_recipe_comment_id");

        builder.HasIndex(
            rc => rc.PersonId,
            "IX_recipe_comment__person_id");
        builder.HasIndex(
            rc => rc.RecipeId,
            "IX_recipe_comment__recipe_id");

        builder.Property(rc => rc.RecipeCommentId)
            .HasColumnName("recipe_comment_id")
            .IsRequired();
        builder.Property(rc => rc.RecipeId)
            .HasColumnName("recipe_id")
            .IsRequired();
        builder.Property(rc => rc.PersonId)
            .HasColumnName("person_id")
            .IsRequired();
        builder.Property(rc => rc.CommentText)
            .HasColumnName("comment_text")
            .IsRequired();
        builder.Property(rc => rc.Created)
            .HasColumnName("created")
            .IsRequired();

        builder.HasOne<Person>()
            .WithMany(a => a.RecipeComments)
            .HasForeignKey(rc => rc.PersonId)
            .HasConstraintName("FK_recipe_comment__person_id");
        builder.HasOne<Recipe>()
            .WithMany(r => r.RecipeComments)
            .HasForeignKey(rc => rc.RecipeId)
            .HasConstraintName("FK_recipe_comment__recipe_id");
    }
}
