using Domain.Entities.Abstract;

namespace Domain.Entities;
public class FriendRequest : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime Created { get; set; }
}