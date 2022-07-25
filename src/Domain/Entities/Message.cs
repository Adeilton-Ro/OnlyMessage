using Domain.Entities.Abstract;

namespace Domain.Entities;
public class Message<T> : Entity where T : Entity, IReceiver<T>
{
    public Guid SenderId { get; set; }
    public User Sender { get; set; }
    public string TextMessage { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public Guid ReceiverId { get; set; }
    public T Receiver { get; set; }
}