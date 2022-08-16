using Application.Feature.Friends.FriendRequests;
using Application.Services.GetTimeZone;
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

namespace Application.UnitTesting.Feature.Friends.FriendRequests;

public class FriendRequestCommandHandlerTest
{

    public static (Mock<IUserRepository> userRepositoryMock, Mock<IFriendRequestRepository> friendRequestRepositoryMock,
        Mock<IFriendshipRepository> friendshipRepositoryMock, IList<FriendRequest> context,
        Mock<IGetTimeZone> getTimeZoneMock, Mock<IUnitOfWork> unitOfWorkMock) GetDependency()
    {
        var users = new List<User>
        {
            new User
            {
                Id = Guid.Parse("25e4178a-df47-4bb5-a98e-74e7dedc7a44"),
                UserName = "User"
            },
            new User
            {
                Id = Guid.Parse("e93cca14-3bd2-4c82-92e5-3af968ab0a4d"),
                UserName = "Other User"
            },
            new User
            {
                Id = Guid.Parse("fdeb1396-d98b-4645-b95d-0250391cdfec"),
                UserName = "Friend Required"
            },
            new User
            {
                Id = Guid.Parse("6464e979-bc03-4c89-9a27-1543416e1c1d"),
                UserName = "Alredy Friends"
            }
        };

        var friendships = new List<Friendship>
        {
            new Friendship
            {
                UserId = Guid.Parse("25e4178a-df47-4bb5-a98e-74e7dedc7a44"),
                FriendId = Guid.Parse("6464e979-bc03-4c89-9a27-1543416e1c1d")
            }
        };

        var friendRequests = new List<FriendRequest>
       {
           new FriendRequest
           {
               UserId = Guid.Parse("25e4178a-df47-4bb5-a98e-74e7dedc7a44"),
               FriendId = Guid.Parse("fdeb1396-d98b-4645-b95d-0250391cdfec")
           }
       };

        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(ur => ur.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns<Guid, CancellationToken>((id, ct) => Task.FromResult(users.FirstOrDefault(c => c.Id == id)));

        var friendRequestRepository = new Mock<IFriendRequestRepository>();
        friendRequestRepository.Setup(frr => frr.Save(It.IsAny<FriendRequest>(), It.IsAny<CancellationToken>()))
        .Callback<FriendRequest, CancellationToken>((frr, ct) => friendRequests.Add(frr))
        .Returns<FriendRequest, CancellationToken>((frr, ct) => Task.CompletedTask);

        friendRequestRepository.Setup(frr => frr.IsAlredyRequested(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns<Guid, Guid, CancellationToken>((id, fId, ct) =>
            Task.FromResult(friendRequests.Any(c => c.UserId == id && c.FriendId == fId
            || c.UserId == fId && c.FriendId == id)));

        var friendshipRepository = new Mock<IFriendshipRepository>();
        friendshipRepository.Setup(fs => fs.IsAlreadyFriends(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns((Guid id, Guid fId, CancellationToken cancellationToken) =>
            Task.FromResult(friendships.Any(c => c.UserId == id && c.FriendId == fId
            || c.UserId == fId && c.FriendId == id)));

        var getTimeZone = new Mock<IGetTimeZone>();
        getTimeZone.Setup(gtz => gtz.GetApplicationTimeZone())
            .Returns(() => DateTime.UtcNow);

        return (userRepository, friendRequestRepository, friendshipRepository, friendRequests, getTimeZone, new Mock<IUnitOfWork>());
    }

    [Fact]
    public async Task Friend_Request_Was_Made()
    {
        (Mock<IUserRepository> userRepositoryMock, Mock<IFriendRequestRepository> friendRequestRepositoryMock,
        Mock<IFriendshipRepository> friendshipRepositoryMock, IList<FriendRequest> context,
        Mock<IGetTimeZone> getTimeZoneMock, Mock<IUnitOfWork> unitOfWorkMock) = GetDependency();

        var request = new FriendRequestCommand(Guid.Parse("25e4178a-df47-4bb5-a98e-74e7dedc7a44"), Guid.Parse("e93cca14-3bd2-4c82-92e5-3af968ab0a4d"));
        var handler = new FriendRequestCommandHandler(userRepositoryMock.Object, friendRequestRepositoryMock.Object, friendshipRepositoryMock.Object, getTimeZoneMock.Object, unitOfWorkMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Success);
        Assert.Empty(result.Errors);
        Assert.Equal(context.Count, 2);
    }

    [Theory]
    [InlineData("25e4178a-df47-4bb5-a98e-74e7dedc7a44", "fdeb1396-d98b-4645-b95d-0250391cdfec")]
    [InlineData("fdeb1396-d98b-4645-b95d-0250391cdfec", "25e4178a-df47-4bb5-a98e-74e7dedc7a44")]
    public async Task FriendRequest_Is_Already_Exist(Guid id, Guid friendId)
    {
        (Mock<IUserRepository> userRepositoryMock, Mock<IFriendRequestRepository> friendRequestRepositoryMock,
        Mock<IFriendshipRepository> friendshipRepositoryMock, IList<FriendRequest> context,
        Mock<IGetTimeZone> getTimeZoneMock, Mock<IUnitOfWork> unitOfWorkMock) = GetDependency();

        var request = new FriendRequestCommand(id, friendId);
        var handler = new FriendRequestCommandHandler(userRepositoryMock.Object, friendRequestRepositoryMock.Object, friendshipRepositoryMock.Object, getTimeZoneMock.Object, unitOfWorkMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Failure);
        Assert.NotEmpty(result.Errors);
        Assert.Equal(context.Count, 1);
    }

    [Theory]
    [InlineData("25e4178a-df47-4bb5-a98e-74e7dedc7a44", "6464e979-bc03-4c89-9a27-1543416e1c1d")]
    [InlineData("6464e979-bc03-4c89-9a27-1543416e1c1d", "25e4178a-df47-4bb5-a98e-74e7dedc7a44")]
    public async Task Friendship_Is_Already_Exist(Guid id, Guid friendId)
    {
        (Mock<IUserRepository> userRepositoryMock, Mock<IFriendRequestRepository> friendRequestRepositoryMock,
        Mock<IFriendshipRepository> friendshipRepositoryMock, IList<FriendRequest> context,
        Mock<IGetTimeZone> getTimeZoneMock, Mock<IUnitOfWork> unitOfWorkMock) = GetDependency();

        var request = new FriendRequestCommand(id, friendId);
        var handler = new FriendRequestCommandHandler(userRepositoryMock.Object, friendRequestRepositoryMock.Object, friendshipRepositoryMock.Object, getTimeZoneMock.Object, unitOfWorkMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Failure);
        Assert.NotEmpty(result.Errors);
        Assert.Equal(context.Count, 1);
    }
}
