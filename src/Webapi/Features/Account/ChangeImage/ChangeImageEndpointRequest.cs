using Application.Feature.Account.ChangeImage;
using AutoMapper;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils.Mapping;
using Webapi.Extesions;

namespace Webapi.Features.Account.ChangeImage;
public class ChangeImageEndpointRequest : IMappeableTo<ChangeImageCommand>
{
    [FromClaim(ClaimType = ClaimTypes.NameIdentifier)]
    public Guid Id { get; set; }
    [FromForm]
    public IFormFile Image { get; set; }

    public void ConfigureMap(Profile profile)
    {
        profile.CreateMap<ChangeImageEndpointRequest, ChangeImageCommand>()
            .ForCtorParam("Image", ce => ce.MapFrom(ce => ce.Image.ToBytes()))
            .ForCtorParam("Extension", ce => ce.MapFrom(ce => ce.Image.ContentType.Replace("image/", ".")));
    }
}