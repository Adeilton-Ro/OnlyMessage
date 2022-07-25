using Application.Services.Crypto;
using Domain.Entities;
using infrastructure.DataBase.Abstract.Interfaces;
using infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Infrastructure.FileSystem.Abstraction.Interfaces.IFileRepositories;
using Utils.Results;

namespace Application.Feature.Account.SignUp;
public class SignUpCommandHandler : IRequestWithResultHandler<SignUpCommand, SignUpCommandResponse>
{
    private readonly IUserRepository userRepository;
    private readonly ICryptographyService cryptography;
    private readonly IUnitOfWork unitOfWork;
    private readonly IFileRepository fileRepository;

    public SignUpCommandHandler(IUserRepository userRepository, ICryptographyService cryptography
        , IUnitOfWork unitOfWork, IFileRepository fileRepository)
    {
        this.userRepository = userRepository;
        this.cryptography = cryptography;
        this.unitOfWork = unitOfWork;
        this.fileRepository = fileRepository;
    }
    public async Task<Result<SignUpCommandResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByUserName(request.UserName, cancellationToken) is not null)
            return Result.OfUnauthorizedResult("UserName já está em uso!").Build<SignUpCommandResponse>();

        var salt = cryptography.CreateSalt();

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Password = cryptography.Hash(request.Password, salt),
            Salt = salt,
            Uri = fileRepository.GetDefaultImagePath()
        };

        await userRepository.Create(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.OfSuccess(new SignUpCommandResponse(user.Id)).Build();
    }
}