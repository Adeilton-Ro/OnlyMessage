using Application.Feature.Account.SignUp;
using Microsoft.AspNetCore.Mvc;
using Utils.Mapping;

namespace Webapi.Features.Account.SignUp;
public class SignUpEndpointRequest : IMappeableTo<SignUpCommand>
{
    [FromBody]
    public string UserName { get; set; }
    [FromBody]
    public string Password { get; set; }
}