using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.Mapping;
public class FriendshipMap : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.HasKey(fs => fs.Id);
        builder.HasOne(fs => fs.Friend)
            .WithMany(u => u.Friendships)
            .HasForeignKey(fs => fs.FriendId);
    }
}