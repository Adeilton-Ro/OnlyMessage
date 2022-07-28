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
        throw new NotImplementedException();
    }
}