using Application.Feature.Auth.RefreshToken;
using Microsoft.AspNetCore.Mvc;
using Utils.Mapping;

namespace Webapi.Features.Auth.RefreshToken;
public class RefreshTokenEndpointRequest : IMappeableTo<RefreshTokenCommand>
{
    [FromBody]
    public string Token { get; set; }
    [FromBody]
    public string RefreshToken { get; set; }
}