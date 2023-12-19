using Xunit;
using Xunit.Abstractions;

namespace Configuration.Core.Test.Demo;

public class TestOutputTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TestOutputTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test_Output_Test()
    {
        // this is the preferred method to reliably log messages
        _testOutputHelper.WriteLine("This is a test message");
    }
}