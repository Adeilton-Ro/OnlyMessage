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
        var user = await userRepository.GetById(request.Id, cancellationToken);

        if (user is null)
            return Result.OfNotFoundResult("Usuário").Build<ChangeImageCommandResponse>();

        if (user.Uri != fileRepository.GetDefaultImagePath() && request.Image.Length == 0)
        {
            fileRepository.DeleteImage(user.Uri);
            user.Uri = fileRepository.GetDefaultImagePath();
        }
        if (request.Image.Length > 0)
        {
            fileRepository.SaveUserAvatarImage(user.Id.ToString(), request.Image, request.Extension);
            user.Uri = fileRepository.GetUserAvatarFilePath(user.Id.ToString(), request.Extension);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.OfSuccess(new ChangeImageCommandResponse(user.Uri)).Build();
    }
}