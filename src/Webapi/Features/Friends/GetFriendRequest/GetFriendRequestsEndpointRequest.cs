using Application.Feature.Friends.GetFriendRequests;
using FastEndpoints;
using System.Security.Claims;
using Utils.Mapping;

namespace Webapi.Features.Friends.GetFriendRequest;
public class GetFriendRequestsEndpointRequest : IMappeableTo<GetFriendRequestsQuery>
{
    [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
    public Guid Id { get; set; }
}