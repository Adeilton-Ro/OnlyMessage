using Application.Feature.Chat.GetMessages;
using AutoMapper;
using Utils.Mapping;

namespace Webapi.Features.Chat.GetMessages;
public record GetMessagesEndpointResponse(
    GetUserMessagesEndpointResponse MyMessages, GetUserMessagesEndpointResponse FriendMessages) : IMappeableFrom<GetMessagesQueryResponse>
{
    public void ConfigureMap(Profile profile)
    {
        profile.CreateMap(typeof(GetMessagesQueryResponse), GetType());
        profile.CreateMap(typeof(GetUserMessagesQueryResponse), typeof(GetUserMessagesEndpointResponse));
        profile.CreateMap(typeof(GetMessageQueryResponse),typeof(GetMessageEndpointResponse));
    }
}
public record GetUserMessagesEndpointResponse(Guid Id, IEnumerable<GetMessageEndpointResponse> Messages);
public record GetMessageEndpointResponse(Guid Id, string TextMessage, DateTime SendeTime);