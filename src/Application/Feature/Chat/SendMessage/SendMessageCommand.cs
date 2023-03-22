using Utils.Results;

namespace Application.Feature.Chat.SendMessage;
public record SendMessageCommand(Guid Id, Guid ReceiverId, string TextMessage) : IRequestWithResult<SendMessageCommandResponse>;