using Application.Feature.Auth.RefreshToken;
using Utils.Mapping;

namespace Webapi.Features.Auth.RefreshToken;
public record RefreshTokenEndpointResponse(string Token, DateTime Expiration, string RefreshToken) : IMappeableFrom<RefreshTokenCommandResponse>;