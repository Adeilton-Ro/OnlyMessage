using Domain.Entities;

namespace Application.Services.Token;

public interface ITokenService
{
    string GenerateToken(User user);
}
