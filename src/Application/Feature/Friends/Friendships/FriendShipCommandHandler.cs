using Domain.Entities;
using infrastructure.DataBase.Abstract.Interfaces;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Friends.Friendships;
public class FriendShipCommandHandler : IRequestWithResultHandler<FriendShipCommand>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IFriendRequestRepository friendRequestRepository;
    private readonly IFriendshipRepository friendshipRepository;

    public FriendShipCommandHandler(IUnitOfWork unitOfWork, IFriendRequestRepository friendRequestRepository,
        IFriendshipRepository friendshipRepository)
    {
        this.unitOfWork = unitOfWork;
        this.friendRequestRepository = friendRequestRepository;
        this.friendshipRepository = friendshipRepository;
    }
    public async Task<Result> Handle(FriendShipCommand request, CancellationToken cancellationToken)
    {
        var friendRequest = await friendRequestRepository.GetById(request.Id, cancellationToken);
        if (friendRequest == null)
            return Result.OfNotFoundResult("Pedido de amizade").Build();

        if (friendRequest.FriendId != request.UserId)
            return Result.OfUnauthorizedResult("Você não tem permissão para fazer isso!").Build();

        friendRequestRepository.Delete(friendRequest);

        if (!request.IsAccepted)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.OfSuccess().Build();
        }

        var userFriendship = new Friendship
        {
            Id = Guid.NewGuid(),
            UserId = friendRequest.UserId,
            FriendId = friendRequest.FriendId,
        };
        await friendshipRepository.Save(userFriendship, cancellationToken);

        var friendsFriendship = new Friendship
        {
            Id = Guid.NewGuid(),
            UserId = friendRequest.FriendId,
            FriendId = friendRequest.UserId,
        };
        await friendshipRepository.Save(friendsFriendship, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.OfSuccess().Build();
    }
}