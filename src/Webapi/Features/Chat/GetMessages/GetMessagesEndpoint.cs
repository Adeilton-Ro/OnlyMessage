using Application.Feature.Chat.GetMessages;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Chat.GetMessages;
public class GetMessagesEndpoint : EndpointWithResultCollection<GetMessagesEndpointRequest, GetMessagesQuery,
    GetMessagesQueryResponse, GetMessagesEndpointResponse>
{
    public GetMessagesEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Verbs(Http.GET);
        Routes("chat/messages/{FriendId}");
    }

    public override void ConfigureSwagger()
    {
        Description(descriptor =>
            descriptor.RequireAuthorization()
            .Accepts<GetMessagesEndpointRequest>("application/json")
            .Produces<GetMessagesEndpointResponse>(200, "application/json")
            .ProducesValidationProblem()
        );
    }
}
