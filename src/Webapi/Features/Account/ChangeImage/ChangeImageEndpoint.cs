using Application.Feature.Account.ChangeImage;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Account.ChangeImage;
public class ChangeImageEndpoint :
    EndpointWithResult<ChangeImageEndpointRequest, ChangeImageCommand, ChangeImageCommandResponse, ChangeImageEndpointResponse>
{
    public ChangeImageEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Verbs(Http.PUT);
        Routes("account/changeimage");
        AllowFileUploads();
    }

    public override void ConfigureSwagger()
    {
        Description(descriptor =>
            descriptor.RequireAuthorization()
            .Accepts<ChangeImageEndpointRequest>("multipart/form-data")
            .Produces<ChangeImageEndpointResponse>(200, "application/json")
            .ProducesValidationProblem()
        );

        Summary(s =>
        {
            s.Summary = "Change Image";
            s.Description = "Change the user's image";
            s[200] = "When the image was changed";
        });
    }
}