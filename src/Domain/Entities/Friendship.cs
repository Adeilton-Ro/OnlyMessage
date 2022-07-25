using Domain.Entities.Abstract;

namespace Domain.Entities;
public class Friendship : Entity
{
    public Guid FriendId { get; set; }
    public User Friend { get;  set; }
}