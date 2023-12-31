using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Enterprise.ApplicationServices.Queries.Model.Generic;
using Enterprise.DomainDrivenDesign.Event.Abstract;
using Enterprise.Reflection.Assemblies;
using Enterprise.Domain.Generic;

namespace Enterprise.MediatR.Extensions;

public static class DependencyRegistrationExtensions
{
    public static void RegisterQueriesAsRequests(this IServiceCollection services)
    {
        List<Assembly> assemblies = GetAssemblies();

        Type queryInterfaceType = typeof(IQuery<>);
        string? queryInterfaceFullTypeName = queryInterfaceType.FullName;

        if (queryInterfaceFullTypeName == null)
            return;

        // Find all classes implementing IQuery<TResponse>
        IEnumerable<Type> queryTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false, IsGenericType: true } &&
                           type.GetInterfaces().Any(i => i.IsGenericType &&
                                                         i.GetGenericTypeDefinition() == typeof(IQuery<>)));

        // Register each query type with MediatR
        foreach (Type queryType in queryTypes)
        {
            Type? queryTypeInterface = queryType.GetInterface(queryInterfaceFullTypeName);
            Type[]? genericArguments = queryTypeInterface?.GetGenericArguments();

            if (genericArguments is not { Length: 1 })
                continue;

            // Set Result<TResponse> to be the generic type argument for the MediatR IRequest interface
            Type resultType = typeof(Result<>).MakeGenericType(genericArguments[0]);
            Type requestInterface = typeof(IRequest<>).MakeGenericType(resultType);

            services.AddScoped(requestInterface, queryType);
        }
    }

    public static void RegisterQueryHandlersAsRequestHandlers(this IServiceCollection services)
    {
        List<Assembly> assemblies = GetAssemblies();

        // Find all classes implementing IHandleQuery<TQuery, TResult>
        IEnumerable<Type> handlerTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass &&
                           !type.IsAbstract &&
                           type.GetInterfaces().Any(i => i.IsGenericType &&
                                                         i.GetGenericTypeDefinition() == typeof(IHandleQuery<,>)));

        // Register each handler type with MediatR
        foreach (Type handlerType in handlerTypes)
        {
            Type[] interfaceTypes = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleQuery<,>))
                .ToArray();

            foreach (Type interfaceType in interfaceTypes)
            {
                Type[] genericArguments = interfaceType.GetGenericArguments();

                // Assuming genericArguments[0] is TQuery and genericArguments[1] is TResult
                if (genericArguments.Length != 2)
                    continue;

                Type queryType = genericArguments[0];
                Type resultType = genericArguments[1];

                Type genericResultType = typeof(Result<>).MakeGenericType(resultType);

                Type requestHandlerType = typeof(IRequestHandler<,>)
                    .MakeGenericType(queryType, genericResultType);

                services.AddScoped(requestHandlerType, handlerType);
            }
        }
    }

    public static void RegisterDomainEventsAsNotifications(this IServiceCollection services)
    {
        List<Assembly> assemblies = GetAssemblies();

        IEnumerable<Type> domainEventTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IDomainEvent).IsAssignableFrom(t));

        // Register all domain event types as MediatR notifications.
        foreach (Type type in domainEventTypes)
            services.AddScoped(typeof(INotification), type);
    }

    private static List<Assembly> GetAssemblies()
    {
        Assembly[] allAssemblies = AssemblyService.GetSolutionAssemblies();

        // Dynamically load custom or non-Microsoft/package assemblies
        List<Assembly> assemblies = allAssemblies
            .Where(IsNotRestrictedAssembly)
            .ToList();

        return assemblies;
    }

    private static bool IsNotRestrictedAssembly(Assembly assembly)
    {
        if (assembly.FullName == null)
            return false;

        if (assembly.FullName.StartsWith("Microsoft") || assembly.FullName.StartsWith("System"))
            return false;

        return true;
    }
}