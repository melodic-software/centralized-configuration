using Configuration.ApplicationServices;
using Configuration.ApplicationServices.FluentValidation;
using Configuration.Domain;
using Configuration.Infrastructure;
using System.Reflection;

namespace Configuration.Architecture.Tests
{
    public class BaseTest
    {
        protected static Assembly ApplicationServiceAssembly => typeof(ApplicationServiceAssemblyReference).Assembly;
        protected static Assembly ApplicationServiceFluentValidationAssembly => typeof(ApplicationServiceFluentValidationAssembly).Assembly;
        protected static Assembly DomainAssembly => typeof(DomainAssemblyReference).Assembly;
        protected static Assembly InfrastructureAssembly => typeof(InfrastructureAssemblyReference).Assembly;
    }
}
