namespace Application.Feature.Auth.Login;
public record LoginCommandResponse(string Token, string RefreshToken, LoginUserCommandResponse User ,DateTime Expiration);

public record LoginUserCommandResponse(string Username, string ImageUrl);