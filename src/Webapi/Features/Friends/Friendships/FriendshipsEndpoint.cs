using Application.Feature.Friends.Friendships;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Friends.Friendships;
public class FriendshipsEndpoint : EndpointWithResult<FriendshipsEndpointRequest, FriendShipCommand>
{
    public FriendshipsEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Routes("friends/friendship");
        Verbs(Http.POST);
    }

    public override void ConfigureSwagger()
    {
        Description(descriptor =>
            descriptor.RequireAuthorization()
            .Accepts<FriendshipsEndpointRequest>("application/json")
            .ProducesValidationProblem()
        );

        Summary(s =>
        {
            s.Summary = "Friendship";
            s.Description = "Turn a friend request into a friend";
            s[200] = "When the order request is successfully accepted/declined";
        });
    }
}