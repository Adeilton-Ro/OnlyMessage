using FastEndpoints;
using FluentValidation;

namespace Webapi.Features.Friends.FriendRequest;
public class FriendRequestRequestValidator : Validator<FriendRequestEndpointRequest>
{
    public FriendRequestRequestValidator()
    {
        RuleFor(fr => fr.FriendId).NotEmpty();
    }
}