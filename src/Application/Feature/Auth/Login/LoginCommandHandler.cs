using Application.Services.Crypto;
using Application.Services.Token;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Auth.Login;
public class LoginCommandHandler : IRequestWithResultHandler<LoginCommand, LoginCommandResponse>
{
    private readonly IUserRepository userRepository;
    private readonly ICryptographyService cryptography;
    private readonly ITokenService jwtTokenService;

    public LoginCommandHandler(IUserRepository userRepository, ICryptographyService cryptography,
        ITokenService jwtTokenService)
    {
        this.userRepository = userRepository;
        this.cryptography = cryptography;
        this.jwtTokenService = jwtTokenService;
    }

    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUserName(request.UserName, cancellationToken);
        if (user is null)
            return Result.OfFailure("UserName ou senha incorretos!").Build<LoginCommandResponse>();

        if (!cryptography.Compare(user.Password, request.Password, user.Salt))
            return Result.OfFailure("UserName ou senha incorretos!").Build<LoginCommandResponse>();

        var token = jwtTokenService.GenerateToken(user);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        return Result.OfSuccess(
            new LoginCommandResponse(token, refreshToken,
            new LoginUserCommandResponse(user.UserName, user.Uri), 
            DateTime.UtcNow.AddHours(2))).Build();
    }
}