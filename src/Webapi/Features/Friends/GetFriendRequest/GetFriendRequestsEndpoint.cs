using Application.Feature.Friends.GetFriendRequests;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Friends.GetFriendRequest;
public class GetFriendRequestsEndpoint : EndpointWithResultCollection<GetFriendRequestsEndpointRequest, GetFriendRequestsQuery,
    GetFriendRequestsQueryResponse, GetFriendRequestsEndpointResponse>
{
    public GetFriendRequestsEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Verbs(Http.GET);
        Routes("friends/getfriendrequests");
    }

    public override void ConfigureSwagger()
    {
    }
}