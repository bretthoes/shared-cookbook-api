﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedCookbookApi.Data.Entities;

namespace SharedCookbookApi.Data.Maps;

public class CookbookNotificationMap : IEntityTypeConfiguration<CookbookNotification>
{
    public void Configure(EntityTypeBuilder<CookbookNotification> builder)
    {
        builder.ToTable("cookbook_notification");

        builder.HasKey(cn => cn.CookbookNotificationId)
            .HasName("PK_cookbook_notification_id");

        builder.HasIndex(
            cn => cn.SenderPersonId,
            "IX_cookbook_notification__sender_person_id");
        builder.HasIndex(
            cn => cn.CookbookId,
            "IX_cookbook_notification__cookbook_id");
        builder.HasIndex(
            cn => cn.RecipeId,
            "IX_cookbook_notification__recipe_id");

        builder.Property(cn => cn.CookbookNotificationId)
            .HasColumnName("cookbook_notification_id")
            .IsRequired();
        builder.Property(cn => cn.SenderPersonId)
            .HasColumnName("sender_person_id");
        builder.Property(cn => cn.CookbookId)
            .HasColumnName("cookbook_id");
        builder.Property(cn => cn.RecipeId)
            .HasColumnName("recipe_id");
        builder.Property(cn => cn.ActionType)
            .HasColumnName("action_type")
            .HasConversion<string>()
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(cn => cn.Created)
            .HasColumnName("created")
            .IsRequired();

        builder.HasOne(cn => cn.Cookbook)
            .WithMany(c => c.CookbookNotifications)
            .HasForeignKey(cn => cn.CookbookId)
            .HasConstraintName("FK_cookbook_notification__cookbook_id");
        builder.HasOne(cn => cn.Recipe)
            .WithMany(r => r.CookbookNotifications)
            .HasForeignKey(cn => cn.RecipeId)
            .HasConstraintName("FK_cookbook_notification__recipe_id");
        builder.HasOne(cn => cn.SenderPerson)
            .WithMany(p => p.CookbookNotifications)
            .HasForeignKey(cn => cn.SenderPersonId)
            .HasConstraintName("FK_cookbook_notification__sender_person_id");
    }
}
