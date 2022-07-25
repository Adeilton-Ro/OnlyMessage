using System.Reflection;

namespace Utils.Reflection;

public static class ReflectionExtensions
{
    public static bool ImplementsDirectly(this Type type, Type intefaceType)
    {
        var implementedInterfaces = type.GetInterfaces();

        if (!implementedInterfaces.Any())
            return false;

        if (implementedInterfaces.Contains(intefaceType))
            return true;

        if (implementedInterfaces.Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == intefaceType))
            return true;

        return false;
    }

    public static bool ImplementsAnyDirectly(this Type type, params Type[] inheriteds)
    {
        return inheriteds.Any(inherited => type.ImplementsDirectly(inherited));
    }


    public static object? CreateInstanceWithDefaultParams(Type instanciableType)
    {
        var constructors = instanciableType.GetConstructors();

        var constructorWithFewestParameters = constructors.OrderBy(ctor => ctor.GetParameters().Length).FirstOrDefault();
        var fewestParameters = constructorWithFewestParameters?.GetParameters();

        if (constructorWithFewestParameters is null || fewestParameters is null || !fewestParameters.Any())
            return Activator.CreateInstance(instanciableType);

        var defaultParams = fewestParameters.Select(paramInfo => paramInfo.ParameterType.GetDefaultValue());

        return constructorWithFewestParameters.Invoke(defaultParams.ToArray());
    }

    public static object? GetDefaultValue(this Type type)
    {
        if (type.GetTypeInfo().IsValueType)
            return Activator.CreateInstance(type);

        return null;
    }
}
