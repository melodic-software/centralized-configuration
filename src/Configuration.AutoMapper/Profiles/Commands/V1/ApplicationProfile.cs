using AutoMapper;
using Configuration.API.Client.DTOs.Output.V1;
using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Configuration.EntityFramework.Entities;
using ApplicationId = Configuration.Domain.Applications.ApplicationId;

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
            .ConstructUsing(x => new Application(new ApplicationId(x.DomainId), x.Name, x.AbbreviatedName, x.Description, x.IsActive));
    }

    public void DomainEventsToApiModelContracts()
    {
        CreateMap<ApplicationCreated, ApplicationDto>()
            .ConstructUsing(x => new ApplicationDto(x.ApplicationId, x.UniqueName, x.Name, x.AbbreviatedName, x.Description, false));
    }
}