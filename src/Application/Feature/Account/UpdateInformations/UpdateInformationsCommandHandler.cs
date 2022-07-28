using Application.Services.Crypto;
using infrastructure.DataBase.Abstract.Interfaces;
using infrastructure.DataBase.Abstract.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Utils.Results;

namespace Application.Feature.Account.UpdateInformations;
public class UpdateInformationsCommandHandler : IRequestWithResultHandler<UpdateInformationsCommand>
{
    private readonly IUserRepository userRepository;
    private readonly ICryptographyService cryptographyService;
    private readonly IUnitOfWork unitOfWork;

    public UpdateInformationsCommandHandler(IUserRepository userRepository, ICryptographyService cryptographyService,
        IUnitOfWork unitOfWork)
    {
        this.userRepository = userRepository;
        this.cryptographyService = cryptographyService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateInformationsCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(request.Id, cancellationToken);

        if (await userRepository.GetByUserName(request.Username, cancellationToken) is not null)
            if (user.UserName != request.Username)
                return Result.OfUnauthorizedResult("Nome de usuário já existe").Build();

        user.UserName = request.Username;
        if (!string.IsNullOrEmpty(request.Password))
        {
            var salt = cryptographyService.CreateSalt();
            user.Salt = salt;
            user.Password = cryptographyService.Hash(request.Password, salt);
        }

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.OfSuccess().Build();
    }
}