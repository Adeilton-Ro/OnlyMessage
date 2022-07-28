using Application.Feature.Auth.Login;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Auth.Login;
public class LoginEndpoint : EndpointWithResult<LoginEndpointRequest, LoginCommand, LoginCommandResponse, LoginEndpointResponse>
{
    public LoginEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Routes("auth/login");
        Verbs(Http.POST);
        AllowAnonymous();
    }

    public override void ConfigureSwagger()
    {
        Description(descriptor =>
            descriptor.RequireAuthorization()
            .Accepts<LoginEndpointRequest>("application/json")
            .Produces<LoginEndpointResponse>(200, "application/json")
            .ProducesValidationProblem()
        );

        Summary(s =>
        {
            s.Summary = "Login";
            s.Description = "Generates a valid JWT token";
            s[200] = "When login was successfull";
            s[401] = "When username or password was wrong";
        });
    }
}