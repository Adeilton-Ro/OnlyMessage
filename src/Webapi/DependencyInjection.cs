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
                var userList = new List<User>
                {
                    new User
                    {
                        Id = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550"),
                        UserName = "Adeilton",
                        Password = crypto.Hash("12345678", salt),
                        Salt = salt,
                        Uri = "/UsersAvatar/DefaultProfile.jpg",
                    },
                    new User
                    {
                        Id = Guid.Parse("c37d25fa-deb0-4831-b3ef-decebf498a13"),
                        UserName = "Luiz Felipe",
                        Password = crypto.Hash("12345678", salt),
                        Salt = salt,
                        Uri = "/UsersAvatar/DefaultProfile.jpg",
                    },
                    new User {
                        Id = Guid.Parse("7835052c-46e3-4058-a470-ec2453ca7fe3"),
                        UserName = "Cleber de Jesus",
                        Password = crypto.Hash("12345678", salt),
                        Salt = salt,
                        Uri = "/UsersAvatar/DefaultProfile.jpg"
                    },
                    new User
                    {
                        Id = Guid.Parse("d7b0542a-87fb-4234-9f66-410834d145ca"),
                        UserName = "Paulo Afonso",
                        Password = crypto.Hash("12345678", salt),
                        Salt = salt,
                        Uri = "/UsersAvatar/DefaultProfile.jpg"
                    },
                    new User
                    {
                        Id = Guid.Parse("50ccd318-44c8-482b-9391-9481295bbfed"),
                        UserName = "Andre Gustavo",
                        Password = crypto.Hash("12345678", salt),
                        Salt = salt,
                        Uri = "/UsersAvatar/DefaultProfile.jpg"
                    }
                };

                context.Users.AddRange(userList);

                var friendshipList = new List<Friendship>
                {
                    new Friendship
                    {
                        Id = Guid.Parse("3a95da3c-10ab-42cb-9c88-0552c45f8ce5"),
                        UserId = Guid.Parse("c37d25fa-deb0-4831-b3ef-decebf498a13"),
                        FriendId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550")
                    },
                    new Friendship
                    {
                        Id = Guid.Parse("84932544-7863-455d-abab-d2ab0f5865b0"),
                        FriendId = Guid.Parse("c37d25fa-deb0-4831-b3ef-decebf498a13"),
                        UserId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550")
                    },
                    new Friendship
                    {
                        Id = Guid.Parse("e21ad0ab-bfe8-4303-8fcd-129fe18f5531"),
                        UserId = Guid.Parse("7835052c-46e3-4058-a470-ec2453ca7fe3"),
                        FriendId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550")
                    },
                    new Friendship
                    {
                        Id = Guid.Parse("55c6514d-4143-41da-befd-298c9382ee74"),
                        UserId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550"),
                        FriendId = Guid.Parse("7835052c-46e3-4058-a470-ec2453ca7fe3")
                    },
                };
                context.Friendships.AddRange(friendshipList);

                var friendRequestList = new List<FriendRequest>
                {
                    new FriendRequest
                    {
                        Id = Guid.Parse("5ab3dd01-d21c-41c3-8aab-acd09476563d"),
                        UserId = Guid.Parse("d7b0542a-87fb-4234-9f66-410834d145ca"),
                        FriendId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550"),
                        Created = DateTime.UtcNow,
                    },
                    new FriendRequest
                    {
                        Id = Guid.Parse("e21ad0ab-bfe8-4303-8fcd-129fe18f5531"),
                        UserId = Guid.Parse("50ccd318-44c8-482b-9391-9481295bbfed"),
                        FriendId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550"),
                        Created = DateTime.UtcNow,
                    }
                };

                context.FriendRequests.AddRange(friendRequestList);

                var messageList = new List<Message<User>>
                {
                    new Message<User>
                    {
                        Id = Guid.Parse("7dc964d1-c1a8-45d3-b6ed-6f0d1ab20093"),
                        TextMessage = "Oi, sumido!",
                        SenderId = Guid.Parse("c37d25fa-deb0-4831-b3ef-decebf498a13"),
                        ReceiverId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550"),
                        Created = DateTime.Parse("2022-12-11 12:00:00")
                    },
                    new Message<User>
                    {
                        Id = Guid.Parse("fa546a9b-4b39-4096-afc9-c4b482f59024"),
                        TextMessage = "Oi!",
                        SenderId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550"),
                        ReceiverId = Guid.Parse("c37d25fa-deb0-4831-b3ef-decebf498a13"),
                        Created = DateTime.Parse("2022-12-11 12:01:00")
                    },
                    new Message<User>
                    {
                        Id = Guid.Parse("706dd3e8-3638-45cd-bb7c-8c070d84f05f"),
                        TextMessage = "Tudo bem?",
                        SenderId = Guid.Parse("c37d25fa-deb0-4831-b3ef-decebf498a13"),
                        ReceiverId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550"),
                        Created = DateTime.Parse("2022-12-11 12:01:23")
                    },
                    new Message<User>
                    {
                        Id = Guid.Parse("39222533-57ae-499d-af4f-03c720d1c1e0"),
                        TextMessage = "Tudo ótimo",
                        SenderId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550"),
                        ReceiverId = Guid.Parse("c37d25fa-deb0-4831-b3ef-decebf498a13"),
                        Created = DateTime.Parse("2022-12-11 12:02:00")
                    },
                    new Message<User>
                    {
                        Id = Guid.Parse("476e14fc-705e-487d-a124-577134353d06"),
                        TextMessage = "Bom dia",
                        SenderId = Guid.Parse("7835052c-46e3-4058-a470-ec2453ca7fe3"),
                        ReceiverId = Guid.Parse("92e891db-b13f-4aff-8f5a-997b5b41f550"),
                        Created = DateTime.Parse("2022-12-12 09:00:00")
                    },
                };

                context.UserMessages.AddRange(messageList);

                context.SaveChanges();
            }
        }
        return app;
    }
}