using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.Mapping;
public class FriendRequestMap : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        builder.HasKey(fr => fr.Id);
        builder.Property(fr => fr.Created)
            .HasColumnName("timestamp without time zone")
            .IsRequired();
        builder.HasOne(fr => fr.User)
            .WithMany(u => u.FriendsRequest)
            .HasForeignKey(fr => fr.UserId);
    }
}