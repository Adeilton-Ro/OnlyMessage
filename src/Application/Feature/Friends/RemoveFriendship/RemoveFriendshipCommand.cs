using Utils.Results;

namespace Application.Feature.Friends.RemoveFriendship;
public record RemoveFriendshipCommand(Guid Id, Guid UserId) : IRequestWithResult;