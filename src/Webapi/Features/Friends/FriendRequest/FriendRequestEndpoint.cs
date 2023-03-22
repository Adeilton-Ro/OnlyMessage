using Application.Feature.Friends.FriendRequests;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Utils.Results;
using Webapi.Features.Friends.RequestNotification;
using Webapi.RequestHandling;

namespace Webapi.Features.Friends.FriendRequest;
public class FriendRequestEndpoint : EndpointWithResult<FriendRequestEndpointRequest, FriendRequestCommand, FriendRequestCommandResponse, FriendRequestEndpointResponse>
{
    private readonly IHubContext<FriendRequestNotification, IFriendRequestNotification> hubContext;

    public FriendRequestEndpoint(IMapper mapper, ISender sender, IHubContext<FriendRequestNotification,IFriendRequestNotification> hubContext) : base(mapper, sender)
    {
        this.hubContext = hubContext;
    }

    public override void ConfigureRoute()
    {
        Routes("friends/friendrequest");
        Verbs(Http.POST);
    }

    public override async Task HandleAsync(FriendRequestEndpointRequest req, CancellationToken ct)
    {
        var result = await base.AcessApplication(req, ct);

        if(result.IsSuccess && result.Content is not null)
            await hubContext.Clients.User(req.FriendId.ToString()).ReceiveFriendRequestNotification(result.Content);

        await base.SendResponse(result, ct);
    }

    public override void ConfigureSwagger()
    {
        Description(descriptor =>
            descriptor.RequireAuthorization()
            .Accepts<FriendRequestEndpointRequest>("application/json")
            .Produces<FriendRequestEndpointResponse>(200, "application/json")
            .ProducesValidationProblem()
        );

        Summary(s =>
        {
            s.Summary = "Friend Request";
            s.Description = "Send a request to be a friend";
            s[200] = "When the request was sent successfully";
        });
    }
}