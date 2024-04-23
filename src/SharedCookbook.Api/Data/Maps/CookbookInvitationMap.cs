using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedCookbook.Api.Data.Entities;

namespace SharedCookbookApi.Data.Maps;

public class CookbookInvitationMap : IEntityTypeConfiguration<CookbookInvitation>
{
    public void Configure(EntityTypeBuilder<CookbookInvitation> builder)
    {
        builder.ToTable("cookbook_invitation");

        builder.HasKey(ci => ci.CookbookInvitationId)
            .HasName("PK_cookbook_invitation_id");

        builder.HasIndex(
            ci => ci.CookbookId, 
            "IX_cookbook_invitation__cookbook_id");
        builder.HasIndex(
            ci => ci.RecipientPersonId,
            "IX_cookbook_invitation_recipient__person_id");
        builder.HasIndex(
            ci => ci.SenderPersonId,
            "IX_cookbook_invitation_sender__person_id");

        builder.Property(ci => ci.CookbookInvitationId)
            .HasColumnName("cookbook_invitation_id")
            .IsRequired();
        builder.Property(ci => ci.CookbookId)
            .HasColumnName("cookbook_id")
            .IsRequired();
        builder.Property(ci => ci.SenderPersonId)
            .HasColumnName("sender_person_id");
        builder.Property(ci => ci.RecipientPersonId)
            .HasColumnName("recipient_person_id");
        builder.Property(ci => ci.InvitationStatus)
            .HasColumnName("invitation_status")
            .HasConversion<string>()
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(ci => ci.SentDate)
            .HasColumnName("sent_date")
            .IsRequired();
        builder.Property(ci => ci.ResponseDate)
            .HasColumnName("response_date");

        builder.HasOne(ci => ci.Cookbook)
            .WithMany(c => c.CookbookInvitations)
            .HasForeignKey(ci => ci.CookbookId)
            .HasConstraintName("FK_cookbook_invitation__cookbook_id")
            .IsRequired();
        builder.HasOne(ci => ci.Recipient)
            .WithMany(p => p.ReceivedInvitations)
            .HasForeignKey(ci => ci.RecipientPersonId)
            .HasConstraintName("FK_cookbook_invitation__recipient_person_id");
        builder.HasOne(ci => ci.Sender)
            .WithMany(p => p.SentInvitations)
            .HasForeignKey(ci => ci.SenderPersonId)
            .HasConstraintName("FK_cookbook_invitation__sender_person_id");
    }
}
