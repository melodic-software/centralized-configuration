using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Configuration.API.Test.UnitTests.Mocking;

public static class ProblemDetailsFactoryMockService
{
    public static Mock<ProblemDetailsFactory> CreateProblemDetailsFactoryMock()
    {
        Mock<ILogger<ProblemDetailsFactory>> problemDetailsLoggerMock = new Mock<ILogger<ProblemDetailsFactory>>();

        ProblemDetailsOptions problemDetailsOptions = new ProblemDetailsOptions
        {
            FileProvider = new NullFileProvider(),
            SourceCodeLineCount = 0
        };

        IOptions<ProblemDetailsOptions> problemDetailsOptionsOptions = new OptionsWrapper<ProblemDetailsOptions>(problemDetailsOptions);

        Mock<IHostEnvironment> hostEnvironmentMock = new Mock<IHostEnvironment>();
        hostEnvironmentMock.Setup(x => x.ContentRootFileProvider).Returns(new NullFileProvider());

        Mock<ProblemDetailsFactory> problemDetailsFactoryMock = new Mock<ProblemDetailsFactory>(
            problemDetailsOptionsOptions,
            problemDetailsLoggerMock.Object,
            hostEnvironmentMock.Object
        );

        return problemDetailsFactoryMock;
    }
}