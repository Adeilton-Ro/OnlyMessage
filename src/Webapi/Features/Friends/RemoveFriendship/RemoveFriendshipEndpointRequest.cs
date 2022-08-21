using Application.Feature.Friends.RemoveFriendship;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils.Mapping;

namespace Webapi.Features.Friends.RemoveFriendship;
public class RemoveFriendshipEndpointRequest : IMappeableTo<RemoveFriendshipCommand>
{
    [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
    public Guid UserId { get; set; }
    [FromQuery]
    public Guid Id { get; set; }
}