namespace Application.Feature.Chat.SendMessage;
public record SendMessageCommandResponse(Guid Id, MessageCommandResponse Message);
public record MessageCommandResponse(Guid Id, string TextMessage, DateTime SendeTime);