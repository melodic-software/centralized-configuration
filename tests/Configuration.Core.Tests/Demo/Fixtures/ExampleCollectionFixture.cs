using Xunit;

namespace Configuration.Core.Tests.Demo.Fixtures;
// if the fixture needs to be shared across tests use a collection fixture
// https://app.pluralsight.com/course-player?clipId=5a0c365c-257a-4e96-b192-33d73325b482

// this collection fixture is the one you want to share across test classes
// you can think of this as a wrapper around the existing fixture

// unique name that identifies the test collection and allows us to refer to it
// test classes no longer need to implement IClassFixture<EmployeeServiceFixture>
// and instead only need to be decorated with the [Collection] attribute (using the definition name)
[CollectionDefinition("ExampleCollectionFixture")]
public class ExampleCollectionFixture : ICollectionFixture<ExampleFixture>
{

}