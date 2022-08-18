using Application.Feature.Friends.GetFriendships;
using FastEndpoints;
using System.Security.Claims;
using Utils.Mapping;

namespace Webapi.Features.Friends.GetFriendships;
public class GetFriendshipsEndpointRequest : IMappeableTo<GetFriendshipsQuery>
{
    [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
    public Guid Id { get; set; }
}