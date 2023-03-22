using Application.Feature.Auth.Login;
using Microsoft.AspNetCore.Mvc;
using Utils.Mapping;

namespace Webapi.Features.Auth.Login;
public class LoginEndpointRequest : IMappeableTo<LoginCommand>
{
    [FromBody]
    public string UserName { get; set; }
    [FromBody]
    public string Password { get; set; }
}