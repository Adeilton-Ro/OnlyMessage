using Domain.Entities;

namespace infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IUserRepository
{
    public Task<User> GetByUserName(string userName, CancellationToken cancellationToken);
    public Task Create(User user, CancellationToken cancellationToken);
    public Task<User> GetById(Guid Id, CancellationToken cancellationToken);
    public void Update(User user);
}