using AutoMapper;
using Enterprise.API.Client.Pagination;
using Enterprise.Core.Queries.Paging;

namespace Configuration.AutoMapper.Profiles.API;

public class ApiClientProfile : Profile
{
    public ApiClientProfile()
    {
        CreateMap<PaginationMetadata, PagingMetadataModel>()
            .ConstructUsing(x => new PagingMetadataModel(x.TotalCount, x.PageSize.Value, x.CurrentPage.Value, x.TotalPages.Value));
    }
}