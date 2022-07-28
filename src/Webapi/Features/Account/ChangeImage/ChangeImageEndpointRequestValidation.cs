using FastEndpoints;
using FluentValidation;

namespace Webapi.Features.Account.ChangeImage;
public class ChangeImageEndpointRequestValidation : Validator<ChangeImageEndpointRequest>
{
    public ChangeImageEndpointRequestValidation()
    {
        When(x => x.Image is not null, () =>
        {
            RuleFor(x => x.Image)
            .Must(x => IsAllowedSize(x.Length))
            .Must(x => IsAllowedType(x.ContentType));
        });
    }
    public bool IsAllowedType(string contentType)
        => (new[] { "image/jpeg", "image/png" }).Contains(contentType.ToLower());

    public bool IsAllowedSize(long fileLength)
        => fileLength >= 100 && fileLength <= 10485760;
}