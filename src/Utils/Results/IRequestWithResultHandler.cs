using MediatR;

namespace Utils.Results;

public interface IRequestWithResultHandler<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : IRequestWithResult
{ }

public interface IRequestWithResultHandler<TRequest, TResultContent> : IRequestHandler<TRequest, Result<TResultContent>>
    where TRequest : IRequestWithResult<TResultContent>
{ }
