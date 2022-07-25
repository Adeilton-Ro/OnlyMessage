using Domain.Entities.Abstract;

namespace Domain.Entities;
public class User : Entity, IReceiver<User>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string Uri { get; set; }
    public IEnumerable<UserGroup> UserGroups { get; set; }
    public IEnumerable<Friendship> Friendships { get; set; }
    public IEnumerable<FriendRequest> FriendsRequest { get; set; }
    public IEnumerable<Message<User>> Messages { get; set; }
}