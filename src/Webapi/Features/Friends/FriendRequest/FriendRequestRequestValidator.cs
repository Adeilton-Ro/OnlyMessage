using FastEndpoints;
using FluentValidation;

namespace Webapi.Features.Friends.FriendRequest;
public class FriendRequestRequestValidator : Validator<FriendRequestEndpointRequest>
{
    public FriendRequestRequestValidator()
    {
        RuleFor(fr => fr).Must(fr => fr.Id != fr.FriendId).WithMessage("Você não pode solicitar amizade para você mesmo");
    }
}