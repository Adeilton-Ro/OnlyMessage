namespace Application.Feature.Friends.GetFriendships;
public record GetFriendshipsQueryResponse(Guid Id, Guid FriendId, string UserName, string ImageUrl);