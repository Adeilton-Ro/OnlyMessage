using Application.Feature.Account.SignUp;
using Utils.Mapping;

namespace Webapi.Features.Account.SignUp;
public record SignUpEndpointResponse(Guid Id) : IMappeableFrom<SignUpCommandResponse>;