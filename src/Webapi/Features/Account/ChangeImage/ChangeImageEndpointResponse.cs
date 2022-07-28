using Application.Feature.Account.ChangeImage;
using Utils.Mapping;

namespace Webapi.Features.Account.ChangeImage;
public record ChangeImageEndpointResponse(string ImageUrl) : IMappeableFrom<ChangeImageCommandResponse>;