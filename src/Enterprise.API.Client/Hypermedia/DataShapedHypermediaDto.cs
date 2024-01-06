namespace Enterprise.API.Client.Hypermedia;

public class DataShapedHypermediaDto : HypermediaDto<IEnumerable<IDictionary<string, object?>>>
{
    public DataShapedHypermediaDto(IEnumerable<IDictionary<string, object?>> value, IEnumerable<HypermediaLinkDto> links)
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