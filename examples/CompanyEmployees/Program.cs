using CompanyEmployees.Presentation;
using Contracts;
using Enterprise.API;
using Enterprise.API.Controllers.Options;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

ApiConfigurationService.Configure(args, options =>
{
    options.HttpRequestMiddlewareOptions.AddCustomMiddleware = app =>
    {
        
    };

    options.ServiceConfigurationOptions.ControllerConfigurationOptions = new ControllerConfigurationOptions()
    {
        ControllerAssemblyTypes = [typeof(AssemblyReference)],
        EnableGlobalAuthorizeFilter = false
    };

    options.ServiceConfigurationOptions.RegisterCustomServices = (services, builder) =>
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
    };
});