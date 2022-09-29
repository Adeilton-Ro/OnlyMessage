using Domain.Entities;

namespace Infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IMessageRepository
{
    IEnumerable<Message<User>> GetChatMessages(Guid id, Guid friendId);
    IEnumerable<Message<User>> LastsMesssage(Guid id, IEnumerable<Guid> friendsId);
    Task AddMessage(Message<User> message, CancellationToken cancellationToken);
}