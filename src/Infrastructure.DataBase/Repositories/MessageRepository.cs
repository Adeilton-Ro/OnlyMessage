﻿using Domain.Entities;
using Infrastructure.DataBase.Abstract.DTO;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repositories;
public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext context;

    public MessageRepository(AppDbContext context)
    {
        this.context = context;
    }
    public async Task<UserAndFriendMessages> GetChatMessages(Guid id, Guid friendId, CancellationToken cancellationToken)
        => new UserAndFriendMessages(
            await context.Users.Include(u => u.Messages).FirstOrDefaultAsync(u => u.Id == id, cancellationToken),
            await context.Users.Include(u => u.Messages).FirstOrDefaultAsync(u => u.Id == friendId, cancellationToken)
            );

    public IEnumerable<Message<User>> LastsMesssage(Guid id, IEnumerable<Guid> friendsId)
        => friendsId.Select(f => context.UserMessages
            .Where(um => um.SenderId == id || um.ReceiverId == id)
            .OrderByDescending(um => um.Created)
            .FirstOrDefault(um => um.ReceiverId == f || um.SenderId == f));
}