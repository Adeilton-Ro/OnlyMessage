using Domain.Entities;
using infrastructure.DataBase.Abstract.Interfaces;
using Infrastructure.DataBase.Abstract.Interfaces.Repositories;
using Utils.Results;

namespace Application.Feature.Chat.SendMessage;
public class SendMessageCommandHandler : IRequestWithResultHandler<SendMessageCommand, SendMessageCommandResponse>
{
    private readonly IMessageRepository messageRepository;
    private readonly IFriendshipRepository friendshipRepository;
    private readonly IUnitOfWork unitOfWork;

    public SendMessageCommandHandler(IMessageRepository messageRepository, IFriendshipRepository friendshipRepository, IUnitOfWork unitOfWork)
    {
        this.messageRepository = messageRepository;
        this.friendshipRepository = friendshipRepository;
        this.unitOfWork = unitOfWork;
    }
    public async Task<Result<SendMessageCommandResponse>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == request.ReceiverId)
            return Result.OfFailure("Você não pode enviar mensagem para você mesmo!").Build<SendMessageCommandResponse>();
        if (!await friendshipRepository.IsAlreadyFriends(request.Id, request.ReceiverId, cancellationToken))
            return Result.OfFailure("Você não pode enviar mensagem para essa pessoa!").Build<SendMessageCommandResponse>();

        var message = new Message<User>
        {
            Id = Guid.NewGuid(),
            TextMessage = request.TextMessage,
            SenderId = request.Id,
            ReceiverId = request.ReceiverId,
            Created = DateTime.UtcNow
        };

        await messageRepository.AddMessage(message, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.OfSuccess(new SendMessageCommandResponse(request.Id, new MessageCommandResponse(message.Id, message.TextMessage, message.Created))).Build();
    }
}