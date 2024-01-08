namespace Configuration.Architecture.Tests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvent_Should_Have_DomainEventPostfix()
    {
        // TODO: All event classes should have a past tense suffix like "UserRegistered", "UserCreated", or "OrderNotFound".
        // This may not be possible or worth the effort since it would require an NLP library and word detection using PascalCase conventions.

        //var result = Types.InAssembly(DomainAssembly)
        //    .That()
        //    .ImplementInterface(typeof(IDomainEvent))
        //    .Should().HaveNameEndingWith("") 
        //    .GetResult();

        //result.IsSuccessful.Should().BeTrue();
    }
}