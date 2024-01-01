using AutoMapper;
using Configuration.API.Client.DTOs.Input.V1;
using Configuration.API.Controllers.Applications.V1.Partial;
using Configuration.API.Tests.UnitTests.Mocking;
using Configuration.API.Tests.UnitTests.Services;
using Configuration.ApplicationServices.Applications.CreateApplication;
using Configuration.AutoMapper.Profiles.Queries.V1;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Configuration.API.Tests.UnitTests.ControllerTests.V1;

public class ApplicationsControllerTests
{
    private readonly ApplicationsController _controller;

    public ApplicationsControllerTests()
    {
        Mock<ILogger<ApplicationsController>> loggerMock = new Mock<ILogger<ApplicationsController>>();
        IMapper mapper = AutoMapperInstanceCreator.CreateWithProfile<ApplicationProfile>();
        Mock<ProblemDetailsFactory> problemDetailsFactoryMock = ProblemDetailsFactoryMockService.CreateProblemDetailsFactoryMock();

        _controller = new ApplicationsController(loggerMock.Object, mapper, problemDetailsFactoryMock.Object);
    }

    [Fact]
    public async Task CreateApplication_CreateAction_NullModelResultsInBadRequest()
    {
        // ARRANGE
        CreateApplicationDto inputDto = new CreateApplicationDto();

        Mock<IHandleCommand<CreateApplication>> commandHandlerMock = new Mock<IHandleCommand<CreateApplication>>();

        // ACT
        IActionResult result = await _controller.Post(null, commandHandlerMock.Object);

        // ASSERT
        IActionResult actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        BadRequestResult badRequestResult = Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateApplication_CreateAction_InvalidModelStateResultsIn422UnprocessableEntity()
    {
        // ARRANGE
        CreateApplicationDto inputDto = new CreateApplicationDto();

        Mock<IHandleCommand<CreateApplication>> commandHandlerMock = new Mock<IHandleCommand<CreateApplication>>();

        // model state errors occur during model binding, which is part of the framework (we can't test this)
        // but we can manually act on the model state and test controller logic that deals with it
        _controller.ModelState.AddModelError("Name", "Required");

        // ACT
        IActionResult result = await _controller.Post(inputDto, commandHandlerMock.Object);

        // ASSERT
        IActionResult actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        UnprocessableEntityObjectResult unprocessableEntityResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
        Assert.IsType<SerializableError>(unprocessableEntityResult.Value);
    }
}