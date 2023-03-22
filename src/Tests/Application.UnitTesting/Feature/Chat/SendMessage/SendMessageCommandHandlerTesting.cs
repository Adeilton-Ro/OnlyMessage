using Application.Feature.Chat.SendMessage;
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

namespace Application.UnitTesting.Feature.Chat.SendMessage;
public class SendMessageCommandHandlerTesting
{
	private readonly Mock<IMessageRepository> messageRepositoryMock = new();
	private readonly Mock<IUnitOfWork> unitOfWorkMock = new();
	private readonly Mock<IFriendshipRepository> friendshipRepositoryMock = new();

	private readonly List<Message<User>> context = new();
	private readonly List<Friendship> friendships = new()
	{
		new Friendship
		{
			UserId = Guid.Parse("63ffb4e1-e7de-4c49-88c3-1b818e34d37b"),
			FriendId = Guid.Parse("1654f25d-9b15-4877-a951-2d8c0d52d453")
		},
        new Friendship
        {
            UserId = Guid.Parse("1654f25d-9b15-4877-a951-2d8c0d52d453"),
            FriendId = Guid.Parse("63ffb4e1-e7de-4c49-88c3-1b818e34d37b")
        }
    };
	public SendMessageCommandHandlerTesting()
	{
		messageRepositoryMock.Setup(mr => mr.AddMessage(It.IsAny<Message<User>>(), It.IsAny<CancellationToken>()))
			.Callback((Message<User> message, CancellationToken cancellationToken) => context.Add(message))
			.Returns((Message<User> message, CancellationToken cancellationToken) => Task.CompletedTask);

		friendshipRepositoryMock.Setup(fr => fr.IsAlreadyFriends(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Guid id, Guid friendId, CancellationToken ct) => friendships.Any(fs => fs.UserId == id && fs.FriendId == friendId
        || fs.FriendId == id && fs.UserId == friendId));
	}

	[Theory]
	[InlineData("1654f25d-9b15-4877-a951-2d8c0d52d453", "63ffb4e1-e7de-4c49-88c3-1b818e34d37b")]
	[InlineData("63ffb4e1-e7de-4c49-88c3-1b818e34d37b", "1654f25d-9b15-4877-a951-2d8c0d52d453")]
	public async Task Save_With_Success(Guid senderId, Guid receiverId)
	{
		var request = new SendMessageCommand(senderId, receiverId, "Hello!");

		var handler = new SendMessageCommandHandler(messageRepositoryMock.Object, friendshipRepositoryMock.Object, unitOfWorkMock.Object);

		var result = await handler.Handle(request, CancellationToken.None);

        unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None));

        Assert.True(result.IsSuccess);
		Assert.Empty(result.Errors);
		Assert.NotNull(result.Content);
		Assert.Equal(ResultCodes.Success, result.Code);
		Assert.NotEmpty(context);
	}

	[Fact]
    public async Task Isnt_Friends()
    {
        var request = new SendMessageCommand(Guid.Parse("1654f25d-9b15-4877-a951-2d8c0d52d453"), Guid.Parse("3b32061e-c07c-4484-87be-ddfb013c94e1"), "Hello!");

        var handler = new SendMessageCommandHandler(messageRepositoryMock.Object, friendshipRepositoryMock.Object, unitOfWorkMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Content);
        Assert.Equal(ResultCodes.Failure, result.Code);
        Assert.Empty(context);
    }

    [Fact]
    public async Task Self_Send_Message()
    {
        var request = new SendMessageCommand(Guid.Parse("3b32061e-c07c-4484-87be-ddfb013c94e1"), Guid.Parse("3b32061e-c07c-4484-87be-ddfb013c94e1"), "Hello!");

        var handler = new SendMessageCommandHandler(messageRepositoryMock.Object, friendshipRepositoryMock.Object, unitOfWorkMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Content);
        Assert.Equal(ResultCodes.Failure, result.Code);
        Assert.Empty(context);
    }
}