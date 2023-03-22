using Utils.Results;

namespace Application.Feature.Users.GetUsers;
public record GetUsersQuery(Guid Id, string Search) : IRequestWithResult<IEnumerable<GetUsersQueryResponse>>;