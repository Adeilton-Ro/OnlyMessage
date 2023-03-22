using Utils.Results;

namespace Application.Feature.Friends.Friendships;
public record FriendShipCommand(Guid Id, bool IsAccepted, Guid UserId) : IRequestWithResult;