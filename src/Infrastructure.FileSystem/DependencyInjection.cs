using Infrastructure.FileSystem.Abstraction.Interfaces.IFileRepositories;
using Infrastructure.FileSystem.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.FileSystem;
public static class DependencyInjection
{
    public static IServiceCollection AddFileSystem(this IServiceCollection services)
    {
        services.AddScoped<IFileRepository, FileRepository>();
        return services;
    }
}