using Utils.Results;

namespace Application.Feature.Account.SignUp;
public record SignUpCommand(string UserName, string Password) : IRequestWithResult<SignUpCommandResponse>;