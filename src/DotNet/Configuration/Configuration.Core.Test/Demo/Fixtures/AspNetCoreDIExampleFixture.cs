using Microsoft.Extensions.DependencyInjection;

namespace Configuration.Core.Test.Demo.Fixtures;

// test classes will implement the IClassFixture with the fixture using DI
// the fixture will also need to be a constructor parameter and stored in a private field

public class AspNetCoreDIExampleFixture : IDisposable
{
    private ServiceProvider _serviceProvider;

    //public IExampleRepository ExampleRepository
    //{
    //    get
    //    {
    //        return _serviceProvider.GetRequiredService<IExampleRepository>();
    //    }
    //}

    public AspNetCoreDIExampleFixture()
    {
        ServiceCollection services = new ServiceCollection();
        // lifetime depends on the use case
        //services.AddScoped<IExampleRepository, ExampleRepository>();

        // build provider 
        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        // clean up the setup code, if required
    }
}