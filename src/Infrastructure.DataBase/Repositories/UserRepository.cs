﻿using Domain.Entities;
using infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext context;

    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task Create(User user, CancellationToken cancellationToken)
     => await context.Users.AddAsync(user, cancellationToken);

    public async Task<User> GetByUserName(string userName, CancellationToken cancellationToken)
     => await context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
}