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
        => context.UserMessages.Where(um => um.ReceiverId == id || um.SenderId == id).Where(um => um.ReceiverId == friendId || um.SenderId == friendId);

    public IEnumerable<Message<User>> LastsMesssage(Guid id, IEnumerable<Guid> friendsId)
        => friendsId.Select(f => context.UserMessages
            .Where(um => um.SenderId == id || um.ReceiverId == id)
            .OrderByDescending(um => um.Created)
            .FirstOrDefault(um => um.ReceiverId == f || um.SenderId == f));
}