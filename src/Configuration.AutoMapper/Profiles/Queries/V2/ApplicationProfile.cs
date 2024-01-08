using AutoMapper;
using Configuration.API.Client.DTOs.Output.V2;
using Configuration.ApplicationServices.Queries.Applications.Shared;

namespace Configuration.AutoMapper.Profiles.Queries.V2;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        ApiContractsToApplicationServiceModels();
        ApplicationServiceModelsToApiContracts();
    }

    public void ApiContractsToApplicationServiceModels()
    {

    }

    public void ApplicationServiceModelsToApiContracts()
    {
        CreateMap<ApplicationResult, ApplicationDto>()
            .ConstructUsing(x => new ApplicationDto(x.Id, x.UniqueName, x.Name, x.IsActive));
    }
}