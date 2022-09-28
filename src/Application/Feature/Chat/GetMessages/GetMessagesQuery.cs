using Utils.Results;

namespace Application.Feature.Chat.GetMessages;
public record GetMessagesQuery(Guid Id, Guid FriendId) : IRequestWithResult<IEnumerable<GetMessagesQueryResponse>>;