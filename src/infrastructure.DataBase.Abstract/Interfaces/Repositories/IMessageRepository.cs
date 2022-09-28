using Domain.Entities;
using Infrastructure.DataBase.Abstract.DTO;

namespace Infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IMessageRepository
{
    IEnumerable<Message<User>> GetChatMessages(Guid id, Guid friendId);
    IEnumerable<Message<User>> LastsMesssage(Guid id, IEnumerable<Guid> friendsId);
}