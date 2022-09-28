using Application.Feature.Friends.GetFriendships;
using AutoMapper;
using Utils.Mapping;

namespace Webapi.Features.Friends.GetFriendships;
public record GetFriendshipsEndpointResponse(Guid Id, Guid FriendId, string UserName, string ImageUrl, GetMessagesFriendshipsQueryResponse LastMessage) : IMappeableFrom<GetFriendshipsQueryResponse>
{
    public void ConfigureMap(Profile profile)
    {
        profile.CreateMap(typeof(GetFriendshipsQueryResponse), GetType());
        profile.CreateMap(typeof(GetMessagesFriendshipsQueryResponse), typeof(GetMessagesFriendshipsEndpointResponse));
    }
}

public record GetMessagesFriendshipsEndpointResponse(Guid Id, string TextMessage, DateTime SendeTime, Guid WhoSend);