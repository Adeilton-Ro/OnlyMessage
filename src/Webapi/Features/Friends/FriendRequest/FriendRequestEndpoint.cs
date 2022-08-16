using Application.Feature.Friends.FriendRequests;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Utils.Results;
using Webapi.Features.Auth.Login;
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

        if(result.IsSuccess && result.Code == ResultCodes.Success)
        {
            await hubContext.Clients.User(req.FriendId.ToString()).ReceiveFriendRequestNotification(result.Content);
        }
        await base.SendResponse(result, ct);
    }

    public override void ConfigureSwagger()
    {
        Description(descriptor =>
            descriptor.RequireAuthorization()
            .Accepts<FriendRequestEndpointRequest>("application/json")
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