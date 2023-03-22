using Application.Feature.Chat.GetMessages;
using AutoMapper;
using Utils.Mapping;

namespace Webapi.Features.Chat.GetMessages;
public record GetMessagesEndpointResponse(Guid Id, IEnumerable<MessageEndpointResponse> Messages) : IMappeableFrom<GetMessagesQueryResponse>
{
    public void ConfigureMap(Profile profile)
    {
        profile.CreateMap(typeof(GetMessagesQueryResponse), GetType());
        profile.CreateMap(typeof(MessageQueryResponse),typeof(MessageEndpointResponse));
    }
}
public record MessageEndpointResponse(Guid Id, string TextMessage, DateTime SendeTime);