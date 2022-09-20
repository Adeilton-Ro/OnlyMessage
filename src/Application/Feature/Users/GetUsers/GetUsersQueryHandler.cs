using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Users.GetUsers;
public class GetUsersQueryHandler : IRequestWithResultHandler<GetUsersQuery, IEnumerable<GetUsersQueryResponse>>
{
    private readonly IUserRepository userRepository;
    private readonly IFriendRequestRepository friendRequestRepository;

    public GetUsersQueryHandler(IUserRepository userRepository, IFriendRequestRepository friendRequestRepository)
    {
        this.userRepository = userRepository;
        this.friendRequestRepository = friendRequestRepository;
    }

    public async Task<Result<IEnumerable<GetUsersQueryResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = userRepository.Search(request.Id, request.Search.ToLower());
        var friendRequests = friendRequestRepository.

        return Result.OfSuccess().Build();
    }
}