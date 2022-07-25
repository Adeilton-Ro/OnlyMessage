using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.Mapping
{
    public class GroupMessageMap : IEntityTypeConfiguration<Message<Group>>
    {
        public void Configure(EntityTypeBuilder<Message<Group>> builder)
        {
            builder.HasKey(gm => gm.Id);
            builder.Property(gm => gm.TextMessage).IsRequired();
            builder.Property(gm => gm.Created)
                .HasColumnName("timestamp without time zone")
                .IsRequired();
            builder.HasOne(gm => gm.Receiver)
                .WithMany(g => g.Messages)
                .HasForeignKey(gm => gm.ReceiverId);
        }
    }
}
