using infrastructure.DataBase.Abstract.Interfaces;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Friends.RemoveFriendship
{
    public class RemoveFriendCommandHandler : IRequestWithResultHandler<RemoveFriendshipCommand>
    {
        private readonly IFriendshipRepository friendshipRepository;
        private readonly IUnitOfWork unitOfWork;

        public RemoveFriendCommandHandler(IFriendshipRepository friendshipRepository, IUnitOfWork unitOfWork)
        {
            this.friendshipRepository = friendshipRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(RemoveFriendshipCommand request, CancellationToken cancellationToken)
        {
            var userFriendship = await friendshipRepository.GetById(request.Id, cancellationToken);

            if (userFriendship == null)
                return Result.OfNotFoundResult("Amizade").Build();

            if (userFriendship.FriendId != request.UserId)
                return Result.OfFailure("Você não pode remover amizade de outras pessoas").Build();

            friendshipRepository.Remove(userFriendship);

            var friendFriendship = await friendshipRepository.GetByFriendId(userFriendship.FriendId, userFriendship.UserId, cancellationToken);
            
            if(friendFriendship == null)
                Result.OfNotFoundResult("Amizade").Build();

            friendshipRepository.Remove(friendFriendship);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.OfSuccess().Build();
        }
    }
}
