using Utils.Results;

namespace Application.Feature.Friends.FriendRequests;
public record FriendRequestCommand(Guid Id, Guid FriendId) : IRequestWithResult<FriendRequestCommandResponse>;