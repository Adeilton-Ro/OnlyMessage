using Application.Feature.Chat.SendMessage;

namespace Webapi.Features.Chat.MessageNotification;
public interface IReceiveMessageNotification
{
    Task ReceiveMessageNotification(SendMessageCommandResponse message);
}