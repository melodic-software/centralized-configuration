using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Configuration.API.Tests.IntegrationTests.Services;

// TODO: can this be moved into an HTTP(client) related library

public static class GetRequestService
{
    public static async Task<T?> Execute<T>(HttpClient httpClient, string route, ILogger logger)
    {
        HttpResponseMessage response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();
        string stringResult = await response.Content.ReadAsStringAsync();
        logger.LogInformation(stringResult);
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        T? result = JsonSerializer.Deserialize<T>(stringResult, jsonSerializerOptions);
        return result;
    }
}