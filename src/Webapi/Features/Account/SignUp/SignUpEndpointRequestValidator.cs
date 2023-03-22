using FastEndpoints;
using FluentValidation;

namespace Webapi.Features.Account.SignUp;
public class SignUpEndpointRequestValidator : Validator<SignUpEndpointRequest>
{
    public SignUpEndpointRequestValidator()
    {
        RuleFor(s => s.UserName)
            .MaximumLength(50)
            .MinimumLength(3)
            .NotEmpty();
        RuleFor(s => s.Password)
            .MaximumLength(32)
            .MinimumLength(8);
    }
}