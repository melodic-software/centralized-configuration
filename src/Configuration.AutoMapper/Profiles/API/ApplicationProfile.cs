using AutoMapper;
using Configuration.API.Client.Models.Input.V1;

namespace Configuration.AutoMapper.Profiles.API;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        ApiModelContractsToApiModelContracts();
    }

    public void ApiModelContractsToApiModelContracts()
    {
        CreateMap<UpdateApplicationModel, CreateApplicationModel>()
            .ConstructUsing(x => new CreateApplicationModel()
            {
                Id = null, // must be assigned after mapping
                Name = x.Name,
                AbbreviatedName = x.AbbreviatedName,
                Description = x.Description,
                IsActive = x.IsActive
            });

        CreateMap<CreateApplicationModel, UpdateApplicationModel>()
            .ConstructUsing(x => new UpdateApplicationModel()
            {
                Name = x.Name,
                AbbreviatedName = x.AbbreviatedName,
                Description = x.Description,
                IsActive = x.IsActive ?? false
            });
    }
}