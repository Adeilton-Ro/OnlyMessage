using FastEndpoints;
using FluentValidation;

namespace Webapi.Features.Friends.Friendships;
public class FriendshipsEndpointRequestValidator : Validator<FriendshipsEndpointRequest>
{
    public FriendshipsEndpointRequestValidator()
    {
        RuleFor(fs => fs.Id).NotEmpty();
        RuleFor(fs => fs.IsAccepted).NotNull();
    }
}