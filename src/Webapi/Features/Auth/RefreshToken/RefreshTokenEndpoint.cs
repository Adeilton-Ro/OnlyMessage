using Application.Feature.Auth.RefreshToken;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Auth.RefreshToken;
public class RefreshTokenEndpoint : EndpointWithResult<RefreshTokenEndpointRequest, RefreshTokenCommand,
    RefreshTokenCommandResponse, RefreshTokenEndpointResponse>
{
    public RefreshTokenEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Verbs(Http.POST);
        Routes("auth/refresh");
        AllowAnonymous();
    }

    public override void ConfigureSwagger() { }
}