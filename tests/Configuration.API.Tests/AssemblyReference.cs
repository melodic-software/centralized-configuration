

// either of these will disable parallel test execution
//[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
//[assembly: CollectionBehavior(DisableTestParallelization = true)]

// another alternative is to use an xunit.runner.json configuration file
// parallelizeAssembly: true | false (default: false)
// parallelizeTestCollections: true | false (default: true)

namespace Configuration.API.Tests;

public static class AssemblyReference
{

}