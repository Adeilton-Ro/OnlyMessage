using Application.Feature.Auth.Login;
using AutoMapper;
using Utils.Mapping;

namespace Webapi.Features.Auth.Login;
public record LoginEndpointResponse(string Token, string RefreshToken, LoginUserEndpointResponse User, DateTime Expiration) : IMappeableFrom<LoginCommandResponse>
{
    public void ConfigureMap(Profile profile)
    {
        profile.CreateMap(typeof(LoginCommandResponse), GetType());
        profile.CreateMap(typeof(LoginUserCommandResponse), typeof(LoginUserEndpointResponse));
    }
}
public record LoginUserEndpointResponse(string Username, string ImageUrl);