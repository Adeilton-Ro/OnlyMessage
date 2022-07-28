using Utils.Results;

namespace Application.Feature.Account.UpdateInformations;
public record UpdateInformationsCommand(Guid Id, string Username, string Password) : IRequestWithResult;