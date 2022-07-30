using Application.Feature.Account.UpdateInformations;
using AutoMapper;
using FastEndpoints;
using MediatR;
using Webapi.RequestHandling;

namespace Webapi.Features.Account.UpdateInformations;
public class UpdateInformationsEndpoint : EndpointWithResult<UpdateInformationsEndpointRequest, UpdateInformationsCommand>
{
    public UpdateInformationsEndpoint(IMapper mapper, ISender sender) : base(mapper, sender) { }

    public override void ConfigureRoute()
    {
        Verbs(Http.PUT);
        Routes("account/updateinformations");
    }

    public override void ConfigureSwagger()
    {
        Description(descriptor =>
            descriptor.RequireAuthorization()
            .Accepts<UpdateInformationsEndpointRequest>("application/json")
            .ProducesValidationProblem()
        );

        Summary(s =>
        {
            s.Summary = "Update Informations";
            s.Description = "Update logged user information";
            s[200] = "When the informations was updated";
            s[401] = "When username is alredy in use";
        });
    }
}