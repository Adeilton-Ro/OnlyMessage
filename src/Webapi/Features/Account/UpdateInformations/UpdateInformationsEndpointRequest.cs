using Application.Feature.Account.UpdateInformations;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils.Mapping;

namespace Webapi.Features.Account.UpdateInformations;
public class UpdateInformationsEndpointRequest : IMappeableTo<UpdateInformationsCommand>
{
    [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
    public Guid Id { get; set; }
    [FromBody]
    public string Username { get; set; }
    [FromBody]
    public string Password { get; set; }
}