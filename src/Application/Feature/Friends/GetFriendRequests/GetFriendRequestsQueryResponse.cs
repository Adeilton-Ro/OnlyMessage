namespace Application.Feature.Friends.GetFriendRequests;
public record GetFriendRequestsQueryResponse(Guid Id, string Username, string Uri, DateTime Created);