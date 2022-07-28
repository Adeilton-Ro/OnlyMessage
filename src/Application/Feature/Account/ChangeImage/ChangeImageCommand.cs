using Utils.Results;

namespace Application.Feature.Account.ChangeImage;
public record ChangeImageCommand(Guid Id, byte[] Image, string Extension) : IRequestWithResult<ChangeImageCommandResponse>;