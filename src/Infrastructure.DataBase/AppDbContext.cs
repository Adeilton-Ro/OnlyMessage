using Domain.Entities;
using infrastructure.DataBase.Abstract.Interfaces;
using Infrastructure.DataBase.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase;
public class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Message<Group>> GroupMessages { get; set; }
    public DbSet<Message<User>> UserMessages { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<FriendshipMap> FriendshipMaps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new GroupMessageMap());
        modelBuilder.ApplyConfiguration(new UserMessageMap());
        modelBuilder.ApplyConfiguration(new FriendshipMap());
        modelBuilder.ApplyConfiguration(new FriendRequestMap());
        modelBuilder.ApplyConfiguration(new GroupMap());
        modelBuilder.ApplyConfiguration(new UserGroupMap());
    }
}