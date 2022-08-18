using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Friends.GetFriendships
{
    public class GetFriendshipsQueryHandler : IRequestWithResultHandler<GetFriendshipsQuery, IEnumerable<GetFriendshipsQueryResponse>>
    {
        private readonly IFriendshipRepository friendshipRepository;
        private readonly IUserRepository userRepository;

        public GetFriendshipsQueryHandler(IFriendshipRepository friendshipRepository, IUserRepository userRepository)
        {
            this.friendshipRepository = friendshipRepository;
            this.userRepository = userRepository;
        }
        public async Task<Result<IEnumerable<GetFriendshipsQueryResponse>>> Handle(GetFriendshipsQuery request, CancellationToken cancellationToken)
        {
            var friendships = friendshipRepository.GetFriends(request.Id, cancellationToken);
            var user = userRepository.GetByIds(friendships.Select(fs => fs.UserId));
            var response = user.Select(u =>
            new GetFriendshipsQueryResponse(
                friendships.FirstOrDefault(fs => fs.UserId == u.Id).Id,
                u.Id,
                u.UserName,
                u.Uri
            ));

            return Result.OfSuccess(response).Build();
        }
    }
}
