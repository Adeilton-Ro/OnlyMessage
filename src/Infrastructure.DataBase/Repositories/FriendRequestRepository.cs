using Domain.Entities;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repositories;
public class FriendRequestRepository : IFriendRequestRepository
{
    private readonly AppDbContext context;

    public FriendRequestRepository(AppDbContext context)
    {
        this.context = context;
    }

    public Task<bool> IsAlredyRequested(Guid id, Guid friendId, CancellationToken cancellationToken)
        => context.FriendRequests.AnyAsync(fr => fr.UserId == id && fr.FriendId == friendId 
        || fr.UserId == friendId && fr.FriendId == id, cancellationToken);

    public async Task Save(FriendRequest friendRequest, CancellationToken cancellationToken)
        => context.FriendRequests.AddAsync(friendRequest, cancellationToken);
}