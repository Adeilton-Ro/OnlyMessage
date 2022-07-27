using Application.Feature.Account.SignUp;
using Application.Services.Crypto;
using Domain.Entities;
using infrastructure.DataBase.Abstract.Interfaces;
using infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Infrastructure.FileSystem.Abstraction.Interfaces.IFileRepositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils.Results;
using Xunit;

namespace Application.UnitTesting.Feature.Account.SignUp;
public class SignUpCommandHandlerTest
{
    private static (Mock<IUserRepository> userRepository, ICryptographyService cryptography
        , Mock<IUnitOfWork> unitOfWork, Mock<IFileRepository> fileRepository
        , IList<User> context) GetDependency()
    {

        var context = new List<User>()
        {
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "Fulaninho"
            }
        };
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(ur => ur.Create(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((user, ct) => context.Add(user))
            .Returns<User, CancellationToken>((user, ct) => Task.CompletedTask);

        userRepository.Setup(ur => ur.GetByUserName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns<string, CancellationToken>((username, ct) => Task.FromResult(context.FirstOrDefault(c => c.UserName == username)));

        var fileRepository = new Mock<IFileRepository>();
        fileRepository.Setup(fr => fr.GetDefaultImagePath())
            .Returns(() => "/UserAvatar/DefaultImage.jpg");

        return (userRepository, new CryptographyService(), new Mock<IUnitOfWork>(), fileRepository, context);
    }

    [Fact]
    public async Task User_Is_Created_With_Success()
    {
        (Mock<IUserRepository> userRepository, ICryptographyService cryptography
        , Mock<IUnitOfWork> unitOfWork, Mock<IFileRepository> fileRepository, IList<User> context) = GetDependency();

        var request = new SignUpCommand("Roger", "12345678");

        var handler = new SignUpCommandHandler(userRepository.Object, cryptography, unitOfWork.Object, fileRepository.Object);
        var result = await handler.Handle(request, CancellationToken.None);


        Assert.Equal(context.Count, 2);
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Success);
        Assert.NotNull(result.Content);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task Username_Alredy_Exist()
    {
        (Mock<IUserRepository> userRepository, ICryptographyService cryptography
        , Mock<IUnitOfWork> unitOfWork, Mock<IFileRepository> fileRepository, IList<User> context) = GetDependency();

        var request = new SignUpCommand("Fulaninho", "12345678");

        var handler = new SignUpCommandHandler(userRepository.Object, cryptography, unitOfWork.Object, fileRepository.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(context.Count, 1);
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.InsufficientPermission);
        Assert.Null(result.Content);
        Assert.NotEmpty(result.Errors);
    }
}