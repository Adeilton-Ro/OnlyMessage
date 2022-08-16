using Application.Services.GetTimeZone;
using Domain.Entities;
using infrastructure.DataBase.Abstract.Interfaces;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Friends.FriendRequests;
public class FriendRequestCommandHandler : IRequestWithResultHandler<FriendRequestCommand, FriendRequestCommandResponse>
{
    private readonly IUserRepository userRepository;
    private readonly IFriendRequestRepository friendRequestRepository;
    private readonly IFriendshipRepository friendshipRepository;
    private readonly IGetTimeZone getTimeZone;
    private readonly IUnitOfWork unitOfWork;

    public FriendRequestCommandHandler(IUserRepository userRepository, IFriendRequestRepository friendRequestRepository,
        IFriendshipRepository friendshipRepository, IGetTimeZone getTimeZone, IUnitOfWork unitOfWork)
    {
        this.userRepository = userRepository;
        this.friendRequestRepository = friendRequestRepository;
        this.friendshipRepository = friendshipRepository;
        this.getTimeZone = getTimeZone;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<FriendRequestCommandResponse>> Handle(FriendRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(request.Id, cancellationToken);
        if (user is null)
            return Result.OfNotFoundResult("Usuário").Build<FriendRequestCommandResponse>();

        var friend = await userRepository.GetById(request.FriendId, cancellationToken);
        if (friend is null)
            return Result.OfNotFoundResult("Amigo").Build<FriendRequestCommandResponse>();

        if (await friendRequestRepository.IsAlredyRequested(request.Id, request.FriendId, cancellationToken))
            return Result.OfFailure("Solicitação já enviada").Build<FriendRequestCommandResponse>();

        if (await friendshipRepository.IsAlreadyFriends(request.Id, request.FriendId, cancellationToken))
            return Result.OfFailure("Vocês já são amigos").Build<FriendRequestCommandResponse>();

        var friendRequest = new FriendRequest
        {
            Id = Guid.NewGuid(),
            UserId = request.Id,
            FriendId = request.FriendId,
            Created = getTimeZone.GetApplicationTimeZone()
        };


        await friendRequestRepository.Save(friendRequest, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.OfSuccess(new FriendRequestCommandResponse(friendRequest.Id, user.UserName, user.Uri)).Build();
    }
}