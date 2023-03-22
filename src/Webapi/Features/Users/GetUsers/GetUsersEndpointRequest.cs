using Application.Feature.Users.GetUsers;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils.Mapping;

namespace Webapi.Features.Users.GetUsers;
public class GetUsersEndpointRequest : IMappeableTo<GetUsersQuery>
{
    [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
    public Guid Id { get; set; }
    [FromRoute]
    public string Search { get; set; }
}