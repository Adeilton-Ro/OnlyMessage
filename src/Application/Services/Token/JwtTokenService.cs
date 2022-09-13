using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Application.Services.Token;

public class JwtTokenService : ITokenService
{
    private readonly byte[] key;
    private readonly byte[] refreshKey;

    public JwtTokenService(IOptions<JWTTokenServiceOptions> options)
    {
        key = options.Value.TokenSecret;
        refreshKey = options.Value.RefreshTokenSecret;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = "https://OnlyMessage",
            Audience = "OnlyMessage",
            Expires = DateTime.Now.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = "https://OnlyMessage",
            Audience = "OnlyMessage",
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "https://OnlyMessage",
            Audience = "OnlyMessage",
            Expires = DateTime.UtcNow.AddDays(30),
            TokenType = "rt+jwt",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(refreshKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public bool IsValidRefreshToken(string token)
        => new JsonWebTokenHandler().ValidateToken(token, 
            new TokenValidationParameters 
            { 
                ValidIssuer = "https://OnlyMessage",
                ValidAudience = "OnlyMessage",
                IssuerSigningKey = new SymmetricSecurityKey(refreshKey)
            }).IsValid;

    public bool IsValidToken(string token)
        => new JsonWebTokenHandler().ValidateToken(token,
            new TokenValidationParameters
            {
                ValidIssuer = "https://OnlyMessage",
                ValidAudience = "OnlyMessage",
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }).IsValid;

    public ClaimsPrincipal GetClaimFromToken(string token)
    {
        var tokenParameters = new TokenValidationParameters
        {
            ValidIssuer = "https://OnlyMessage",
            ValidAudience = "OnlyMessage",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ValidateToken(token, tokenParameters, out var securityToken);
    }
}
