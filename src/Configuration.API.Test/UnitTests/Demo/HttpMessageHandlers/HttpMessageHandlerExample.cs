using System.Net;
using System.Text;
using System.Text.Json;

namespace Configuration.API.Test.UnitTests.Demo.HttpMessageHandlers;

// https://app.pluralsight.com/course-player?clipId=fb346498-0355-42d2-94ba-e4b36b97c3df
// https://app.pluralsight.com/course-player?clipId=8a69b7e5-6c95-4a38-877f-37083245bc02

// see this for how to use something similar with Moq in a unit test
// https://app.pluralsight.com/course-player?clipId=8a69b7e5-6c95-4a38-877f-37083245bc02
// this can be a better/easier alternative so that you don't have to create http message handlers for all calls

/// <summary>
/// This is an example implementation of a custom HttpMessageHandler.
/// Instances are intended to be passed into the constructor of an HttpClient.
/// </summary>
public class HttpMessageHandlerExample : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // we can short circuit an HTTP request and prevent the actual call from being made

        // NOTE: to make this more reusable for different scenarios, add a constructor that can allow for
        // setting model properties (under test) OR entire model representations (if necessary)

        // in most cases you'll new up a model object, serialize it, and create an HttpResponse manually

        var model = new { FirstName = "John", LastName = "Doe" };

        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        string json = JsonSerializer.Serialize(model, jsonSerializerOptions);
        Encoding encoding = Encoding.ASCII;
        string mediaType = "application/json";

        StringContent content = new StringContent(json, encoding, mediaType);

        HttpStatusCode statusCode = HttpStatusCode.OK;

        HttpResponseMessage response = new HttpResponseMessage(statusCode)
        {
            Content = content,
            //Headers = {},
            //ReasonPhrase = string.Empty,
            RequestMessage = request,
            StatusCode = statusCode,
            //TrailingHeaders = {},
            //Version = new Version(string.Empty),
        };

        return Task.FromResult(response);
    }
}