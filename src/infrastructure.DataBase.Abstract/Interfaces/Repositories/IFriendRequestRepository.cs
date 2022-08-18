using Domain.Entities;

namespace Infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IFriendRequestRepository
{
    Task Save(FriendRequest friendRequest, CancellationToken cancellationToken);
    void Delete(FriendRequest friendRequest);
    Task<bool> IsAlredyRequested(Guid id, Guid friendId, CancellationToken cancellationToken);
    Task<FriendRequest> GetById(Guid id, CancellationToken cancellationToken);
}