using Domain.Entities;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;

namespace Infrastructure.DataBase.Repositories;
public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext context;

    public MessageRepository(AppDbContext context)
    {
        this.context = context;
    }
    public IEnumerable<Message<User>> GetChatMessages(Guid id, Guid friendId)
        => context.UserMessages
        .Where(um => um.SenderId == id && um.ReceiverId == friendId || um.SenderId == friendId && um.ReceiverId == id)
        .OrderBy(um => um.Created);
}