using Application.Feature.Friends.Friendships;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils.Mapping;

namespace Webapi.Features.Friends.Friendships;
public class FriendshipsEndpointRequest : IMappeableTo<FriendShipCommand>
{
    [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
    public Guid UserId { get; set; }
    [FromBody]
    public Guid Id { get; set; }
    [FromBody]
    public bool IsAccepted { get; set; }
}