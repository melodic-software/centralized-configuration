using AutoMapper;
using Configuration.API.Client.Models.Input.V1;
using Configuration.API.Client.Models.Output.V1;
using Configuration.ApplicationServices.Queries.Applications;
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
        CreateMap<GetApplicationsModel, GetApplications>()
            .ConstructUsing(x => new GetApplications(x.Name, x.AbbreviatedName, x.IsActive, x.SearchQuery, x.PageNumber, x.PageSize, x.OrderBy));
    }

    public void ApplicationServiceModelsToApiContracts()
    {
        CreateMap<Application, ApplicationModel>()
            .ConstructUsing(x => new ApplicationModel(x.Id, x.UniqueName, x.Name, x.AbbreviatedName, x.Description, x.IsActive));

        CreateMap<Application, ApplicationModel>()
            .ConstructUsing(x => new ApplicationModel(x.Id, x.UniqueName, x.Name, x.AbbreviatedName, x.Description, x.IsActive));

        CreateMap<Application, UpdateApplicationModel>()
            .ConstructUsing(x => new UpdateApplicationModel
            {
                Name = x.Name,
                AbbreviatedName = x.AbbreviatedName,
                Description = x.Description,
                IsActive = x.IsActive
            });
    }
}