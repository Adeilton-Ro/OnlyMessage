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

    public Task<bool> IsAlreadyFriends(Guid id, Guid friendId, CancellationToken cancellationToken)
        => context.Friendships.AnyAsync(fs => fs.UserId == id && fs.FriendId == friendId
        || fs.FriendId == id && fs.UserId == friendId );
}