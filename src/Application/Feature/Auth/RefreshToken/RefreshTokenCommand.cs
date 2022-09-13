using Utils.Results;

namespace Application.Feature.Auth.RefreshToken;
public record RefreshTokenCommand(string Token, string RefreshToken) : IRequestWithResult<RefreshTokenCommandResponse>;