using System.Reflection;

namespace CleanArchitecture.Api.Extensions;

public static class EndpointRegistrationExtensions
{
    public static void AutoRegisterEndpoints(this WebApplication app)
    {
        // Scan all assemblies loaded for types in the namespace "MinimalApiDemo.Endpoints"
        var endpointTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && t.IsAbstract && t.IsSealed) // static class
            .Where(t => t.Namespace != null && t.Namespace.Contains("Endpoints"));

        foreach (var type in endpointTypes)
        {
            // Busca método 'MapEndpoints'
            var method = type.GetMethod("MapEndpoints", BindingFlags.Public | BindingFlags.Static);

            if (method != null)
            {
                // Invoca 'MapEndpoints' pasando app como parámetro
                method.Invoke(null, [app]);
            }
        }
    }
}