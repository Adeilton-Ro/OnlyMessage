using Application.Feature.Chat.SendMessage;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Utils.Results;
using Webapi.Features.Chat.MessageNotification;
using Webapi.RequestHandling;

namespace Webapi.Features.Chat.SendMessage;
public class SendMessageEndpoint : EndpointWithResult<SendMessageEndpointRequest, SendMessageCommand, SendMessageCommandResponse, SendMessageEndpointResponse>
{
    private readonly IHubContext<ReceiveMessageNotification, IReceiveMessageNotification> hubContext;

    public SendMessageEndpoint(IMapper mapper, ISender sender, IHubContext<ReceiveMessageNotification, IReceiveMessageNotification> hubContext) : base(mapper, sender)
    {
        this.hubContext = hubContext;
    }

    public override void ConfigureRoute()
    {
        Routes("/chat/sendmessage");
        Verbs(Http.POST);
    }

    public override async Task HandleAsync(SendMessageEndpointRequest req, CancellationToken ct)
    {
        var commandResponse = await base.AcessApplication(req, ct);

        if (commandResponse.IsSuccess && commandResponse.Content is not null)
            await hubContext.Clients.User(req.ReceiverId.ToString()).ReceiveMessageNotification(commandResponse.Content);

        await base.SendResponse(commandResponse, ct);
    }

    public override void ConfigureSwagger()
    {
        
    }
}