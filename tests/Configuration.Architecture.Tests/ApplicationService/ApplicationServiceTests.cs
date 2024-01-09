using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Queries.Handlers;
using FluentAssertions;
using FluentValidation;
using NetArchTest.Rules;

namespace Configuration.Architecture.Tests.ApplicationService
{
    public class ApplicationServiceTests : BaseTest
    {
        [Fact]
        public void CommandHandler_Should_HaveNameEndingWith_Handler()
        {
            TestResult? result = Types.InAssembly(ApplicationServiceAssembly)
                .That()
                .ImplementInterface(typeof(IHandleCommand<>))
                .Or()
                .ImplementInterface(typeof(IHandleCommand<,>))
                .Should()
                .HaveNameEndingWith("Handler")
                .Or()
                .HaveNameEndingWith("CommandHandler")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void QueryHandler_Should_HaveNameEndingWith_QueryHandler()
        {
            TestResult? result = Types.InAssembly(ApplicationServiceAssembly)
                .That()
                .ImplementInterface(typeof(IHandleQuery<>))
                .Or()
                .ImplementInterface(typeof(IHandleQuery<,>))
                .Should()
                .HaveNameEndingWith("Handler")
                .Or()
                .HaveNameEndingWith("QueryHandler")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Validator_Should_HaveNameEndingWith_Validator()
        {
            TestResult? result = Types.InAssembly(ApplicationServiceFluentValidationAssembly)
                .That()
                .Inherit(typeof(AbstractValidator<>))
                .Should()
                .HaveNameEndingWith("Validator")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}