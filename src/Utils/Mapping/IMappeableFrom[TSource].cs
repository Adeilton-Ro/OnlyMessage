using AutoMapper;

namespace Utils.Mapping;

public interface IMappeableFrom<TSource>
{
    public void ConfigureMap(Profile profile) => profile.CreateMap(typeof(TSource), GetType());
}
