using Application.Feature.Friends.RemoveFriendship;
using Domain.Entities;
using infrastructure.DataBase.Abstract.Interfaces;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils.Results;
using Xunit;

namespace Application.UnitTesting.Feature.Friends.RemoveFriendship;
public class RemoveFriendshipCommandHandlerTest
{
    public static (Mock<IUnitOfWork> unitOfWorkMock, Mock<IFriendshipRepository> friendshipRepositoryMock,
        List<Friendship> context) GetDependency()
    {
        var context = new List<Friendship>
            {
                new Friendship
                {
                    Id = Guid.Parse("afce2ec4c3334de1adbab340e23e7e0b"),
                    UserId = Guid.Parse("02a74657-3269-481e-9f98-a0a5a4832301"),
                    FriendId = Guid.Parse("4839251f-9d73-4feb-b954-b89fdb7e3d5c")
                },
                new Friendship
                {
                    Id = Guid.Parse("e8468add-f1bf-4eae-af76-6a5bf76acf10"),
                    UserId = Guid.Parse("4839251f-9d73-4feb-b954-b89fdb7e3d5c"),
                    FriendId = Guid.Parse("02a74657-3269-481e-9f98-a0a5a4832301")
                }
            };
        var friendshipRepository = new Mock<IFriendshipRepository>();
        friendshipRepository.Setup(fs => fs.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns((Guid id, CancellationToken ct) => Task.FromResult(context.FirstOrDefault(c => c.Id == id)));

        friendshipRepository.Setup(fs => fs.GetByFriendId(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns((Guid id, Guid fId, CancellationToken ct) => Task.FromResult(context.FirstOrDefault(fs => fs.FriendId == fId && fs.UserId == id)));

        friendshipRepository.Setup(fs => fs.Remove(It.IsAny<Friendship>()))
            .Callback((Friendship fs) => context.Remove(fs));

        return (new Mock<IUnitOfWork>(), friendshipRepository, context);
    }

    [Fact]
    public async Task Removed_With_Success()
    {
        (Mock<IUnitOfWork> unitOfWorkMock, Mock<IFriendshipRepository> friendshipRepositoryMock,
        List<Friendship> context) = GetDependency();

        var request = new RemoveFriendshipCommand(Guid.Parse("e8468add-f1bf-4eae-af76-6a5bf76acf10"), Guid.Parse("02a74657-3269-481e-9f98-a0a5a4832301"));
        var handler = new RemoveFriendCommandHandler(friendshipRepositoryMock.Object, unitOfWorkMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Success);
        Assert.Empty(result.Errors);
        Assert.Equal(context.Count, 0);
    }

    [Fact]
    public async Task The_Friendship_Doesnt_Exist()
    {
        (Mock<IUnitOfWork> unitOfWorkMock, Mock<IFriendshipRepository> friendshipRepositoryMock,
        List<Friendship> context) = GetDependency();

        var request = new RemoveFriendshipCommand(Guid.Parse("85ebe3b4-a87c-4160-96a2-cfcd690d8a12"), Guid.Parse("02a74657-3269-481e-9f98-a0a5a4832301"));
        var handler = new RemoveFriendCommandHandler(friendshipRepositoryMock.Object, unitOfWorkMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.NotFound);
        Assert.NotEmpty(result.Errors);
        Assert.Equal(context.Count, 2);
    }

    [Fact]
    public async Task Other_People_Try_Remove_A_Friendship()
    {
        (Mock<IUnitOfWork> unitOfWorkMock, Mock<IFriendshipRepository> friendshipRepositoryMock,
        List<Friendship> context) = GetDependency();

        var request = new RemoveFriendshipCommand(Guid.Parse("afce2ec4c3334de1adbab340e23e7e0b"), Guid.Parse("1fc7ab15-c0e1-4d27-944f-e37773fb1816"));
        var handler = new RemoveFriendCommandHandler(friendshipRepositoryMock.Object, unitOfWorkMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.InsufficientPermission);
        Assert.NotEmpty(result.Errors);
        Assert.Equal(context.Count, 2);
    }
}