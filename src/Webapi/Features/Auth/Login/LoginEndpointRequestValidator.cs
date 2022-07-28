using FastEndpoints;
using FluentValidation;

namespace Webapi.Features.Auth.Login;
public class LoginEndpointRequestValidator : Validator<LoginEndpointRequest>
{
    public LoginEndpointRequestValidator()
    {
        RuleFor(r => r.UserName)
            .MaximumLength(50)
            .MinimumLength(3);
        RuleFor(r => r.Password)
            .MaximumLength(32)
            .MinimumLength(8);
    }
}