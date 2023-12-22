﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static Microsoft.Extensions.DependencyInjection.ServiceLifetime;

namespace Enterprise.DI.DotNet.Dependencies
{
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

        public IServiceCollection WithDecorator<TDecorator>() where TDecorator : class, TService
        {
            return WithDecorator((serviceProvider, service) => ActivatorUtilities.CreateInstance<TDecorator>(serviceProvider, service));
        }

        public IServiceCollection WithDecorator<TDecorator>(Func<IServiceProvider, TService, TDecorator> decoratorFactory) where TDecorator : class, TService
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

            return services;
        }
    }
}