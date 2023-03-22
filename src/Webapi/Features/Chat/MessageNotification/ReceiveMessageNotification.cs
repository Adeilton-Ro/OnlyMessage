using Application.Feature.Chat.SendMessage;
using Microsoft.AspNetCore.SignalR;

namespace Webapi.Features.Chat.MessageNotification;
public class ReceiveMessageNotification : Hub<IReceiveMessageNotification>
{
    public async Task SendMessageNotification(Guid friendId, SendMessageCommandResponse notification)
    {
        await Clients.User(friendId.ToString()).ReceiveMessageNotification(notification);
    }
}