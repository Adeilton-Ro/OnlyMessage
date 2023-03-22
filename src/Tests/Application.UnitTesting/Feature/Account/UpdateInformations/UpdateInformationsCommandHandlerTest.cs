using Application.Services.Crypto;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Application.Feature.Account.UpdateInformations;
using Utils.Results;
using infrastructure.DataBase.Abstract.Interfaces;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;

namespace Application.UnitTesting.Feature.Account.UpdateInformations;
public class UpdateInformationsCommandHandlerTest
{
    public static (Mock<IUserRepository> userRepositoryMock, ICryptographyService cryptographyService, 
        IList<User> context, Mock<IUnitOfWork> unitOfWork) GetDependency()
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
            },
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "OtherUser",
                Password = crypto.Hash("Password", salt),
                Salt = salt
            },
        };

        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(ur => ur.Update(It.IsAny<User>()))
            .Callback<User>(user => { context.Remove(context.FirstOrDefault(c => c.Id == user.Id)); context.Add(user); });
        userRepository.Setup(ur => ur.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns<Guid, CancellationToken>((id, ct) => Task.FromResult(context.FirstOrDefault(c => c.Id == id)));
        userRepository.Setup(ur => ur.GetByUserName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns<string, CancellationToken>((username, ct) => Task.FromResult(context.FirstOrDefault(c => c.UserName == username)));

        return (userRepository, crypto, context, new Mock<IUnitOfWork>());
    }

    [Theory]
    [InlineData("Name")]
    [InlineData("User")]
    public async Task The_Informations_Was_Changed_With_Success(string name)
    {
        (Mock<IUserRepository> userRepositoryMock, ICryptographyService cryptographyService,
        IList<User> context, Mock<IUnitOfWork> unitOfWork) = GetDependency();

        var request = new UpdateInformationsCommand(context.First().Id, name, "NewPassword");
        var handler = new UpdateInformationsCommandHandler(userRepositoryMock.Object, cryptographyService, unitOfWork.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Success);
        Assert.Empty(result.Errors);
        var compare = context.FirstOrDefault(c => c.Id == request.Id);
        Assert.Equal(compare.UserName, request.Username);
        Assert.True(cryptographyService.Compare(compare.Password, request.Password, compare.Salt));
    }

    [Fact]
    public async Task The_UserName_Alredy_Exist()
    {
        (Mock<IUserRepository> userRepositoryMock, ICryptographyService cryptographyService,
        IList<User> context, Mock<IUnitOfWork> unitOfWork) = GetDependency();

        var request = new UpdateInformationsCommand(context.First().Id, "OtherUser", "NewPassword");
        var handler = new UpdateInformationsCommandHandler(userRepositoryMock.Object, cryptographyService, unitOfWork.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Failure);
        Assert.NotEmpty(result.Errors);
    }
}