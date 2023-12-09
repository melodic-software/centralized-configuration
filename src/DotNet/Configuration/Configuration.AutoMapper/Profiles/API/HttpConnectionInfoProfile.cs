using System.Net;
using AutoMapper;
using Configuration.API.Client.Models.Output.V1;
using Microsoft.AspNetCore.Http.Features;

namespace Configuration.AutoMapper.Profiles.API;

public class HttpConnectionInfoProfile : Profile
{
    public HttpConnectionInfoProfile()
    {
        CreateMap<IHttpConnectionFeature, HttpConnectionInfoModel>()
            .ConstructUsing((feature, context) =>
            {
                string connectionId = feature.ConnectionId;
                IPAddress? localIpAddress = feature.LocalIpAddress;
                int localPort = feature.LocalPort;
                IPAddress? remoteIpAddress = feature.RemoteIpAddress;
                int remotePort = feature.RemotePort;

                HttpConnectionInfoModel model = new HttpConnectionInfoModel
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