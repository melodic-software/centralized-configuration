namespace Enterprise.API.Client.Hypermedia;

public class DataShapedHypermediaDto : HypermediaDto<List<IDictionary<string, object?>>>
{
    public DataShapedHypermediaDto(List<IDictionary<string, object?>> value, List<HypermediaLinkDto> links)
    {
        Value = value;
        Links = links;
    }

    public DataShapedHypermediaDto()
    {
        Value = new List<IDictionary<string, object?>>();
        Links = new List<HypermediaLinkDto>();
    }
}