using Enterprise.DomainDrivenDesign.Events.Abstract;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Enterprise.Reflection.Assemblies;

namespace Enterprise.MediatR.Extensions;

public static class DependencyRegistrationExtensions
{
    public static void RegisterDomainEventsAsMediatRNotifications(this IServiceCollection services)
    {
        // TODO: Add assembly filtering (Microsoft, system, Third Party libraries, etc.)

        Assembly[] allAssemblies = AssemblyService.GetSolutionAssemblies();

        // Dynamically load custom or non-Microsoft/package assemblies
        List<Assembly> assemblies = allAssemblies
            .Where(IsCustomAssembly)
            .ToList();

        IEnumerable<Type> domainEventTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IDomainEvent).IsAssignableFrom(t));

        // Register all domain event types as MediatR notifications.
        foreach (Type type in domainEventTypes)
            services.AddScoped(typeof(INotification), type);
    }

    private static bool IsCustomAssembly(Assembly assembly)
    {
        if (assembly.FullName == null)
            return false;

        if (assembly.FullName.StartsWith("Microsoft") || assembly.FullName.StartsWith("System"))
            return false;

        return true;
    }
}