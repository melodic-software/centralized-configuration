using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static Microsoft.Extensions.DependencyInjection.ServiceLifetime;

namespace Enterprise.DI.DotNet.Dependencies;

public class RegistrationContext<TService>(IServiceCollection services)
    where TService : class
{
    public RegistrationContext<TService> AddSingleton<TImplementation>() where TImplementation : class, TService => Add<TImplementation>(Singleton);
    public RegistrationContext<TService> AddSingleton(Func<IServiceProvider, TService> implementationFactory) => Add(implementationFactory, Singleton);
    public RegistrationContext<TService> AddScoped<TImplementation>() where TImplementation : class, TService => Add<TImplementation>(Scoped);
    public RegistrationContext<TService> AddScoped(Func<IServiceProvider, TService> implementationFactory) => Add(implementationFactory, Scoped);
    public RegistrationContext<TService> AddTransient<TImplementation>() where TImplementation : class, TService => Add<TImplementation>(Transient);
    public RegistrationContext<TService> AddTransient(Func<IServiceProvider, TService> implementationFactory) => Add(implementationFactory, Transient);

    public RegistrationContext<TService> Add<TImplementation>(ServiceLifetime serviceLifetime)
        where TImplementation : class, TService
    {
        Type serviceType = typeof(TService);
        Type implementationType = typeof(TImplementation);
        ServiceDescriptor serviceDescriptor = new ServiceDescriptor(serviceType, implementationType, serviceLifetime);
        services.Add(serviceDescriptor);
        return this;
    }

    public RegistrationContext<TService> Add(Func<IServiceProvider, TService> implementationFactory, ServiceLifetime serviceLifetime)
    {
        Type serviceType = typeof(TService);
        ServiceDescriptor serviceDescriptor = new ServiceDescriptor(serviceType, implementationFactory, serviceLifetime);
        services.Add(serviceDescriptor);
        return this;
    }

    public RegistrationContext<TService> Add(ServiceDescriptor serviceDescriptor)
    {
        services.Add(serviceDescriptor);
        return this;
    }

    public RegistrationContext<TService> WithDecorators(params Func<IServiceProvider, TService, TService>[] decoratorFactories)
    {
        return ApplyDecorators(decoratorFactories.Select(df =>
            new Func<IServiceProvider, Func<TService, TService>>(provider =>
                service => df(provider, service)
            )
        ).ToArray());
    }

    public RegistrationContext<TService> WithDecorators(params Func<IServiceProvider, Func<TService, TService>>[] decoratorFactories)
    {
        return ApplyDecorators(decoratorFactories);
    }

    private RegistrationContext<TService> ApplyDecorators(Func<IServiceProvider, Func<TService, TService>>[] decoratorFactories)
    {
        var serviceDescriptor = GetServiceDescriptor();
        var decoratedFactory = CreateDecoratedFactory(serviceDescriptor, decoratorFactories);
        ReplaceServiceDescriptor(serviceDescriptor, decoratedFactory);

        return this;
    }

    private ServiceDescriptor GetServiceDescriptor()
    {
        var serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
        if (serviceDescriptor == null)
            throw new InvalidOperationException($"The service of type {typeof(TService).Name} has not been registered.");

        return serviceDescriptor;
    }

    private static Func<IServiceProvider, TService> CreateDecoratedFactory(ServiceDescriptor serviceDescriptor, Func<IServiceProvider, Func<TService, TService>>[] decoratorFactories)
    {
        return provider =>
        {
            var originalFactory = serviceDescriptor.ImplementationFactory ?? (sp => ActivatorUtilities.CreateInstance(sp, serviceDescriptor.ImplementationType));
            var service = (TService)originalFactory(provider);

            foreach (var decoratorFactory in decoratorFactories)
            {
                service = decoratorFactory(provider)(service);
            }

            return service;
        };
    }

    private void ReplaceServiceDescriptor(ServiceDescriptor originalDescriptor, Func<IServiceProvider, TService> decoratedFactory)
    {
        var decoratedServiceDescriptor = new ServiceDescriptor(typeof(TService), decoratedFactory, originalDescriptor.Lifetime);
        services.Replace(decoratedServiceDescriptor);
    }

    public RegistrationContext<TService> WithDecorator<TDecorator>(Func<IServiceProvider, TService, TDecorator> decoratorFactory) where TDecorator : class, TService
    {
        Type serviceType = typeof(TService);

        // capture the original service registration
        ServiceDescriptor? originalServiceDescriptor = services
            .FirstOrDefault(d => d.ServiceType == serviceType);

        if (originalServiceDescriptor == null)
            throw new InvalidOperationException($"The service of type {serviceType.Name} has not been registered and cannot be decorated.");

        ServiceLifetime lifetime = originalServiceDescriptor.Lifetime;

        // replace the original service with the decorator
        object ImplementationFactory(IServiceProvider serviceProvider)
        {
            TService originalService;

            if (originalServiceDescriptor.ImplementationFactory != null)
            {
                // use the factory if available
                originalService = (TService)originalServiceDescriptor.ImplementationFactory(serviceProvider);
            }
            else if (originalServiceDescriptor.ImplementationInstance != null)
            {
                // use the instance if available
                originalService = (TService)originalServiceDescriptor.ImplementationInstance;
            }
            else if (originalServiceDescriptor.ImplementationType != null)
            {
                // create an instance using ActivatorUtilities if only a type is registered
                originalService = (TService)ActivatorUtilities.CreateInstance(serviceProvider, originalServiceDescriptor.ImplementationType);
            }
            else
            {
                throw new InvalidOperationException("The registration method for the original service is not supported.");
            }

            return decoratorFactory(serviceProvider, originalService);
        }

        ServiceDescriptor decoratorDescriptor = ServiceDescriptor.Describe(serviceType, ImplementationFactory, lifetime);

        services.Replace(decoratorDescriptor);

        return this;
    }
}