using Application;
using Application.Services.Crypto;
using Application.Services.Token;
using Domain.Entities;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Infrastructure.DataBase;
using Infrastructure.FileSystem;
using System.Text;
using Utils;
using Webapi.MiddlewareConfigs;

namespace Webapi;
public static class DependencyInjection
{
    public static IServiceCollection AddJwtAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JWTTokenServiceOptions>()
            .Configure<IConfiguration>(
                (options, configuration) =>
                {
                    options.TokenSecret = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JWTOKEN_DECRYPT"));
                    options.RefreshTokenSecret = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JWREFRESHTOKEN_DECRYPT"));
                }
            );

        services.AddAuthenticationJWTBearer(configuration.GetValue<string>("JWTOKEN_DECRYPT"));

        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors();
        services.AddFastEndpoints();
        services.AddJwtAuthorization(configuration);
        services.AddSignalR();
        services.AddSwaggerDoc();
        services.AddAutoMapping();
        services.AddApplication();
        services.AddDatabase(configuration);
        services.AddFileSystem();

        return services;
    }
    public static WebApplication Configure(this WebApplication app, IConfiguration configuration)
    {
        app.AddInfosInDB();
        app.UseCors(options =>
        {
            options.WithOrigins(configuration.GetValue<string>("Origin"));
            options.AllowAnyMethod();
            options.AllowAnyHeader();
            options.AllowCredentials();
        });

        app.UseMiddleware<WebSocketsMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.AddHubsMappers();

        app.UseFastEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi3(s => s.ConfigureDefaults());
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        return app;
    }

    public static WebApplication AddInfosInDB(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var crypto = scope.ServiceProvider.GetRequiredService<ICryptographyService>();

            context.Database.EnsureCreated();
            if (!context.Users.Any())
            {
                var salt = crypto.CreateSalt();
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "Adeilton",
                    Password = crypto.Hash("12345678", salt),
                    Salt = salt,
                    Uri = "/UsersAvatar/DefaultProfile.jpg"
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        return app;
    }
}