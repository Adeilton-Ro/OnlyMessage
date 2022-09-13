﻿using Domain.Entities;

namespace Infrastructure.DataBase.Abstract.Interfaces.Repositories;
public interface IUserRepository
{
    public Task<User> GetByUserName(string userName, CancellationToken cancellationToken);
    public Task Create(User user, CancellationToken cancellationToken);
    public Task<User> GetById(Guid Id, CancellationToken cancellationToken);
    public void Update(User user);
    IEnumerable<User> GetByIds(IEnumerable<Guid> ids);
    IEnumerable<User> Search(Guid id, string seach);
}