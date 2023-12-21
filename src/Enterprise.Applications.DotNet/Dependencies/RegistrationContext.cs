using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.Applications.DotNet.Dependencies
{
    public class RegistrationContext<TService>(IServiceCollection services)
        where TService : class
    {
        public RegistrationContext<TService> AddSingleton<TImplementation>()
        {
            services.AddSingleton(typeof(TService), typeof(TImplementation));
            return this;
        }

        public RegistrationContext<TService> AddSingleton(Func<IServiceProvider, TService> implementationFactory)
        {
            services.AddSingleton(implementationFactory);
            return this;
        }

        public IServiceCollection WithDecorator<TDecorator>()
            where TDecorator : class, TService
        {
            (Type serviceType, ServiceLifetime lifetime) = GetServiceDescriptorDetails();
            Type decoratorType = typeof(TDecorator);
            ServiceDescriptor decoratorDescriptor = ServiceDescriptor.Describe(serviceType, decoratorType, lifetime);

            services.Replace(decoratorDescriptor);
            return services;
        }

        public IServiceCollection WithDecorator<TDecorator>(Func<IServiceProvider, TService, TDecorator> decoratorFactory)
            where TDecorator : class, TService
        {
            (Type? serviceType, ServiceLifetime lifetime) = GetServiceDescriptorDetails();

            object ImplementationFactory(IServiceProvider serviceProvider)
            {
                TService originalService = (TService)serviceProvider.GetRequiredService(serviceType);
                return decoratorFactory.Invoke(serviceProvider, originalService);
            }

            services.Replace(ServiceDescriptor.Describe(serviceType, ImplementationFactory, lifetime));

            return services;
        }

        private (Type, ServiceLifetime) GetServiceDescriptorDetails()
        {
            ServiceDescriptor? originalServiceDescriptor = services
                .LastOrDefault(descriptor => descriptor.ServiceType == typeof(TService));

            if (originalServiceDescriptor == null)
                throw new InvalidOperationException($"Service type {typeof(TService).Name} has not been registered.");

            return (typeof(TService), originalServiceDescriptor.Lifetime);
        }
    }
}