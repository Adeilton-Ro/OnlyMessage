using Application.Feature.Chat.SendMessage;
using Utils.Mapping;

namespace Webapi.Features.Chat.SendMessage;
public record SendMessageEndpointResponse(Guid Id) : IMappeableFrom<SendMessageCommandResponse>;