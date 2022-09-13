namespace Application.Feature.Auth.RefreshToken;
public record RefreshTokenCommandResponse(string Token, DateTime Expiration, string RefreshToken);