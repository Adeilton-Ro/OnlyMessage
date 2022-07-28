using FastEndpoints;
using FluentValidation;

namespace Webapi.Features.Account.UpdateInformations;
public class UpdateInformationsEndpointRequestValidation : Validator<UpdateInformationsEndpointRequest>
{
    public UpdateInformationsEndpointRequestValidation()
    {
        RuleFor(u => u.Username)
        .MaximumLength(50)
        .MinimumLength(3);
        When(u => !string.IsNullOrEmpty(u.Password), () => { RuleFor(u => u.Password).MinimumLength(8).MaximumLength(32); });
    }
}