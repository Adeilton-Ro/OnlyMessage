using Domain.Entities;

namespace Infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IFriendshipRepository
{
    Task Save(Friendship friendship, CancellationToken cancellationToken);
    Task<bool> IsAlreadyFriends(Guid Id, Guid FriendId, CancellationToken cancellationToken);
}