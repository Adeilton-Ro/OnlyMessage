using Application.Feature.Friends.FriendRequests;
using Utils.Mapping;

namespace Webapi.Features.Friends.FriendRequest;
public record FriendRequestEndpointResponse(Guid id) : IMappeableFrom<FriendRequestCommandResponse>;