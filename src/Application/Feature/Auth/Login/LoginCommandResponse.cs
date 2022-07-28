namespace Application.Feature.Auth.Login;

public record LoginCommandResponse(string Token, DateTime Expiration);