using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.DI.DotNet.Dependencies
{
    /// <summary>
    /// Provides a context for fluent service registration, allowing for easy addition of services and decorators.
    /// </summary>
    /// <typeparam name="TService">The type of service to be registered.</typeparam>
    public class RegistrationContext<TService>(IServiceCollection services) where TService : class
    {
        /// <summary>
        /// Registers a singleton service of the specified implementation type.
        /// </summary>
        public RegistrationContext<TService> AddSingleton<TImplementation>() where TImplementation : class, TService => Add<TImplementation>(ServiceLifetime.Singleton);

        /// <summary>
        /// Registers a singleton service with a factory for creating the service.
        /// </summary>
        public RegistrationContext<TService> AddSingleton(Func<IServiceProvider, TService> implementationFactory) => Add(implementationFactory, ServiceLifetime.Singleton);

        /// <summary>
        /// Registers a scoped service of the specified implementation type.
        /// </summary>
        public RegistrationContext<TService> AddScoped<TImplementation>() where TImplementation : class, TService => Add<TImplementation>(ServiceLifetime.Scoped);

        /// <summary>
        /// Registers a scoped service with a factory for creating the service.
        /// </summary>
        public RegistrationContext<TService> AddScoped(Func<IServiceProvider, TService> implementationFactory) => Add(implementationFactory, ServiceLifetime.Scoped);

        /// <summary>
        /// Registers a transient service of the specified implementation type.
        /// </summary>
        public RegistrationContext<TService> AddTransient<TImplementation>() where TImplementation : class, TService => Add<TImplementation>(ServiceLifetime.Transient);

        /// <summary>
        /// Registers a transient service with a factory for creating the service.
        /// </summary>
        public RegistrationContext<TService> AddTransient(Func<IServiceProvider, TService> implementationFactory) => Add(implementationFactory, ServiceLifetime.Transient);

        /// <summary>
        /// Registers a service of the specified type with a specific implementation type and service lifetime.
        /// </summary>
        /// <typeparam name="TImplementation">The implementation type of the service to be registered.</typeparam>
        /// <param name="serviceLifetime">The lifetime of the service being registered (Singleton, Scoped, or Transient).</param>
        /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
        public RegistrationContext<TService> Add<TImplementation>(ServiceLifetime serviceLifetime)
            where TImplementation : class, TService
        {
            Type serviceType = typeof(TService);
            Type implementationType = typeof(TImplementation);
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(serviceType, implementationType, serviceLifetime);
            services.Add(serviceDescriptor);
            return this;
        }

        /// <summary>
        /// Registers a service of the specified type with a factory method for creating the service instance and service lifetime.
        /// </summary>
        /// <param name="implementationFactory">The factory method used to create the service instance.</param>
        /// <param name="serviceLifetime">The lifetime of the service being registered (Singleton, Scoped, or Transient).</param>
        /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
        public RegistrationContext<TService> Add(Func<IServiceProvider, TService> implementationFactory, ServiceLifetime serviceLifetime)
        {
            Type serviceType = typeof(TService);
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(serviceType, implementationFactory, serviceLifetime);
            services.Add(serviceDescriptor);
            return this;
        }

        /// <summary>
        /// Registers a service using a pre-configured ServiceDescriptor.
        /// </summary>
        /// <param name="serviceDescriptor">The ServiceDescriptor that encapsulates all the information to register the service.</param>
        /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
        public RegistrationContext<TService> Add(ServiceDescriptor serviceDescriptor)
        {
            services.Add(serviceDescriptor);
            return this;
        }

        /// <summary>
        /// Registers decorators for the service, each taking the service instance and IServiceProvider as parameters.
        /// </summary>
        public RegistrationContext<TService> WithDecorators(params Func<IServiceProvider, TService, TService>[] decoratorFactories)
        {
            return ApplyDecorators(decoratorFactories.Select(df =>
                new Func<IServiceProvider, Func<TService, TService>>(provider =>
                    service => df(provider, service)
                )
            ).ToArray());
        }

        /// <summary>
        /// Registers decorators for the service, each represented as a factory function.
        /// </summary>
        public RegistrationContext<TService> WithDecorators(params Func<IServiceProvider, Func<TService, TService>>[] decoratorFactories)
        {
            return ApplyDecorators(decoratorFactories);
        }

        /// <summary>
        /// Registers a single decorator for the service.
        /// </summary>
        public RegistrationContext<TService> WithDecorator<TDecorator>(Func<IServiceProvider, TService, TDecorator> decoratorFactory) where TDecorator : class, TService
        {
            Type serviceType = typeof(TService);
            ServiceDescriptor? originalServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);

            if (originalServiceDescriptor == null)
                throw new InvalidOperationException($"The service of type {serviceType.Name} has not been registered and cannot be decorated.");

            ServiceLifetime lifetime = originalServiceDescriptor.Lifetime;

            object ImplementationFactory(IServiceProvider serviceProvider)
            {
                TService originalService = GetOriginalService(originalServiceDescriptor, serviceProvider);
                return decoratorFactory(serviceProvider, originalService);
            }

            ServiceDescriptor decoratorDescriptor = ServiceDescriptor.Describe(serviceType, ImplementationFactory, lifetime);
            services.Replace(decoratorDescriptor);

            return this;
        }

        private RegistrationContext<TService> ApplyDecorators(Func<IServiceProvider, Func<TService, TService>>[] decoratorFactories)
        {
            ServiceDescriptor serviceDescriptor = GetServiceDescriptor();
            Func<IServiceProvider, TService> decoratedFactory = CreateDecoratedFactory(serviceDescriptor, decoratorFactories);
            ReplaceServiceDescriptor(serviceDescriptor, decoratedFactory);

            return this;
        }

        private ServiceDescriptor GetServiceDescriptor()
        {
            ServiceDescriptor? serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
            if (serviceDescriptor == null)
                throw new InvalidOperationException($"The service of type {typeof(TService).Name} has not been registered.");

            return serviceDescriptor;
        }

        private static Func<IServiceProvider, TService> CreateDecoratedFactory(ServiceDescriptor serviceDescriptor, Func<IServiceProvider, Func<TService, TService>>[] decoratorFactories)
        {
            return provider =>
            {
                TService service = GetOriginalService(serviceDescriptor, provider);
                foreach (Func<IServiceProvider, Func<TService, TService>> decoratorFactory in decoratorFactories)
                    service = decoratorFactory(provider)(service);

                return service;
            };
        }

        private void ReplaceServiceDescriptor(ServiceDescriptor originalDescriptor, Func<IServiceProvider, TService> decoratedFactory)
        {
            services.Replace(ServiceDescriptor.Describe(
                typeof(TService),
                decoratedFactory,
                originalDescriptor.Lifetime)
            );
        }

        private static TService GetOriginalService(ServiceDescriptor serviceDescriptor, IServiceProvider provider)
        {
            if (serviceDescriptor.ImplementationFactory != null)
                return (TService)serviceDescriptor.ImplementationFactory(provider);

            if (serviceDescriptor.ImplementationInstance != null)
                return (TService)serviceDescriptor.ImplementationInstance;

            if (serviceDescriptor.ImplementationType != null)
                return (TService)ActivatorUtilities.CreateInstance(provider, serviceDescriptor.ImplementationType);

            throw new InvalidOperationException("The registration method for the original service is not supported.");
        }
    }
}
