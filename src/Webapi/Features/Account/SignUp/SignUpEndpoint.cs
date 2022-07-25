using Application.Feature.Account.SignUp;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Account.SignUp;
public class SignUpEndpoint
    : EndpointWithResult<SignUpEndpointRequest, SignUpCommand, SignUpCommandResponse, SignUpEndpointResponse>
{
    public SignUpEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Verbs(Http.POST);
        Routes("account/signup");
        AllowAnonymous();
    }

    public override void ConfigureSwagger()
    {
        Description(descriptor =>
            descriptor.RequireAuthorization()
            .Accepts<SignUpEndpointRequest>("application/json")
            .Produces<SignUpEndpointResponse>(200, "application/json")
            .ProducesValidationProblem()
        );

        Summary(s =>
        {
            s.Summary = "Sign up";
            s.Description = "sign up an user in the system";
            s[200] = "When sign up was successfull";
            s[401] = "When username was alredy in use";
        });
    }
}