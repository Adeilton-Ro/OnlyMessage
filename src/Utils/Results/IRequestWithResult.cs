using MediatR;

namespace Utils.Results;

public interface IRequestWithResult : IRequest<Result> { }
public interface IRequestWithResult<TResultContent> : IRequest<Result<TResultContent>> { }
