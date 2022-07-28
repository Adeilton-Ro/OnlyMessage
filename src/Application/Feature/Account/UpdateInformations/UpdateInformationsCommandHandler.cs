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

    public Task<Result> Handle(UpdateInformationsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}