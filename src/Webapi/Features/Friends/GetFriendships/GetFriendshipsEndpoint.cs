using Application.Feature.Friends.GetFriendships;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Friends.GetFriendships;
public class GetFriendshipsEndpoint : EndpointWithResultCollection<GetFriendshipsEndpointRequest, GetFriendshipsQuery,
    GetFriendshipsQueryResponse, GetFriendshipsEndpointResponse>
{
    public GetFriendshipsEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Routes("friends/getfriendships");
        Verbs(Http.GET);
    }

    public override void ConfigureSwagger()
    {
    }
}