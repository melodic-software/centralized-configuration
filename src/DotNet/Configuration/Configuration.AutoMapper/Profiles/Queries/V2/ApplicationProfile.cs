using AutoMapper;
using Configuration.API.Client.Models.Output.V2;
using Configuration.Core.Queries.Model;

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
        CreateMap<Application, ApplicationModel>()
            .ConstructUsing(x => new ApplicationModel(x.Id, x.UniqueName, x.Name, x.IsActive));
    }
}