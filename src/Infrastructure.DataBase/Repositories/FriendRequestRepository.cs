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

    public void Delete(FriendRequest friendRequest)
        => context.FriendRequests.Remove(friendRequest);

    public IEnumerable<FriendRequest> GetAll(Guid id, CancellationToken cancellationToken)
        => context.FriendRequests.Where(fr => fr.FriendId == id);

    public async Task<FriendRequest> GetById(Guid id, CancellationToken cancellationToken)
        => await context.FriendRequests.FirstOrDefaultAsync(fr => fr.Id == id, cancellationToken);

    public Task<bool> IsAlredyRequested(Guid id, Guid friendId, CancellationToken cancellationToken)
        => context.FriendRequests.AnyAsync(fr => fr.UserId == id && fr.FriendId == friendId 
        || fr.UserId == friendId && fr.FriendId == id, cancellationToken);

    public async Task Save(FriendRequest friendRequest, CancellationToken cancellationToken)
        => await context.FriendRequests.AddAsync(friendRequest, cancellationToken);
}