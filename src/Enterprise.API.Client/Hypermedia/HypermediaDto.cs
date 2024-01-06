namespace Enterprise.API.Client.Hypermedia
{
    /// <summary>
    /// This is a hypermedia result contract that includes the resulting resource representation AND hypermedia links.
    /// NOTE: This is technically an envelope object, which breaks REST.
    /// TODO: Come back and fix this. Is there a way to do this without an envelope?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HypermediaDto<T>
    {
        public T? Value { get; set; }

        public List<HypermediaLinkDto> Links { get; set; }

        public HypermediaDto(T ? value, List<HypermediaLinkDto> links)
        {
            Value = value;
            Links = links;
        }

        public HypermediaDto()
        {
            Links = new List<HypermediaLinkDto>();
        }
    }
}
