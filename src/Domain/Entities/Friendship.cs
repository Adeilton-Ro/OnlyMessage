using Domain.Entities.Abstract;

namespace Domain.Entities;
public class Friendship : Entity
{
    public Guid UserId { get; set; }
    public User User { get;  set; }
    public Guid FriendId { get; set; }
}