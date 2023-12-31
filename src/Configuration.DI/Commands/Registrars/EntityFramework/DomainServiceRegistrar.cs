﻿using Configuration.Domain.Applications;
using Configuration.EntityFramework.Commands.Services;
using Configuration.EntityFramework.DbContexts.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Commands.Registrars.EntityFramework;

internal class DomainServiceRegistrar
{
    internal static void RegisterDomainServices(IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            ConfigurationContext configurationContext = provider.GetRequiredService<ConfigurationContext>();
            IApplicationExistenceService applicationExistenceService = new ApplicationExistenceService(configurationContext);
            return applicationExistenceService;
        });
    }
}