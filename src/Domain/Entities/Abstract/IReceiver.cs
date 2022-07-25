namespace Domain.Entities.Abstract;
public interface IReceiver<T> where T : Entity, IReceiver<T>
{
    IEnumerable<Message<T>> Messages { get; set; }
}