using infrastructure.DataBase.Abstract.Interfaces;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Infrastructure.DataBase.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataBase;
public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = configuration.GetValue<string>("Database_Connection"), Cache = SqliteCacheMode.Shared };
        var sqliteConnection = new SqliteConnection(connectionStringBuilder.ToString());
        sqliteConnection.Open();
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(sqliteConnection));

        services.AddScoped(sp => sp.GetRequiredService<AppDbContext>() as IUnitOfWork);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();

        return services;
    }
}