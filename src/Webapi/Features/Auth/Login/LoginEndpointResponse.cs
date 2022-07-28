using Application.Feature.Auth.Login;
using Utils.Mapping;

namespace Webapi.Features.Auth.Login;
public record LoginEndpointResponse(string Token, DateTime Expiration) : IMappeableFrom<LoginCommandResponse>;