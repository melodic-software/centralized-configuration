using Xunit;

namespace Configuration.Domain.Tests.Demo;

public class IgnoredTest
{
    // it may be helpful to ignore / skip tests when they are first created
    // specifically for test driven development (TDD)
    // this should be temporary

    [Fact(Skip = "Skipping this one for demo reasons.")]
    public void Ignored_Example_Test()
    {

    }
}