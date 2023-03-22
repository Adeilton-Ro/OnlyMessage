namespace Application.Services.Token;

public class JWTTokenServiceOptions
{
    public byte[] TokenSecret { get; set; }
    public byte[] RefreshTokenSecret { get; set; }
}
