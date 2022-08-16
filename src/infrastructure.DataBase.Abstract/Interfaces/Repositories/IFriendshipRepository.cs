using Domain.Entities;

namespace Infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IFriendshipRepository
{
    Task<bool> IsAlreadyFriends(Guid Id, Guid FriendId, CancellationToken cancellationToken);
}