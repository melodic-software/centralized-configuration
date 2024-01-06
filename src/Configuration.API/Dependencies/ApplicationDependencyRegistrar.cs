using Configuration.ApplicationServices.FluentValidation;
using Configuration.DI.Commands;
using Configuration.DI.Queries;
using Enterprise.Applications.DotNet.Commands;
using Enterprise.Applications.DotNet.Queries;
using Enterprise.ApplicationServices.Commands.Handlers.Resolution;
using Enterprise.ApplicationServices.Queries.Handlers;
using FluentValidation;
using static Configuration.API.EntityFramework.EntityFrameworkConfiguration;

namespace Configuration.API.Dependencies;

public static class ApplicationDependencyRegistrar
{
    public static void RegisterCustomServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        ConfigureDbContexts(services, builder);

        // add additional custom service / DI registrations that are specific to this application below this line:

        
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

        // FluentValidation
        // TODO: Where does this really need to live? Can it be automatic or configurable?
        services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
    }
}