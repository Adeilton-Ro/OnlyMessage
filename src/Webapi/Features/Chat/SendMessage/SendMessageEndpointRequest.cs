using Application.Feature.Chat.SendMessage;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils.Mapping;

namespace Webapi.Features.Chat.SendMessage;
public class SendMessageEndpointRequest : IMappeableTo<SendMessageCommand>
{
    [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
    public Guid Id { get; set; }
    [FromBody]
    public Guid ReceiverId { get; set; }
    [FromBody]
    public string TextMessage { get; set; } = string.Empty;
}