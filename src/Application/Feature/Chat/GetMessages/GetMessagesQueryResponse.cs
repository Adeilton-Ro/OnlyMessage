namespace Application.Feature.Chat.GetMessages;
public record GetMessagesQueryResponse(Guid Id, IEnumerable<MessageQueryResponse> Messages);
public record MessageQueryResponse(Guid Id, string TextMessage, DateTime SendeTime);