using Application.Feature.Friends.RemoveFriendship;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Friends.RemoveFriendship;
public class RemoveFriendshipEndpoint : EndpointWithResult<RemoveFriendshipEndpointRequest, RemoveFriendshipCommand>
{
    public RemoveFriendshipEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Verbs(Http.DELETE);
        Routes("friends/removefriendship/{Id}");
    }

    public override void ConfigureSwagger()
    {
    }
}