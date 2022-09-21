using Domain.Entities;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Chat.GetMessages;
public class GetMessagesQueryHandler : IRequestWithResultHandler<GetMessagesQuery, GetMessagesQueryResponse>
{
    private readonly IMessageRepository messageRepository;
    private readonly IFriendshipRepository friendshipRepository;

    public GetMessagesQueryHandler(IMessageRepository messageRepository, IFriendshipRepository friendshipRepository)
    {
        this.messageRepository = messageRepository;
        this.friendshipRepository = friendshipRepository;
    }
    public async Task<Result<GetMessagesQueryResponse>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        if (!await friendshipRepository.IsAlreadyFriends(request.Id, request.FriendId, cancellationToken))
            return Result.OfNotFoundResult("Friendship").Build<GetMessagesQueryResponse>();

        var userAndFriendMessages = await messageRepository.GetChatMessages(request.Id, request.FriendId, cancellationToken);
        var response = new GetMessagesQueryResponse(
            new GetUserMessagesQueryResponse(userAndFriendMessages.User.Id,
            userAndFriendMessages.User.Messages.OrderBy(m => m.Created).Select(m => new GetMessageQueryResponse(m.Id, m.TextMessage, m.Created))),
            new GetUserMessagesQueryResponse(userAndFriendMessages.Friend.Id,
            userAndFriendMessages.Friend.Messages.OrderBy(m => m.Created).Select(m => new GetMessageQueryResponse(m.Id, m.TextMessage, m.Created))));

        return Result.OfSuccess(response).Build();
    }
}