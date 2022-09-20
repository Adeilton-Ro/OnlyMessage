using Domain.Entities;
using Infrastructure.DataBase.Abstract.DTO;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext context;

    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task Create(User user, CancellationToken cancellationToken)
     => await context.Users.AddAsync(user, cancellationToken);

    public Task<User> GetById(Guid id, CancellationToken cancellationToken)
        => context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public IEnumerable<User> GetByIds(IEnumerable<Guid> ids)
        => context.Users.Where(u => ids.Contains(u.Id));

    public async Task<User> GetByUserName(string userName, CancellationToken cancellationToken)
     => await context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);

    public void Update(User user)
        => context.Users.Update(user);

    public async Task<IEnumerable<UserIsAlredyRequested>> Search(Guid id, string seach)
    {
        var friendships = (await context.Users.Include(u => u.Friendships).FirstOrDefaultAsync(u => u.Id == id)).Friendships.Select(fs => fs.UserId);

        var friendRequests = context.FriendRequests.Where(fr => fr.UserId == id || fr.FriendId == id);

        var users = context.Users.Include(u => u.FriendsRequest).Where(u => u.UserName.ToLower().StartsWith(seach) && u.Id != id
        && !friendships.Any(fId => fId == u.Id)).OrderBy(u => u.UserName);

        return users.Select(u =>
            new UserIsAlredyRequested(u, friendRequests.Any(fr => fr.UserId == u.Id || fr.FriendId == u.Id))
        );
    }
}