using CleanArchitecture.Application.Abstractions.Cqrs;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
    private static readonly Assembly assembly = typeof(DependencyInjection).Assembly;

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {


        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
                    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());


        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        return services;
    }
}
