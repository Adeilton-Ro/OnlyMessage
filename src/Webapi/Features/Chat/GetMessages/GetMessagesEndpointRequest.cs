using Application.Feature.Chat.GetMessages;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils.Mapping;

namespace Webapi.Features.Chat.GetMessages;
public class GetMessagesEndpointRequest : IMappeableTo<GetMessagesQuery>
{
    [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
    public Guid Id { get; set; }
    [FromRoute]
    public Guid FriendId { get; set; }
}