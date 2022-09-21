namespace Application.Feature.Chat.GetMessages;
public record GetMessagesQueryResponse(GetUserMessagesQueryResponse MyMessages, GetUserMessagesQueryResponse FriendMessages);

public record GetUserMessagesQueryResponse(Guid Id, IEnumerable<GetMessageQueryResponse> Messages);
public record GetMessageQueryResponse(Guid Id, string TextMessage, DateTime SendeTime);