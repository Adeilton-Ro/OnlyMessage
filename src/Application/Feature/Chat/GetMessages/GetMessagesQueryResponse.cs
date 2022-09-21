namespace Application.Feature.Chat.GetMessages;
public record GetMessagesQueryResponse(
    Guid Id, 
    string TextMessage, DateTime SendeTime, 
    GetUserMessagesQueryResponse Sender, 
    GetUserMessagesQueryResponse Receiver);

public record GetUserMessagesQueryResponse(Guid Id, string Name, string ImageUrl);