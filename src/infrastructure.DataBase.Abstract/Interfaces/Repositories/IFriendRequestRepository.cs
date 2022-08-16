using Domain.Entities;

namespace Infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IFriendRequestRepository
{
    Task Save(FriendRequest friendRequest, CancellationToken cancellationToken);
    Task<bool> IsAlredyRequested(Guid id, Guid friendId, CancellationToken cancellationToken);
}