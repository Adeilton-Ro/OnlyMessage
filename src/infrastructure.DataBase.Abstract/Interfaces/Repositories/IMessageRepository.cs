using Domain.Entities;
using Infrastructure.DataBase.Abstract.DTO;

namespace Infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IMessageRepository
{
    Task<UserAndFriendMessages> GetChatMessages(Guid id, Guid friendId, CancellationToken cancellationToken);
    IEnumerable<Message<User>> LastsMesssage(Guid id, IEnumerable<Guid> friendsId);
}