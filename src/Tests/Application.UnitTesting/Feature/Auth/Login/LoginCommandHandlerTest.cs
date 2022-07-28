using Application.Feature.Auth.Login;
using Application.Services.Crypto;
using Application.Services.Token;
using Domain.Entities;
using infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils.Results;
using Xunit;

namespace Application.UnitTesting.Feature.Auth.Login;
public class LoginCommandHandlerTest
{
    public static (Mock<IUserRepository> userRepositoryMock, ICryptographyService cryptographyService
        , Mock<ITokenService> tokenServiceMock, IList<User> context) GetDependency()
    {
        var crypto = new CryptographyService();
        var salt = crypto.CreateSalt();
        var context = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "User",
                Password = crypto.Hash("Password", salt),
                Salt = salt
            }
        };
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(ur => ur.GetByUserName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns<string, CancellationToken>((username, ct) => Task.FromResult(context.FirstOrDefault(c => c.UserName == username)));
        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(ts => ts.GenerateToken(It.IsAny<User>())).Returns<User>(u => $"Token of {u.UserName}");

        return (userRepository, crypto, tokenService, context);
    }

    [Fact]
    public async Task Login_With_Success()
    {
        (Mock<IUserRepository> userRepositoryMock, ICryptographyService cryptographyService
        , Mock<ITokenService> tokenServiceMock, IList<User> context) = GetDependency();

        var request = new LoginCommand("User", "Password");
        var handler = new LoginCommandHandler(userRepositoryMock.Object, cryptographyService, tokenServiceMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Success);
        Assert.Empty(result.Errors);
        Assert.Equal(result.Content.Token, $"Token of {context.First().UserName}");
    }

    [Theory]
    [InlineData("user")]
    [InlineData("otherUser")]
    public async Task The_Username_Does_Not_Exist(string username)
    {
        (Mock<IUserRepository> userRepositoryMock, ICryptographyService cryptographyService
        , Mock<ITokenService> tokenServiceMock, IList<User> context) = GetDependency();

        var request = new LoginCommand(username, "Password");
        var handler = new LoginCommandHandler(userRepositoryMock.Object, cryptographyService, tokenServiceMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.InsufficientPermission);
        Assert.NotEmpty(result.Errors);
    }

    [Theory]
    [InlineData("password")]
    [InlineData("12345678")]
    public async Task The_Password_Was_Wrong(string password)
    {
        (Mock<IUserRepository> userRepositoryMock, ICryptographyService cryptographyService
        , Mock<ITokenService> tokenServiceMock, IList<User> context) = GetDependency();

        var request = new LoginCommand("Cochicho", password);
        var handler = new LoginCommandHandler(userRepositoryMock.Object, cryptographyService, tokenServiceMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.InsufficientPermission);
        Assert.NotEmpty(result.Errors);
    }
}