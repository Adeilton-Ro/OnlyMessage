using Application.Feature.Friends.FriendRequests;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils.Mapping;

namespace Webapi.Features.Friends.FriendRequest;
public class FriendRequestEndpointRequest : IMappeableTo<FriendRequestCommand>
{
    [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
    public Guid Id { get; set; }
    [FromBody]
    public Guid FriendId { get; set; }
}