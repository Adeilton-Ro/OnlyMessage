using Utils.Results;

namespace Application.Feature.Friends.GetFriendRequests;
public record GetFriendRequestsQuery(Guid Id) : IRequestWithResult<IEnumerable<GetFriendRequestsQueryResponse>>;
