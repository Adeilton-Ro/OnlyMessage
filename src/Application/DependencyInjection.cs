using Application.Behaviours.ErrorHandling;
using Application.Behaviours.ErrorHandling.Strategies;
using Application.Services.Crypto;
using Application.Services.GetTimeZone;
using Application.Services.Token;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddErrorHandlingPipeline();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<ICryptographyService, CryptographyService>();
        services.AddScoped<IGetTimeZone, GetTimeZone>();
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    public static IServiceCollection AddErrorHandlingPipeline(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ErrorHandlingBehaviour<,>));

        services.AddScoped<DefaultErrorHandlingStrategy>();

        return services;
    }
}