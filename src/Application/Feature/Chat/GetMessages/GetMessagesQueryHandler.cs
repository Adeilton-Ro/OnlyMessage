using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Chat.GetMessages;
public class GetMessagesQueryHandler : IRequestWithResultHandler<GetMessagesQuery, IEnumerable<GetMessagesQueryResponse>>
{
    private readonly IMessageRepository messageRepository;
    private readonly IFriendshipRepository friendshipRepository;

    public GetMessagesQueryHandler(IMessageRepository messageRepository, IFriendshipRepository friendshipRepository)
    {
        this.messageRepository = messageRepository;
        this.friendshipRepository = friendshipRepository;
    }
    public async Task<Result<IEnumerable<GetMessagesQueryResponse>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        if (!await friendshipRepository.IsAlreadyFriends(request.Id, request.FriendId, cancellationToken))
            return Result.OfNotFoundResult("Friendship").Build<IEnumerable<GetMessagesQueryResponse>>();

        var friendMessages = messageRepository.GetChatMessages(request.Id, request.FriendId).GroupBy(m => m.SenderId);
        var response = friendMessages.Select(
            fm => new GetMessagesQueryResponse(fm.Key, fm.Select(m => new MessageQueryResponse(m.Id, m.TextMessage, m.Created)))
            );

        return Result.OfSuccess(response).Build();
    }
}