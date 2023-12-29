using System.Net;
using AutoMapper;
using Configuration.API.Client.DTOs.Output.V1;
using Microsoft.AspNetCore.Http.Features;

namespace Configuration.AutoMapper.Profiles.API;

public class HttpConnectionInfoProfile : Profile
{
    public HttpConnectionInfoProfile()
    {
        CreateMap<IHttpConnectionFeature, HttpConnectionInfoDto>()
            .ConstructUsing((feature, context) =>
            {
                string connectionId = feature.ConnectionId;
                IPAddress? localIpAddress = feature.LocalIpAddress;
                int localPort = feature.LocalPort;
                IPAddress? remoteIpAddress = feature.RemoteIpAddress;
                int remotePort = feature.RemotePort;

                HttpConnectionInfoDto model = new HttpConnectionInfoDto
                {
                    ConnectionId = connectionId,
                    LocalIpAddress = localIpAddress?.ToString(),
                    LocalPort = localPort,
                    RemoteIpAddress = remoteIpAddress?.ToString(),
                    RemotePort = remotePort
                };

                return model;
            });
    }
}