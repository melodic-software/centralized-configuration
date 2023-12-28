using Configuration.DI;
using Configuration.DI.Commands;
using Configuration.DI.Queries;
using Enterprise.Applications.DotNet.Commands;
using Enterprise.Applications.DotNet.Queries;
using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Queries.Handlers;
using static Configuration.API.EntityFramework.EntityFrameworkConfiguration;

namespace Configuration.API.Dependencies;

public static class ApplicationDependencyRegistrar
{
    public static void RegisterCustomServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        ConfigureDbContexts(services, builder);

        // add additional custom service / DI registrations that are specific to this application below this line:

        services.RegisterEnterpriseServices();
        services.RegisterCommandServices();
        services.RegisterQueryServices(builder.Configuration);

        // services that are absolutely specific to this API (contain API specific implementation)
        RegisterApiServices(services);
    }

    public static void RegisterApiServices(IServiceCollection services)
    {
        services.AddTransient(provider =>
        {
            IResolveCommandHandler commandHandlerResolver = new CommandHandlerResolver(provider);

            return commandHandlerResolver;
        });

        services.AddTransient(provider =>
        {
            IResolveQueryHandler queryHandlerResolver = new QueryHandlerResolver(provider);

            return queryHandlerResolver;
        });
    }
}