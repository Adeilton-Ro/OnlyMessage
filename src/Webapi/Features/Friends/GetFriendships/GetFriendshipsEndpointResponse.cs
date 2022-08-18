using Application.Feature.Friends.GetFriendships;
using Utils.Mapping;

namespace Webapi.Features.Friends.GetFriendships;
public record GetFriendshipsEndpointResponse(Guid Id, Guid FriendId, string UserName, string ImageUrl) : IMappeableFrom<GetFriendshipsQueryResponse>;