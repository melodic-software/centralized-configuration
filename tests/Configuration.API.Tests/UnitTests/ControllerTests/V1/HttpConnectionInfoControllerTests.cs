using System.Net;
using AutoMapper;
using Configuration.API.Client.DTOs.Output.V1;
using Configuration.API.Controllers;
using Configuration.API.Tests.UnitTests.Services;
using Configuration.AutoMapper.Profiles.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Configuration.API.Tests.UnitTests.ControllerTests.V1;

public class HttpConnectionInfoControllerTests
{
    [Fact]
    public void GetHttpConnectionInfo_InputFromHttpConnectionFeature_MustReturnInputtedIps()
    {
        // ARRANGE
        string connectionId = "0HMTU1BU7HMJT";
        IPAddress localIpAddress = IPAddress.Parse("111.111.111.111");
        int localPort = 5000;
        IPAddress remoteIpAddress = IPAddress.Parse("222.222.222.222");
        int remotePort = 8080;

        Mock<IFeatureCollection> featureCollectionMock = new Mock<IFeatureCollection>();

        featureCollectionMock
            .Setup(e => e.Get<IHttpConnectionFeature>())
            .Returns(new HttpConnectionFeature
            {
                ConnectionId = connectionId,
                LocalIpAddress = localIpAddress,
                LocalPort = localPort,
                RemoteIpAddress = remoteIpAddress,
                RemotePort = remotePort
            });

        // can't use a default http context since we need to modify the features property (which is read-only)
        //HttpContext defaultHttpContext = HttpContextCreationService.CreateDefaultContext();

        // we use a mock instead
        Mock<HttpContext> httpContextMock = new Mock<HttpContext>();

        // whenever features is accessed, return our mock implementation
        httpContextMock.Setup(x => x.Features)
            .Returns(featureCollectionMock.Object);

        IMapper mapper = AutoMapperInstanceCreator.CreateWithProfile<HttpConnectionInfoProfile>();

        HttpConnectionInfoController controller = new HttpConnectionInfoController(mapper);

        // we can't do this because the property is read-only on the controller
        // controller.HttpContext = httpContextMock.Object;

        // but we can create a new controller context
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object,
        };

        // ACT
        ActionResult<HttpConnectionInfoDto> result = controller.Get();

        // ASSERT
        ActionResult<HttpConnectionInfoDto> actionResult = Assert.IsType<ActionResult<HttpConnectionInfoDto>>(result);
        OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        HttpConnectionInfoDto dto = Assert.IsType<HttpConnectionInfoDto>(okObjectResult.Value);

        Assert.Equal(connectionId, dto.ConnectionId);
        Assert.Equal(localIpAddress.ToString(), dto.LocalIpAddress);
        Assert.Equal(localPort, dto.LocalPort);
        Assert.Equal(remoteIpAddress.ToString(), dto.RemoteIpAddress);
        Assert.Equal(remotePort, dto.RemotePort);
    }
}