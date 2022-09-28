namespace Application.Feature.Friends.GetFriendships;
public record GetFriendshipsQueryResponse(Guid Id, Guid FriendId, string UserName, string ImageUrl, GetMessagesFriendshipsQueryResponse LastMessage);

public record GetMessagesFriendshipsQueryResponse(Guid Id, string TextMessage, DateTime SendeTime, Guid WhoSend);