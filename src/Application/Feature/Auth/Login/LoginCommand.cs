using Utils.Results;

namespace Application.Feature.Auth.Login;

public record LoginCommand(string UserName, string Password) : IRequestWithResult<LoginCommandResponse>;