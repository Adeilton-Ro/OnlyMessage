using Domain.Entities;
using System.Security.Claims;

namespace Application.Services.Token;

public interface ITokenService
{
    string GenerateToken(User id);
    string GenerateToken(IEnumerable<Claim> claim);
    string GenerateRefreshToken();
    bool IsValidRefreshToken(string token);
    bool IsValidToken(string token);
    ClaimsPrincipal GetClaimFromToken(string token);
}
