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

    public async Task<Friendship> GetById(Guid Id, CancellationToken cancellationToken)
        => await context.Friendships.FirstOrDefaultAsync(fs => fs.Id == Id, cancellationToken);

    public async Task<Friendship> GetByFriendId(Guid id, Guid friendId, CancellationToken cancellationToken)
        => await context.Friendships.FirstOrDefaultAsync(fs => fs.FriendId == friendId && fs.UserId == id , cancellationToken);

    public void Remove(Friendship friendship)
        => context.Friendships.Remove(friendship);

    public async Task Save(Friendship friendship, CancellationToken cancellationToken)
        => await context.Friendships.AddAsync(friendship, cancellationToken);
}