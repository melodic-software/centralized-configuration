namespace Enterprise.API.Client.Hypermedia;

public class HypermediaLinkDto
{
    /// <summary>
    /// Contains the URI to be invoked to execute this action.
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// Identifies the type of action.
    /// </summary>
    public string? Rel { get; set; }

    /// <summary>
    /// Defines the HTTP method to use.
    /// </summary>
    public string? Method { get; set; }

    public HypermediaLinkDto(string? href, string? rel, string? method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }

    public HypermediaLinkDto()
    {

    }
}