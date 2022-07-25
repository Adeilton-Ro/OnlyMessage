using AutoMapper;
using FastEndpoints;
using MediatR;
using Utils.Mapping;
using Utils.Results;

namespace Webapi.RequestHandling;

/// <summary>
/// Pre-Configured endpoint. <br/>
/// This endpoint validates the request, then it sends it to the Mediatr Pipeline
/// When the response is ready, it is mapped and sent.
/// </summary>
/// <typeparam name="TEndpointRequest">Type of the endpoint request dto. 
/// <br/> Must inherit from IMappeableTo[TApplicationRequest] </typeparam>
/// <typeparam name="TApplicationRequest">Type of the application request, mapped from the endpoint request</typeparam>
/// <typeparam name="TApplicationResultContent">Type inside the application result</typeparam>
/// <typeparam name="TEndpointResponse">Type of the endpoint response, mapped from the content of the application result
/// <br/> Must inherit from IMappeableFrom[TApplicationRequest] </typeparam>
public abstract class EndpointWithResult<TEndpointRequest, TApplicationRequest, TApplicationResultContent, TEndpointResponse>
    : Endpoint<TEndpointRequest>
    where TEndpointRequest : IMappeableTo<TApplicationRequest>, new()
    where TApplicationRequest : IRequestWithResult<TApplicationResultContent>
    where TEndpointResponse : IMappeableFrom<TApplicationResultContent>
{
    private readonly IMapper mapper;
    private readonly ISender sender;

    public EndpointWithResult(IMapper mapper, ISender sender)
    {
        this.mapper = mapper;
        this.sender = sender;
    }

    public override async Task HandleAsync(TEndpointRequest req, CancellationToken ct)
    {
        var commandResponse = await AcessApplication(req, ct);

        await SendResponse(commandResponse, ct);
    }

    public virtual async Task<IResult<TApplicationResultContent>> AcessApplication(TEndpointRequest req, CancellationToken cancellationToken)
    {
        var mappedRequest = mapper.Map<TEndpointRequest, TApplicationRequest>(req);

        return await sender.Send(mappedRequest, cancellationToken);
    }

    public virtual Task SendResponse(IResult<TApplicationResultContent> applicationResponse, CancellationToken cancellationToken)
    {
        if (applicationResponse.IsSuccess && applicationResponse.Content is not null)
        {
            var endpointResponse = mapper.Map<TApplicationResultContent, TEndpointResponse>(applicationResponse.Content);
            return SendAsync(endpointResponse, 200, cancellationToken);
        }

        if (applicationResponse.Code == ResultCodes.InsufficientPermission)
            return SendAsync(new { Message = applicationResponse.Errors.FirstOrDefault()?.Detail }, 401, cancellationToken);

        if (applicationResponse.Code == ResultCodes.NotFound)
            return SendAsync(new { Message = applicationResponse.Errors.FirstOrDefault()?.Detail }, 404, cancellationToken);

        if (applicationResponse.Code == ResultCodes.InternalFailure)
            return SendAsync(new { Message = applicationResponse.Errors.FirstOrDefault()?.Detail }, 500, cancellationToken);

        if (applicationResponse.Code == ResultCodes.Failure)
        {
            if (applicationResponse.Errors.Count == 1)
                return SendAsync(new { Message = applicationResponse.Errors.FirstOrDefault()?.Detail }, 403, cancellationToken);

            if (applicationResponse.Errors.Count > 1)
                return SendAsync(applicationResponse.Errors.Select(e => new { Message = e.Detail }), 403, cancellationToken);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Method used to configure the endpoint's route. <br/>
    /// Use Verbs(), Routes() and AllowAnonymus() to configure.
    /// </summary>
    public abstract void ConfigureRoute();
    /// <summary>
    /// Method used to configure the endpoint's swagger generation. <br/>
    /// Use Description to configure.
    /// </summary>
    public abstract void ConfigureSwagger();

    public override void Configure()
    {
        ConfigureRoute();
        ConfigureSwagger();
    }
}
