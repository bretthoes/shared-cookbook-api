using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Data.Maps;

public class CookbookMap : IEntityTypeConfiguration<Cookbook>
{
    public void Configure(EntityTypeBuilder<Cookbook> builder) 
    {
        builder.ToTable("cookbook");

        builder.HasKey(c => c.CookbookId)
            .HasName("PK_cookbook_id");

        builder.HasIndex(
            c => c.CreatorPersonId, 
            "IX_cookbook_creator__person_id");

        builder.Property(c => c.CookbookId)
            .HasColumnName("cookbook_id")
            .IsRequired();
        builder.Property(c => c.CreatorPersonId)
            .HasColumnName("creator_person_id");
        builder.Property(c => c.Title)
            .HasColumnName("title")
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(c => c.ImagePath)
            .HasColumnName("image_path")
            .HasMaxLength(255);

        builder.HasOne(c => c.Creator)
            .WithMany(p => p.Cookbooks)
            .HasForeignKey(c => c.CreatorPersonId)
            .HasConstraintName("FK_cookbook__creator_person_id");
        builder.HasMany(c => c.CookbookInvitations)
            .WithOne(ci => ci.Cookbook)
            .HasForeignKey(ci => ci.CookbookId);
        builder.HasMany(c => c.CookbookMembers)
            .WithOne(cm => cm.Cookbook)
            .HasForeignKey(cm => cm.CookbookId);
        builder.HasMany(c => c.CookbookNotifications)
            .WithOne(cn => cn.Cookbook)
            .HasForeignKey(cn => cn.CookbookId);
        builder.HasMany(c => c.Recipes)
            .WithOne(r => r.Cookbook)
            .HasForeignKey(r => r.CookbookId);
    }
}
