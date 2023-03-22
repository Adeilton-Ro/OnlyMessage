using Application.Behaviours.ErrorHandling.Strategies;
using MediatR;
using Microsoft.Extensions.Logging;
using Utils.Results;

namespace Application.Behaviours.ErrorHandling;

public class ErrorHandlingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : IResult, new()
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IErrorHandlingStrategy> errorHandlingStrategies;
    private readonly ILogger<ErrorHandlingBehaviour<TRequest, TResponse>> logger;
    private readonly IErrorHandlingStrategy defaultErrorHandlingStrategy;

    public ErrorHandlingBehaviour(
        IEnumerable<IErrorHandlingStrategy> errorHandlingStrategies,
        ILogger<ErrorHandlingBehaviour<TRequest, TResponse>> logger,
        DefaultErrorHandlingStrategy defaultErrorHandlingStrategy)
    {
        this.errorHandlingStrategies = errorHandlingStrategies;
        this.logger = logger;
        this.defaultErrorHandlingStrategy = defaultErrorHandlingStrategy;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var response = TryHandle(e, request);

            if (response is null)
                response = defaultErrorHandlingStrategy.Handle<TRequest, TResponse>(logger, e, request);

            return response;
        }
    }

    public TResponse? TryHandle(Exception e, TRequest request)
    {
        var chosenStrategy = errorHandlingStrategies.FirstOrDefault(s => s.CanHandle(e, request));

        if (chosenStrategy is null)
            return default;

        return chosenStrategy.Handle<TRequest, TResponse>(logger, e, request);
    }
}
