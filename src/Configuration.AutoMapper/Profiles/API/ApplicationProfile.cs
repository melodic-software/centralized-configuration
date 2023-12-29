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
        CreateMap<UpdateApplicationDto, CreateApplicationDto>()
            .ConstructUsing(x => new CreateApplicationDto()
            {
                Id = null, // must be assigned after mapping
                Name = x.Name,
                AbbreviatedName = x.AbbreviatedName,
                Description = x.Description,
                IsActive = x.IsActive
            });

        CreateMap<CreateApplicationDto, UpdateApplicationDto>()
            .ConstructUsing(x => new UpdateApplicationDto()
            {
                Name = x.Name,
                AbbreviatedName = x.AbbreviatedName,
                Description = x.Description,
                IsActive = x.IsActive ?? false
            });
    }
}