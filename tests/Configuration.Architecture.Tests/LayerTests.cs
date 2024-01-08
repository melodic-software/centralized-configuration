using FluentAssertions;
using NetArchTest.Rules;

namespace Configuration.Architecture.Tests
{
    public class LayerTests : BaseTest
    {
        [Fact]
        public void DomainLayer_Should_NotHaveDependencyOn_ApplicationLayer()
        {
            TestResult? result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ApplicationServiceAssembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void DomainLayer_Should_NotHaveDependencyOn_InfrastructureLayer()
        {
            TestResult? result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void ApplicationLayer_Should_NotHaveDependencyOn_InfrastructureLayer()
        {
            TestResult? result = Types.InAssembly(ApplicationServiceAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
