﻿using Enterprise.Reflection.Properties;
using Enterprise.Reflection.Properties.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Applications.DotNet.Dependencies.Registrars;

internal class ReflectionServiceRegistrar
{
    internal static void RegisterReflectionServices(IServiceCollection services)
    {
        services.AddTransient(provider =>
        {
            IPropertyExistenceService propertyExistenceService = new PropertyExistenceService();
            return propertyExistenceService;
        });
    }
}