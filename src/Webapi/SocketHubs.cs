using Webapi.Features.Chat.MessageNotification;
using Webapi.Features.Friends.RequestNotification;

namespace Webapi;
public static class SocketHubs
{
    public static WebApplication AddHubsMappers(this WebApplication app)
    {
        app.MapHub<FriendRequestNotification>("/friendrequestnotification").RequireAuthorization();
        app.MapHub<ReceiveMessageNotification>("/chat/messagenotification").RequireAuthorization();
        return app;
    }
}