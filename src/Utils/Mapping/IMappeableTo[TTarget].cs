using AutoMapper;

namespace Utils.Mapping;

public interface IMappeableTo<TTarget>
{
    public void ConfigureMap(Profile profile) => profile.CreateMap(GetType(), typeof(TTarget));
}