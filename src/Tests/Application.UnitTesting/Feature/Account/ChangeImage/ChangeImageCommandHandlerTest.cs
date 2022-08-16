using Application.Feature.Account.ChangeImage;
using Domain.Entities;
using infrastructure.DataBase.Abstract.Interfaces;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Infrastructure.FileSystem.Abstraction.Interfaces.IFileRepositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils.Results;
using Xunit;

namespace Application.UnitTesting.Feature.Account.ChangeImage;
public class ChangeImageCommandHandlerTest
{
    public static (Mock<IUserRepository> userRepositoryMock, Mock<IFileRepository> fileRepositoryMock
        , Mock<IUnitOfWork> unitOfWorkMock, IList<User> context) GetDependency()
    {
        var context = new List<User>
        {
            new User
            {
                Id = Guid.Parse("25e4178a-df47-4bb5-a98e-74e7dedc7a44"),
                Uri = "/UserAvatar/UserAvatar.png"
            }
        };
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(ur => ur.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns<Guid, CancellationToken>((id, ct) => Task.FromResult(context.FirstOrDefault(c => c.Id == id)));
        
        var fileRepository = new Mock<IFileRepository>();
        fileRepository.Setup(fr => fr.GetDefaultImagePath()).Returns(() => "/UserAvatar/DefaultImage.jpg");
        fileRepository.Setup(fr => fr.GetUserAvatarFilePath(It.IsAny<string>(), It.IsAny<string>()))
            .Returns<string, string>((id, extension) => "Image was changed with success");

        return (userRepository, fileRepository, new Mock<IUnitOfWork>() ,context);
    }
    
    [Fact]
    public async Task Image_Was_Change_With_Success()
    {
        (Mock<IUserRepository> userRepositoryMock, Mock<IFileRepository> fileRepositoryMock
        , Mock<IUnitOfWork> unitOfWorkMock, IList<User> context) = GetDependency();

        var image = new byte[] { 0, 0, 1 };
        var request = new ChangeImageCommand(context.First().Id, image, ".png");
        var handler = new ChangeImageCommandHandler(userRepositoryMock.Object, fileRepositoryMock.Object, unitOfWorkMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Success);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Content);
        Assert.Equal(result.Content.ImageUrl, "Image was changed with success");
    }

    [Fact]
    public async Task The_User_Doesnt_Exist()
    {
        (Mock<IUserRepository> userRepositoryMock, Mock<IFileRepository> fileRepositoryMock
        , Mock<IUnitOfWork> unitOfWorkMock, IList<User> context) = GetDependency();

        var image = new byte[] { 0, 0, 1 };
        var request = new ChangeImageCommand(Guid.Parse("c65ecaf1-f88e-4e49-98eb-87eaf4197df5"), image, ".png");
        var handler = new ChangeImageCommandHandler(userRepositoryMock.Object, fileRepositoryMock.Object, unitOfWorkMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.NotFound);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Content);
    }

    [Fact]
    public async Task The_Image_Was_Empty()
    {
        (Mock<IUserRepository> userRepositoryMock, Mock<IFileRepository> fileRepositoryMock
        , Mock<IUnitOfWork> unitOfWorkMock, IList<User> context) = GetDependency();

        var image = new byte[] { };
        var request = new ChangeImageCommand(context.First().Id, image, "");
        var handler = new ChangeImageCommandHandler(userRepositoryMock.Object, fileRepositoryMock.Object, unitOfWorkMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Success);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Content);
        Assert.Equal(result.Content.ImageUrl, "/UserAvatar/DefaultImage.jpg");
    }
}