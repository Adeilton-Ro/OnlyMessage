using Application.Feature.Chat.GetMessages;
using AutoMapper;
using Utils.Mapping;

namespace Webapi.Features.Chat.GetMessages;
public record GetMessagesEndpointResponse(Guid Id, string TextMessage, DateTime SendeTime, GetUserMessagesEndpointResponse Sender, GetUserMessagesEndpointResponse Receiver) : IMappeableFrom<GetMessagesQueryResponse>
{
    public void ConfigureMap(Profile profile)
    {
        profile.CreateMap(typeof(GetMessagesQueryResponse), GetType());
        profile.CreateMap(typeof(GetUserMessagesQueryResponse), typeof(GetUserMessagesEndpointResponse));
    }
}

public record GetUserMessagesEndpointResponse(Guid Id, string Name, string ImageUrl);