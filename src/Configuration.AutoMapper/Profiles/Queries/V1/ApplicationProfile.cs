using AutoMapper;
using Configuration.API.Client.DTOs.Input.V1;
using Configuration.API.Client.DTOs.Output.V1;
using Configuration.ApplicationServices.Queries.Applications.GetApplications;
using Configuration.Core.Queries.Model;

namespace Configuration.AutoMapper.Profiles.Queries.V1;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        ApiContractsToApplicationServiceModels();
        ApplicationServiceModelsToApiContracts();
    }

    public void ApiContractsToApplicationServiceModels()
    {
        CreateMap<GetApplicationsDto, GetApplications>()
            .ConstructUsing(x => new GetApplications(x.Name, x.AbbreviatedName, x.IsActive, x.SearchQuery, x.PageNumber, x.PageSize, x.OrderBy));
    }

    public void ApplicationServiceModelsToApiContracts()
    {
        CreateMap<Application, ApplicationDto>()
            .ConstructUsing(x => new ApplicationDto(x.Id, x.UniqueName, x.Name, x.AbbreviatedName, x.Description, x.IsActive));

        CreateMap<Application, ApplicationDto>()
            .ConstructUsing(x => new ApplicationDto(x.Id, x.UniqueName, x.Name, x.AbbreviatedName, x.Description, x.IsActive));

        CreateMap<Application, UpdateApplicationDto>()
            .ConstructUsing(x => new UpdateApplicationDto
            {
                Name = x.Name,
                AbbreviatedName = x.AbbreviatedName,
                Description = x.Description,
                IsActive = x.IsActive
            });
    }
}