using Application.Feature.Friends.Friendships;
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

namespace Application.UnitTesting.Feature.Friends.Friendships;
public class FriendshipCommandHandlerTest
{
    public static (Mock<IFriendRequestRepository> friendRequestRepositoryMock,
    Mock<IFriendshipRepository> friendshipRepositoryMock, IList<Friendship> context,
        Mock<IUnitOfWork> unitOfWorkMock, List<FriendRequest> friendRequests) GetDependency()
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
                   Id = Guid.Parse("EF2FD2F5-6934-4F69-9979-D2EFDE52926E"),
                   UserId = Guid.Parse("25e4178a-df47-4bb5-a98e-74e7dedc7a44"),
                   FriendId = Guid.Parse("fdeb1396-d98b-4645-b95d-0250391cdfec")
               }
           };

        var friendRequestRepository = new Mock<IFriendRequestRepository>();
        friendRequestRepository.Setup(fr => fr.Delete(It.IsAny<FriendRequest>()))
            .Callback((FriendRequest fr) => friendRequests.Remove(fr));
        friendRequestRepository.Setup(fr => fr.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns((Guid id, CancellationToken ct) => Task.FromResult(friendRequests.FirstOrDefault(fr => fr.Id == id)));

        var friendshipRepository = new Mock<IFriendshipRepository>();
        friendshipRepository.Setup(fs => fs.Save(It.IsAny<Friendship>(), It.IsAny<CancellationToken>()))
            .Callback((Friendship fs, CancellationToken ct) => friendships.Add(fs));

        return (friendRequestRepository, friendshipRepository, friendships, new Mock<IUnitOfWork>(), friendRequests);
    }

    [Fact]
    public async Task Successfully_Accepted_Friend_Request()
    {
        (Mock<IFriendRequestRepository> friendRequestRepositoryMock,
        Mock<IFriendshipRepository> friendshipRepositoryMock, IList<Friendship> context
        , Mock<IUnitOfWork> unitOfWorkMock, List<FriendRequest> friendRequests) = GetDependency();

        var request = new FriendShipCommand(Guid.Parse("EF2FD2F5-6934-4F69-9979-D2EFDE52926E"), true, Guid.Parse("fdeb1396-d98b-4645-b95d-0250391cdfec"));
        var handler = new FriendShipCommandHandler(unitOfWorkMock.Object, friendRequestRepositoryMock.Object, friendshipRepositoryMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Success);
        Assert.Empty(result.Errors);
        Assert.Empty(friendRequests);
        Assert.Equal(context.Count, 3);
    }

    [Fact]
    public async Task Successfully_Declined_Friend_Request()
    {
        (Mock<IFriendRequestRepository> friendRequestRepositoryMock,
        Mock<IFriendshipRepository> friendshipRepositoryMock, IList<Friendship> context
        , Mock<IUnitOfWork> unitOfWorkMock, List<FriendRequest> friendRequests) = GetDependency();

        var request = new FriendShipCommand(Guid.Parse("EF2FD2F5-6934-4F69-9979-D2EFDE52926E"), false, Guid.Parse("fdeb1396-d98b-4645-b95d-0250391cdfec"));
        var handler = new FriendShipCommandHandler(unitOfWorkMock.Object, friendRequestRepositoryMock.Object, friendshipRepositoryMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Success);
        Assert.Empty(result.Errors);
        Assert.Empty(friendRequests);
        Assert.Single(context);
    }

    [Fact]
    public async Task Friend_Request_Doesnt_Exist()
    {
        (Mock<IFriendRequestRepository> friendRequestRepositoryMock,
        Mock<IFriendshipRepository> friendshipRepositoryMock, IList<Friendship> context
        , Mock<IUnitOfWork> unitOfWorkMock, List<FriendRequest> friendRequests) = GetDependency();

        var request = new FriendShipCommand(Guid.Parse("7605b923-5c9c-44d8-96ec-9df797784d24"), true, Guid.Parse("fdeb1396-d98b-4645-b95d-0250391cdfec"));
        var handler = new FriendShipCommandHandler(unitOfWorkMock.Object, friendRequestRepositoryMock.Object, friendshipRepositoryMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.NotFound);
        Assert.NotEmpty(result.Errors);
        Assert.Single(friendRequests);
        Assert.Single(context);
    }

    [Fact]
    public async Task Other_People_Try_Accept_The_Friend_Request()
    {
        (Mock<IFriendRequestRepository> friendRequestRepositoryMock,
        Mock<IFriendshipRepository> friendshipRepositoryMock, IList<Friendship> context
        , Mock<IUnitOfWork> unitOfWorkMock, List<FriendRequest> friendRequests) = GetDependency();

        var request = new FriendShipCommand(Guid.Parse("EF2FD2F5-6934-4F69-9979-D2EFDE52926E"), true, Guid.Parse("25e4178a-df47-4bb5-a98e-74e7dedc7a44"));
        var handler = new FriendShipCommandHandler(unitOfWorkMock.Object, friendRequestRepositoryMock.Object, friendshipRepositoryMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(result.Code, ResultCodes.Failure);
        Assert.NotEmpty(result.Errors);
        Assert.Single(friendRequests);
        Assert.Single(context);
    }
}