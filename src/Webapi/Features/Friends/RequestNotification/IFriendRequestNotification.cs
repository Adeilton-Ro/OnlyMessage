using Application.Feature.Friends.FriendRequests;

namespace Webapi.Features.Friends.RequestNotification;
public interface IFriendRequestNotification
{
    Task ReceiveFriendRequestNotification(FriendRequestCommandResponse friendRequest);
}