using Application.Services.Crypto;
using Application.Services.Token;
using infrastructure.DataBase.Abstract.Interfaces.Repositories;
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
            return Result.OfUnauthorizedResult("UserName ou senha incorretos!").Build<LoginCommandResponse>();

        if (!cryptography.Compare(user.Password, request.Password, user.Salt))
            return Result.OfUnauthorizedResult("UserName ou senha incorretos!").Build<LoginCommandResponse>();

        var token = jwtTokenService.GenerateToken(user);

        return Result.OfSuccess(new LoginCommandResponse(token, DateTime.UtcNow.AddHours(8))).Build();
    }
}