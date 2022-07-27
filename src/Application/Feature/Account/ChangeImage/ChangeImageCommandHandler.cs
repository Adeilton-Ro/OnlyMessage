using infrastructure.DataBase.Abstract.Interfaces;
using infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Infrastructure.FileSystem.Abstraction.Interfaces.IFileRepositories;
using Utils.Results;

namespace Application.Feature.Account.ChangeImage;
public class ChangeImageCommandHandler : IRequestWithResultHandler<ChangeImageCommand, ChangeImageCommandResponse>
{
    private readonly IUserRepository userRepository;
    private readonly IFileRepository fileRepository;
    private readonly IUnitOfWork unitOfWork;

    public ChangeImageCommandHandler(IUserRepository userRepository, IFileRepository fileRepository, IUnitOfWork unitOfWork)
    {
        this.userRepository = userRepository;
        this.fileRepository = fileRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<ChangeImageCommandResponse>> Handle(ChangeImageCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}