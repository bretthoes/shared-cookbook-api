using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Data.Maps;

public class PersonMap : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("person");

        builder.HasKey(p => p.PersonId)
            .HasName("PK_person_id");

        builder.HasIndex(p => p.Email, "UQ_email")
            .IsUnique();

        builder.Property(p => p.PersonId)
            .HasColumnName("person_id")
            .IsRequired();
        builder.Property(p => p.DisplayName)
            .HasMaxLength(255)
            .HasColumnName("display_name");
        builder.Property(p => p.Email)
            .HasMaxLength(255)
            .HasColumnName("email")
            .IsRequired();
        builder.Property(p => p.ImagePath)
            .HasMaxLength(255)
            .HasColumnName("image_path");
        builder.Property(p => p.PasswordHash)
            .HasMaxLength(255)
            .HasColumnName("password_hash");
        builder.Property(p => p.PasswordSalt)
            .HasMaxLength(255)
            .HasColumnName("password_salt");
        builder.Property(p => p.FirebaseUserUid)
            .HasMaxLength(255)
            .HasColumnName("firebase_user_uid");

        builder.HasMany(p => p.Cookbooks)
            .WithOne(c => c.Creator)
            .HasForeignKey(c => c.CreatorPersonId)
            .HasConstraintName("FK_cookbook__creator_person_id");
        builder.HasMany(p => p.CookbookMemberships)
            .WithOne(cm => cm.Person)
            .HasForeignKey(cm => cm.PersonId)
            .HasConstraintName("FK_cookbook_member__person_id");
        builder.HasMany(p => p.CookbookNotifications)
            .WithOne(cn => cn.SenderPerson)
            .HasForeignKey(cn => cn.SenderPersonId)
            .HasConstraintName("FK_cookbook_notification__sender_person_id");
        builder.HasMany(p => p.SentInvitations)
            .WithOne(ci => ci.Sender)
            .HasForeignKey(ci => ci.SenderPersonId)
            .HasConstraintName("FK_cookbook_invitation__sender_person_id");
        builder.HasMany(p => p.ReceivedInvitations)
            .WithOne(ci => ci.Recipient)
            .HasForeignKey(ci => ci.RecipientPersonId)
            .HasConstraintName("FK_cookbook_invitation__recipient_person_id");
        builder.HasMany(p => p.Recipes)
            .WithOne(rc => rc.Author)
            .HasForeignKey(rc => rc.PersonId)
            .HasConstraintName("FK_recipe__person_id");
        builder.HasMany(p => p.RecipeComments)
            .WithOne(rc => rc.Author)
            .HasForeignKey(rc => rc.PersonId)
            .HasConstraintName("FK_recipe_comment__person_id");
        builder.HasMany(p => p.RecipeRatings)
            .WithOne(rr => rr.Person)
            .HasForeignKey(rr => rr.PersonId)
            .HasConstraintName("FK_recipe_rating__person_id");
    }
}
