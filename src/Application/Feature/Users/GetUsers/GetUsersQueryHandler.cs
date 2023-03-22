using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Users.GetUsers;
public class GetUsersQueryHandler : IRequestWithResultHandler<GetUsersQuery, IEnumerable<GetUsersQueryResponse>>
{
    private readonly IUserRepository userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<GetUsersQueryResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var response = (await userRepository.Search(request.Id, request.Search.ToLower()))
            .Select(u => new GetUsersQueryResponse(u.User.Id, u.User.UserName, u.User.Uri, u.IsAlredyRequested));

        return Result.OfSuccess(response).Build();
    }
}