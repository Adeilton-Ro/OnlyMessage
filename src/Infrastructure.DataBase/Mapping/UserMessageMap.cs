using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.Mapping;
public class UserMessageMap : IEntityTypeConfiguration<Message<User>>
{
    public void Configure(EntityTypeBuilder<Message<User>> builder)
    {
        builder.HasKey(um => um.Id);
        builder.Property(um => um.TextMessage)
            .IsRequired();
        builder.Property(um => um.Created)
            .HasColumnName("timestamp without time zone")
            .IsRequired();
        builder.HasOne(um => um.Sender)
            .WithMany(u => u.Messages)
            .HasForeignKey(um => um.SenderId);
    }
}