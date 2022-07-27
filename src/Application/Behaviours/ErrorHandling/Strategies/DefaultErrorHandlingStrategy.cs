using Microsoft.Extensions.Logging;
using Utils.Results;

namespace Application.Behaviours.ErrorHandling.Strategies;

public class DefaultErrorHandlingStrategy : IErrorHandlingStrategy
{
    bool IErrorHandlingStrategy.CanHandle<TRequest>(Exception e, TRequest request)
    {
        return true;
    }

    TResponse IErrorHandlingStrategy.Handle<TRequest, TResponse>(ILogger logger, Exception e, TRequest request)
    {
        logger.LogError("Unkown error");
        return Result.OfInternalFailure("Unkown error").CastTo<TResponse>();
    }
}