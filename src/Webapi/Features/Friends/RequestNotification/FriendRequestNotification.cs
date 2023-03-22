using Application.Feature.Friends.FriendRequests;
using Microsoft.AspNetCore.SignalR;

namespace Webapi.Features.Friends.RequestNotification;
public class FriendRequestNotification : Hub<IFriendRequestNotification>
{
    public async Task SendFriendRequestNotification(Guid friendId,FriendRequestCommandResponse friendRequest)
    {
        await Clients.User(friendId.ToString()).ReceiveFriendRequestNotification(friendRequest);
    }
}