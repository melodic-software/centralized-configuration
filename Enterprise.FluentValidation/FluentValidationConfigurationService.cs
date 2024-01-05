using Enterprise.Reflection.Assemblies;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.FluentValidation
{
    public static class FluentValidationConfigurationService
    {
        public static void RegisterFluentValidation(this IServiceCollection services)
        {
            // TODO: Define specific assemblies
            var assemblies = AssemblyService.GetSolutionAssemblies(x =>
                !string.IsNullOrWhiteSpace(x.Name) &&
                !x.Name.StartsWith("Microsoft") &&
                !x.Name.StartsWith("System"));

            services.AddValidatorsFromAssemblies(assemblies);
        }
    }
}
