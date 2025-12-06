using CleanArchitecture.Api.Infraestructure;
using System.Reflection;

namespace CleanArchitecture.Api;

public static class DependencyInjection
{
    private static readonly Assembly assembly = typeof(DependencyInjection).Assembly;

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddHttpContextAccessor();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
