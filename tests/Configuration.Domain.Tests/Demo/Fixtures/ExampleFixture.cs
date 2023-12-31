namespace Configuration.Domain.Tests.Demo.Fixtures;
// this fixture type can be used as a generic type parameter of "IClassFixture"
// test classes can implement this interface
// the test class will need to add a constructor parameter of the fixture type

// using a fixture means that the context is reused and only has to be initialized (and disposed) once
// otherwise the dependencies involved here would be created (and disposed) for each and every test method in the test class
// https://app.pluralsight.com/course-player?clipId=0484ebd6-a8ea-4043-ad9b-0df159976339

// if the fixture needs to be shared across tests use a collection fixture
// https://app.pluralsight.com/course-player?clipId=5a0c365c-257a-4e96-b192-33d73325b482

public class ExampleFixture : IDisposable
{
    // by making this a readonly property, tests can get the instances via property wrappers
    // but they cannot set the properties to new instances
    private string Test { get; }

    public ExampleFixture()
    {
        // add any dependencies required by the test (that can be shared)
        Test = "This is a test!";
    }

    public void Dispose()
    {
        // clean up the test context / setup code, if required
    }
}