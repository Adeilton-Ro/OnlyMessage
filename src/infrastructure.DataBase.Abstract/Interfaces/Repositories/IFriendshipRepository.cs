using Domain.Entities;

namespace Infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IFriendshipRepository
{
    Task Save(Friendship friendship, CancellationToken cancellationToken);
    Task<bool> IsAlreadyFriends(Guid Id, Guid FriendId, CancellationToken cancellationToken);
    IEnumerable<Friendship> GetFriends(Guid Id, CancellationToken cancellationToken);
    Task<Friendship> GetById(Guid Id, CancellationToken cancellationToken);
    void Remove(Friendship friendship);
    Task<Friendship> GetByFriendId(Guid id, Guid friendId, CancellationToken cancellationToken);
}