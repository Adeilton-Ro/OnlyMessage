using Application.Feature.Friends.GetFriendRequests;
using Utils.Mapping;

namespace Webapi.Features.Friends.GetFriendRequest;
public record GetFriendRequestsEndpointResponse(Guid Id, string Username, string Uri, DateTime Created) : IMappeableFrom<GetFriendRequestsQueryResponse>;