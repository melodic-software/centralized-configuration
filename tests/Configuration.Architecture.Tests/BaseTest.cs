using Configuration.ApplicationServices;
using Configuration.ApplicationServices.FluentValidation;
using Configuration.Domain;
using Configuration.Infrastructure;
using System.Reflection;

namespace Configuration.Architecture.Tests
{
    public class BaseTest
    {
        protected static Assembly ApplicationServiceAssembly => typeof(ApplicationServiceAssembly).Assembly;
        protected static Assembly ApplicationServiceFluentValidationAssembly => typeof(AppServiceFluentValidationAssembly).Assembly;
        protected static Assembly DomainAssembly => typeof(DomainAssembly).Assembly;
        protected static Assembly InfrastructureAssembly => typeof(InfrastructureAssembly).Assembly;
    }
}
