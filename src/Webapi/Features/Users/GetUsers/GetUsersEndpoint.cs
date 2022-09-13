using Application.Feature.Users.GetUsers;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Users.GetUsers;
public class GetUsersEndpoint : EndpointWithResultCollection<GetUsersEndpointRequest, GetUsersQuery, GetUsersQueryResponse, GetUsersEndpointResponse>
{
    public GetUsersEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Verbs(Http.GET);
        Routes("users/getusers/{Search}");
    }

    public override void ConfigureSwagger() { }
}