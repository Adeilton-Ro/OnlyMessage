using Application.Services.Token;
using Utils.Results;

namespace Application.Feature.Auth.RefreshToken;
public class RefreshTokenCommandHandler : IRequestWithResultHandler<RefreshTokenCommand, RefreshTokenCommandResponse>
{
    private readonly ITokenService tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
        this.tokenService = tokenService;
    }
    public async Task<Result<RefreshTokenCommandResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!tokenService.IsValidToken(request.Token))
            return Result.OfFailure("Token inválido").Build<RefreshTokenCommandResponse>();

        if(!tokenService.IsValidRefreshToken(request.RefreshToken))
            return Result.OfFailure("Token inválido").Build<RefreshTokenCommandResponse>();

        var claim = tokenService.GetClaimFromToken(request.Token);

        var response = new RefreshTokenCommandResponse
            (
            tokenService.GenerateToken(claim.Claims),
            DateTime.UtcNow.AddHours(2),
            tokenService.GenerateRefreshToken()
            );

        return Result.OfSuccess(response).Build();
    }
}