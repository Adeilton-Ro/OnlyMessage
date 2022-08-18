using Domain.Entities;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repositories;
public class FriendshipRepository : IFriendshipRepository
{
    private readonly AppDbContext context;

    public FriendshipRepository(AppDbContext context)
    {
        this.context = context;
    }

    public IEnumerable<Friendship> GetFriends(Guid Id, CancellationToken cancellationToken)
        => context.Friendships.Where(f => f.FriendId == Id);

    public Task<bool> IsAlreadyFriends(Guid id, Guid friendId, CancellationToken cancellationToken)
        => context.Friendships.AnyAsync(fs => fs.UserId == id && fs.FriendId == friendId
        || fs.FriendId == id && fs.UserId == friendId );

    public async Task Save(Friendship friendship, CancellationToken cancellationToken)
        => await context.Friendships.AddAsync(friendship, cancellationToken);
}