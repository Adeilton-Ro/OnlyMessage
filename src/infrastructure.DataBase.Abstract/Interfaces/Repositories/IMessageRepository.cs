using Domain.Entities;

namespace Infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IMessageRepository
{
    IEnumerable<Message<User>> GetChatMessages(Guid id, Guid friendId);
}