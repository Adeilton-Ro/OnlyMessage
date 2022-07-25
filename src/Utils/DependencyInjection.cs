using Microsoft.Extensions.DependencyInjection;
using Utils.Mapping;

namespace Utils;

public static class DependencyInjection
{
    public static IServiceCollection AddAutoMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMappingProfile>());
        return services;
    }

}
