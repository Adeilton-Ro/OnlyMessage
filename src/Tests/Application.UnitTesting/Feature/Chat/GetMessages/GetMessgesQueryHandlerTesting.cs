using Application.Feature.Chat.GetMessages;
using Domain.Entities;
using Infrastructure.DataBase.Abstract.DTO;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils.Results;
using Xunit;

namespace Application.UnitTesting.Feature.Chat.GetMessages;
public class GetMessgesQueryHandlerTesting
{
	private readonly Mock<IMessageRepository> MessageRepositoryMock = new();
	private readonly Mock<IFriendshipRepository> FriendshipRepositoryMock = new();
	private readonly List<User> context = new()
	{
		new User
		{
			Id = Guid.Parse("4d33f876-2183-4032-b257-940846bbbe5d"),
			Messages = new List<Message<User>>()
		},
		new User
		{
			Id = Guid.Parse("305f9c55-784f-456b-b9c4-d114bbda3bcc"),
			Messages = new List<Message<User>>()
        },
		new User
		{
			Id= Guid.Parse("872eed50-0914-45ca-bb79-9f42f5ad4086")
		}
	};

	private List<Friendship> friendshipList = new()
	{
		new Friendship
		{
			UserId = Guid.Parse("4d33f876-2183-4032-b257-940846bbbe5d"),
			FriendId = Guid.Parse("305f9c55-784f-456b-b9c4-d114bbda3bcc")
        }
	};
	public GetMessgesQueryHandlerTesting()
	{
		MessageRepositoryMock.Setup(m => m.GetChatMessages(It.IsAny<Guid>(),It.IsAny<Guid>(),It.IsAny<CancellationToken>()))
			.ReturnsAsync((Guid id, Guid friendId, CancellationToken ct)=> new UserAndFriendMessages(
				context.FirstOrDefault(u=>u.Id == id),
				context.FirstOrDefault(f=>f.Id == friendId)));

		FriendshipRepositoryMock.Setup(fsr => fsr.IsAlreadyFriends(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(
			(Guid id, Guid friendId, CancellationToken ct) 
				=> friendshipList.Any(fs => fs.UserId == id && fs.FriendId == friendId || fs.UserId == friendId && fs.FriendId == id));
	}

	[Fact]
	public async Task Sucesso()
	{
		var request = new GetMessagesQuery(Guid.Parse("4d33f876-2183-4032-b257-940846bbbe5d"), Guid.Parse("305f9c55-784f-456b-b9c4-d114bbda3bcc"));

		var handle = new GetMessagesQueryHandler(MessageRepositoryMock.Object, FriendshipRepositoryMock.Object);

		var result = await handle.Handle(request, CancellationToken.None);

		Assert.True(result.IsSuccess);
		Assert.Empty(result.Errors);
		Assert.Equal(ResultCodes.Success, result.Code);
		Assert.NotNull(result.Content);
	}

	[Fact]
	public async Task IsNotFriends()
	{
        var request = new GetMessagesQuery(Guid.Parse("4d33f876-2183-4032-b257-940846bbbe5d"),Guid.Parse("872eed50-0914-45ca-bb79-9f42f5ad4086"));

        var handle = new GetMessagesQueryHandler(MessageRepositoryMock.Object, FriendshipRepositoryMock.Object);

        var result = await handle.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Equal(ResultCodes.NotFound, result.Code);
        Assert.Null(result.Content);
    }
}