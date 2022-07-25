using Domain.Entities.Abstract;

namespace Domain.Entities;
public class Group : Entity, IReceiver<Group>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<UserGroup> UsersGroup { get; set; }
    public IEnumerable<Message<Group>> Messages { get; set; }
}