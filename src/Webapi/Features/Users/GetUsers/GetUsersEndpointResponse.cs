using Application.Feature.Users.GetUsers;
using Utils.Mapping;

namespace Webapi.Features.Users.GetUsers;
public record GetUsersEndpointResponse(Guid Id, string UserName, string ImageUrl) : IMappeableFrom<GetUsersQueryResponse>;