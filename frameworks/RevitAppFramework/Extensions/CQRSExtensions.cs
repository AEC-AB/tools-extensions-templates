using System.Linq;

namespace RevitAppFramework.Extensions;

public static class CQRSExtensions
{
    public static global::Microsoft.Extensions.DependencyInjection.IServiceCollection AddCqrs(this global::Microsoft.Extensions.DependencyInjection.IServiceCollection services, global::System.Reflection.Assembly assembly, bool useDesignQueryHandlers)
    {
        global::System.Type[] cqrsInterfaceTypes = GetInterfaceTypes(useDesignQueryHandlers);

        foreach (var type in GetTypesFromAssemblySafely(assembly))
        {
            foreach (var cqrsInterfaceType in cqrsInterfaceTypes)
            {
                var interfaceType = type.GetInterface(cqrsInterfaceType.Name);
                if (interfaceType != null)
                {
                    global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient(services, interfaceType, type);
                }
            }
        }

        return services;
    }

    /// <summary>
    /// Safely retrieves types from an assembly, handling ReflectionTypeLoadException
    /// by returning any types that were successfully loaded.
    /// </summary>
    private static global::System.Type[] GetTypesFromAssemblySafely(global::System.Reflection.Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (global::System.Reflection.ReflectionTypeLoadException ex)
        {
            // Return the types that were successfully loaded
            return ex.Types.Where(t => t != null).ToArray();
        }
        catch (global::System.Exception)
        {
            // If we encounter other exceptions, return an empty array
            return global::System.Array.Empty<global::System.Type>();
        }
    }

    private static global::System.Type[] GetInterfaceTypes(bool useDesignQueryHandlers)
    {
        if (useDesignQueryHandlers)
        {
            return [
            typeof(global::RevitAppFramework.Design.IDesignCommandHandler<>),
            typeof(global::RevitAppFramework.Design.IDesignQueryHandler<,>)
            ];
        }

        return [
            typeof(global::RevitAppFramework.CQRS.ICommandHandler<>),
            typeof(global::RevitAppFramework.CQRS.IQueryHandler<,>)
        ];
    }
}
