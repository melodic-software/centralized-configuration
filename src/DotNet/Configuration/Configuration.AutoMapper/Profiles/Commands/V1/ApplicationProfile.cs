using AutoMapper;
using Configuration.API.Client.Models.Output.V1;
using Configuration.Core.Domain.Model.Entities;
using Configuration.Core.Domain.Model.Events;
using Configuration.EntityFramework.Entities;

namespace Configuration.AutoMapper.Profiles.Commands.V1;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        DomainModelsToEntityFrameworkEntities();
        EntityFrameworkEntitiesToDomainModels();
        DomainEventsToApiModelContracts();
    }

    public void DomainModelsToEntityFrameworkEntities()
    {
        // TODO: add mapping profiles
    }

    public void EntityFrameworkEntitiesToDomainModels()
    {
        CreateMap<ApplicationEntity, Application>()
            .ConstructUsing(x => new Application(x.DomainId, x.Name, x.AbbreviatedName, x.Description, x.IsActive));
    }

    public void DomainEventsToApiModelContracts()
    {
        CreateMap<ApplicationCreated, ApplicationModel>()
            .ConstructUsing(x => new ApplicationModel(x.ApplicationId, x.UniqueName, x.Name, x.AbbreviatedName, x.Description, false));
    }
}