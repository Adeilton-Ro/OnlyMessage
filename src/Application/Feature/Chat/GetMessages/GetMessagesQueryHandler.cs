using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Chat.GetMessages;
public class GetMessagesQueryHandler : IRequestWithResultHandler<GetMessagesQuery, IEnumerable<GetMessagesQueryResponse>>
{
    private readonly IMessageRepository messageRepository;

    public GetMessagesQueryHandler(IMessageRepository messageRepository)
    {
        this.messageRepository = messageRepository;
    }
    public Task<Result<IEnumerable<GetMessagesQueryResponse>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var response = messageRepository.GetChatMessages(request.Id, request.FriendId)
            .Select(m => new GetMessagesQueryResponse(m.Id, m.TextMessage, m.Created, 
            new GetUserMessagesQueryResponse(m.SenderId, m.Sender.UserName, m.Sender.Uri), 
            new GetUserMessagesQueryResponse( m.ReceiverId, m.Receiver.UserName, m.Receiver.Uri)
            ));

        return Task.FromResult(Result.OfSuccess(response).Build());
    }
}