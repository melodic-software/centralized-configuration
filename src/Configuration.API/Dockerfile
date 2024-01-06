#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["src/Configuration.Api/Configuration.API.csproj", "src/Configuration.Api/"]
COPY ["src/Configuration.API.Client/Configuration.API.Client.csproj", "src/Configuration.API.Client/"]
COPY ["src/Enterprise.API.Client/Enterprise.API.Client.csproj", "src/Enterprise.API.Client/"]
COPY ["src/Configuration.ApplicationServices.FluentValidation/Configuration.ApplicationServices.FluentValidation.csproj", "src/Configuration.ApplicationServices.FluentValidation/"]
COPY ["src/Configuration.ApplicationServices.Commands/Configuration.ApplicationServices.Commands.csproj", "src/Configuration.ApplicationServices.Commands/"]
COPY ["src/Configuration.Domain/Configuration.Domain.csproj", "src/Configuration.Domain/"]
COPY ["src/Enterprise.DomainDrivenDesign/Enterprise.DomainDrivenDesign.csproj", "src/Enterprise.DomainDrivenDesign/"]
COPY ["src/Enterprise.Core/Enterprise.Core.csproj", "src/Enterprise.Core/"]
COPY ["src/Enterprise.Domain/Enterprise.Domain.csproj", "src/Enterprise.Domain/"]
COPY ["src/Enterprise.Events/Enterprise.Events.csproj", "src/Enterprise.Events/"]
COPY ["src/Enterprise.ApplicationServices/Enterprise.ApplicationServices.csproj", "src/Enterprise.ApplicationServices/"]
COPY ["src/Configuration.ApplicationServices/Configuration.ApplicationServices.csproj", "src/Configuration.ApplicationServices/"]
COPY ["src/Enterprise.DateTimes/Enterprise.DateTimes.csproj", "src/Enterprise.DateTimes/"]
COPY ["src/Enterprise.Calculations/Enterprise.Calculations.csproj", "src/Enterprise.Calculations/"]
COPY ["src/Enterprise.Exceptions/Enterprise.Exceptions.csproj", "src/Enterprise.Exceptions/"]
COPY ["src/Enterprise.Validation/Enterprise.Validation.csproj", "src/Enterprise.Validation/"]
COPY ["src/Configuration.AutoMapper/Configuration.AutoMapper.csproj", "src/Configuration.AutoMapper/"]
COPY ["src/Configuration.ApplicationServices.Queries/Configuration.ApplicationServices.Queries.csproj", "src/Configuration.ApplicationServices.Queries/"]
COPY ["src/Configuration.Core.Queries/Configuration.Core.Queries.csproj", "src/Configuration.Core.Queries/"]
COPY ["src/Enterprise.Core.Queries/Enterprise.Core.Queries.csproj", "src/Enterprise.Core.Queries/"]
COPY ["src/Enterprise.Sorting/Enterprise.Sorting.csproj", "src/Enterprise.Sorting/"]
COPY ["src/Enterprise.Constants/Enterprise.Constants.csproj", "src/Enterprise.Constants/"]
COPY ["src/Enterprise.Mapping/Enterprise.Mapping.csproj", "src/Enterprise.Mapping/"]
COPY ["src/Configuration.EntityFramework/Configuration.EntityFramework.csproj", "src/Configuration.EntityFramework/"]
COPY ["src/Enterprise.DesignPatterns/Enterprise.DesignPatterns.csproj", "src/Enterprise.DesignPatterns/"]
COPY ["src/Enterprise.EntityFramework/Enterprise.EntityFramework.csproj", "src/Enterprise.EntityFramework/"]
COPY ["src/Enterprise.Hosting/Enterprise.Hosting.csproj", "src/Enterprise.Hosting/"]
COPY ["src/Configuration.DI.Commands/Configuration.DI.Commands.csproj", "src/Configuration.DI.Commands/"]
COPY ["src/Configuration.Dapper.Commands/Configuration.Dapper.Commands.csproj", "src/Configuration.Dapper.Commands/"]
COPY ["src/Configuration.Dapper/Configuration.Dapper.csproj", "src/Configuration.Dapper/"]
COPY ["src/Enterprise.Dapper/Enterprise.Dapper.csproj", "src/Enterprise.Dapper/"]
COPY ["src/Configuration.EntityFramework.Commands/Configuration.EntityFramework.Commands.csproj", "src/Configuration.EntityFramework.Commands/"]
COPY ["src/Configuration.EventHandlers/Configuration.EventHandlers.csproj", "src/Configuration.EventHandlers/"]
COPY ["src/Enterprise.ApplicationServices.DI/Enterprise.ApplicationServices.DI.csproj", "src/Enterprise.ApplicationServices.DI/"]
COPY ["src/Enterprise.ApplicationServices.Decorators/Enterprise.ApplicationServices.Decorators.csproj", "src/Enterprise.ApplicationServices.Decorators/"]
COPY ["src/Enterprise.DI.DotNet/Enterprise.DI.DotNet.csproj", "src/Enterprise.DI.DotNet/"]
COPY ["src/Enterprise.Events.DI/Enterprise.Events.DI.csproj", "src/Enterprise.Events.DI/"]
COPY ["src/Configuration.DI.Queries/Configuration.DI.Queries.csproj", "src/Configuration.DI.Queries/"]
COPY ["src/Configuration.Dapper.Queries/Configuration.Dapper.Queries.csproj", "src/Configuration.Dapper.Queries/"]
COPY ["src/Configuration.EntityFramework.Queries/Configuration.EntityFramework.Queries.csproj", "src/Configuration.EntityFramework.Queries/"]
COPY ["src/Enterprise.Sorting.Dynamic/Enterprise.Sorting.Dynamic.csproj", "src/Enterprise.Sorting.Dynamic/"]
COPY ["src/Configuration.DI/Configuration.DI.csproj", "src/Configuration.DI/"]
COPY ["src/Enterprise.API/Enterprise.API.csproj", "src/Enterprise.API/"]
COPY ["src/Enterprise.Applications.DotNet/Enterprise.Applications.DotNet.csproj", "src/Enterprise.Applications.DotNet/"]
COPY ["src/Enterprise.Reflection/Enterprise.Reflection.csproj", "src/Enterprise.Reflection/"]
COPY ["src/Enterprise.DataShaping/Enterprise.DataShaping.csproj", "src/Enterprise.DataShaping/"]
COPY ["src/Enterprise.Library/Enterprise.Library.csproj", "src/Enterprise.Library/"]
COPY ["src/Enterprise.Logging.NLog/Enterprise.Logging.NLog.csproj", "src/Enterprise.Logging.NLog/"]
COPY ["src/Enterprise.Logging/Enterprise.Logging.csproj", "src/Enterprise.Logging/"]
COPY ["src/Enterprise.TextEncoding/Enterprise.TextEncoding.csproj", "src/Enterprise.TextEncoding/"]
COPY ["src/Enterprise.Logging.Serilog/Enterprise.Logging.Serilog.csproj", "src/Enterprise.Logging.Serilog/"]
COPY ["src/Enterprise.Middleware/Enterprise.Middleware.csproj", "src/Enterprise.Middleware/"]
COPY ["src/Enterprise.Monitoring/Enterprise.Monitoring.csproj", "src/Enterprise.Monitoring/"]
COPY ["src/Enterprise.Serialization.Json/Enterprise.Serialization.Json.csproj", "src/Enterprise.Serialization.Json/"]
COPY ["src/Enterprise.Traceability/Enterprise.Traceability.csproj", "src/Enterprise.Traceability/"]
COPY ["src/Configuration.EntityFramework.Sqlite/Configuration.EntityFramework.Sqlite.csproj", "src/Configuration.EntityFramework.Sqlite/"]
COPY ["src/Configuration.EntityFramework.SqlServer/Configuration.EntityFramework.SqlServer.csproj", "src/Configuration.EntityFramework.SqlServer/"]
RUN dotnet restore "./src/Configuration.Api/./Configuration.API.csproj"
COPY . .
WORKDIR "/src/src/Configuration.Api"
RUN dotnet build "./Configuration.API.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Configuration.API.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Configuration.API.dll"]