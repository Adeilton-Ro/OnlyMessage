using AutoMapper;
using System.Reflection;
using Utils.Reflection;

namespace Utils.Mapping;

public class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        CreateMappingsFromAssembly(AppDomain.CurrentDomain.GetAssemblies());
    }

    public void CreateMappingsFromAssembly(Assembly[] assemblies)
    {
        var allTypes = assemblies.SelectMany(a => a.GetTypes());

        var mappeableToType = typeof(IMappeableTo<>);
        var mappeableFromType = typeof(IMappeableFrom<>);

        var mappeableTypes = allTypes
            .Where(type => type.ImplementsAnyDirectly(mappeableToType, mappeableFromType));

        foreach (var mappeableType in mappeableTypes)
        {
            var mappeableInterface = mappeableType.GetInterfaces()
                .FirstOrDefault(@interface => @interface.GetGenericTypeDefinition() == mappeableToType
                || @interface.GetGenericTypeDefinition() == mappeableFromType);

            var configureMapMethod = mappeableInterface?.GetMethod("ConfigureMap");

            var instance = ReflectionExtensions.CreateInstanceWithDefaultParams(mappeableType);

            configureMapMethod?.Invoke(instance, new object[] { this });
        }
    }
}
