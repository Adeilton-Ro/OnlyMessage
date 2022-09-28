using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Friends.GetFriendships
{
    public class GetFriendshipsQueryHandler : IRequestWithResultHandler<GetFriendshipsQuery, IEnumerable<GetFriendshipsQueryResponse>>
    {
        private readonly IFriendshipRepository friendshipRepository;
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;

        public GetFriendshipsQueryHandler(IFriendshipRepository friendshipRepository, IUserRepository userRepository, IMessageRepository messageRepository)
        {
            this.friendshipRepository = friendshipRepository;
            this.userRepository = userRepository;
            this.messageRepository = messageRepository;
        }
        public async Task<Result<IEnumerable<GetFriendshipsQueryResponse>>> Handle(GetFriendshipsQuery request, CancellationToken cancellationToken)
        {
            var friendships = friendshipRepository.GetFriends(request.Id, cancellationToken);
            var friendsId = friendships.Select(fs => fs.UserId);
            var user = userRepository.GetByIds(friendsId);
            var messages = messageRepository.LastsMesssage(request.Id, friendsId);
            var response = user.Select(u =>
            {
                var lastMessage = 
                    messages.FirstOrDefault(m => m.SenderId == u.Id || m.SenderId == request.Id && m.ReceiverId == request.Id || m.ReceiverId == u.Id);
                return new GetFriendshipsQueryResponse(
                    friendships.FirstOrDefault(fs => fs.UserId == u.Id).Id,
                    u.Id,
                    u.UserName,
                    u.Uri,
                    new GetMessagesFriendshipsQueryResponse(lastMessage.Id, lastMessage.TextMessage, lastMessage.Created, lastMessage.SenderId)
                );
            });

            return Result.OfSuccess(response).Build();
        }
    }
}
