namespace Configuration.API.Client.DTOs.Output.V1;

/// <summary>
/// The model for HTTP connection information.
/// </summary>
public class HttpConnectionInfoDto
{
    public string ConnectionId { get; set; } = null!;
    public string? LocalIpAddress { get; set; }
    public int LocalPort { get; set; }
    public string? RemoteIpAddress { get; set; }
    public int RemotePort { get; set; }

    public HttpConnectionInfoDto(string connectionId, string? localIpAddress, int localPort, string? remoteIpAddress, int remotePort)
    {
        ConnectionId = connectionId;
        LocalIpAddress = localIpAddress;
        LocalPort = localPort;
        RemoteIpAddress = remoteIpAddress;
        RemotePort = remotePort;
    }

    public HttpConnectionInfoDto()
    {

    }
}