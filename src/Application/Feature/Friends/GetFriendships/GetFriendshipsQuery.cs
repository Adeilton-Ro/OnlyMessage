using Utils.Results;

namespace Application.Feature.Friends.GetFriendships;
public record GetFriendshipsQuery(Guid Id) : IRequestWithResult<IEnumerable<GetFriendshipsQueryResponse>>;