using AutoMapper;
using Configuration.API.Client.Models.Output.V2;
using Configuration.API.Controllers.Applications.V2;
using Configuration.API.Test.UnitTests.Services;
using Configuration.ApplicationServices.Queries.Applications;
using Configuration.AutoMapper.Profiles.Queries.V2;
using Configuration.Core.Queries.Model;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Configuration.API.Test.UnitTests.ControllerTests.V2;

public class ApplicationsControllerTests
{
    [Fact]
    public async Task GetApplicationById_GetAction_MustReturnOkObjectResultWithApplicationModel()
    {
        // ARRANGE
        IMapper mapper = AutoMapperInstanceCreator.CreateWithProfile<ApplicationProfile>();

        ApplicationsControllerV2 applicationsController = new ApplicationsControllerV2(mapper);

        Guid applicationId = new Guid("5e950823-fe27-4c22-bf4b-7c7d429517ea");

        Application application = new Application(
            applicationId,
            "Test Application-5e950823",
            "Test Application",
            "TEST",
            "This is a test application",
            true
        );

        Mock<IHandleQuery<GetApplicationById, Application?>> queryHandlerMock = new Mock<IHandleQuery<GetApplicationById, Application?>>();
        queryHandlerMock.Setup(x => x.HandleAsync(It.IsAny<GetApplicationById>())).ReturnsAsync(application);

        // ACT
        ActionResult<ApplicationModel> result = await applicationsController.GetApplicationById(applicationId, queryHandlerMock.Object);

        // ASSERT
        ActionResult<ApplicationModel> actionResult = Assert.IsType<ActionResult<ApplicationModel>>(result);
        OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        ApplicationModel applicationModel = Assert.IsType<ApplicationModel>(okObjectResult.Value);

        // if the controller and action under test has an IEnumerable<TModel>, we could test it using this
        //IEnumerable<ApplicationModel> enumerable = Assert.IsAssignableFrom<IEnumerable<ApplicationModel>>(okObjectResult.Value);

        // this tests the actual mapping code (see comment above w/ mapping dependency)
        Assert.True(applicationModel.Id == application.Id);
        Assert.True(applicationModel.Name == application.Name);
        Assert.True(applicationModel.UniqueName == application.UniqueName);
        Assert.True(applicationModel.IsActive == application.IsActive);
    }
}