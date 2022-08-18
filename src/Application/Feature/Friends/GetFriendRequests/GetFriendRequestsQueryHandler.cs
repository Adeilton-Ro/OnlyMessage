using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Friends.GetFriendRequests;
public class GetFriendRequestsQueryHandler : IRequestWithResultHandler<GetFriendRequestsQuery, IEnumerable<GetFriendRequestsQueryResponse>>
{
    private readonly IFriendRequestRepository friendRequestRepository;
    private readonly IUserRepository userRepository;

    public GetFriendRequestsQueryHandler(IFriendRequestRepository friendRequestRepository, IUserRepository userRepository)
    {
        this.friendRequestRepository = friendRequestRepository;
        this.userRepository = userRepository;
    }
    public async Task<Result<IEnumerable<GetFriendRequestsQueryResponse>>> Handle(GetFriendRequestsQuery request, CancellationToken cancellationToken)
    {
        var friendRequest = friendRequestRepository.GetAll(request.Id, cancellationToken);
        var users = userRepository.GetByIds(friendRequest.Select(fr => fr.UserId));
        var response = friendRequest.Select(fr => 
        new GetFriendRequestsQueryResponse(
            fr.Id, 
            users.FirstOrDefault(u => u.Id == fr.UserId).UserName,
            users.FirstOrDefault(u => u.Id == fr.UserId).Uri,
            fr.Created
            ));
        return Result.OfSuccess(response).Build();
    }
}