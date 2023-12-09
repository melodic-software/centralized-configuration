using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Enterprise.Testing.Services;
using Xunit;

namespace Configuration.API.Test.UnitTests.Demo.Controllers.Tests;

public class DemoControllerTests
{
    public const string UnitTestAuthenticationType = "UnitTest";

    [Fact]
    public void GetDemo_GetActionForUserWithoutAdminOrSpecialRole_MustReturnForbidden()
    {
        // ARRANGE
        DemoController controller = new DemoController();

        Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();
        mockPrincipal.Setup(x =>
            x.IsInRole(It.IsAny<string>())).Returns(false);

        Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(x => x.User)
            .Returns(mockPrincipal.Object);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        // ACT
        IActionResult result = controller.Get();

        // ASSERT
        IActionResult actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        ForbidResult forbidResult = Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public void GetDemo_GetActionForUserInSpecialRole_MustRedirectToGetSpecial()
    {
        // ARRANGE
        DemoController controller = new DemoController();

        List<Claim> userClaims = new List<Claim>
        {
            new(ClaimTypes.Name, "John"),
            new(ClaimTypes.Role, "Special")
        };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(userClaims, UnitTestAuthenticationType);
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        HttpContext httpContext = HttpContextCreationService.CreateDefaultContext(claimsPrincipal);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // ACT
        IActionResult result = controller.Get();

        // ASSERT
        IActionResult actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal(nameof(DemoController.GetSpecial), redirectToActionResult.ActionName);
        Assert.Equal(nameof(DemoController).Replace("Controller", string.Empty), redirectToActionResult.ControllerName);
    }

    [Fact]
    public void GetDemo_GetActionForUserInSpecialRole_MustRedirectToGetSpecial_Moq()
    {
        // ARRANGE
        DemoController controller = new DemoController();

        Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();
        mockPrincipal.Setup(x =>
            x.IsInRole(It.Is<string>(s => s == "Special"))).Returns(true);

        Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(x => x.User)
            .Returns(mockPrincipal.Object);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        // ACT
        IActionResult result = controller.Get();

        // ASSERT
        IActionResult actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal(nameof(DemoController.GetSpecial), redirectToActionResult.ActionName);
        Assert.Equal(nameof(DemoController).Replace("Controller", string.Empty), redirectToActionResult.ControllerName);
    }

    [Fact]
    public void GetDemo_GetActionForUserInAdminRole_MustReturnOk()
    {
        // ARRANGE
        DemoController controller = new DemoController();

        Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();
        mockPrincipal.Setup(x =>
            x.IsInRole(It.Is<string>(s => s == "Admin"))).Returns(true);

        Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(x => x.User)
            .Returns(mockPrincipal.Object);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        // ACT
        IActionResult result = controller.Get();

        // ASSERT
        IActionResult actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        OkResult okResult = Assert.IsType<OkResult>(result);
    }
}