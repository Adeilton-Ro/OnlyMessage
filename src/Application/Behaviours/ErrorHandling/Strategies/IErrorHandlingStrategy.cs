using MediatR;
using Microsoft.Extensions.Logging;
using Utils.Results;

namespace Application.Behaviours.ErrorHandling.Strategies;

public interface IErrorHandlingStrategy
{
    bool CanHandle<TRequest>(Exception e, TRequest request);
    TResponse Handle<TRequest, TResponse>(ILogger logger, Exception e, TRequest request) where TResponse : IResult, new()
    where TRequest : IRequest<TResponse>;
}
