namespace Enterprise.API.Client.Hypermedia;

public class HypermediaLinkModel
{
    /// <summary>
    /// Contains the URI to be invoked to execute this action.
    /// </summary>
    public string? Href { get; private set; }

    /// <summary>
    /// Identifies the type of action.
    /// </summary>
    public string? Rel { get; private set; }

    /// <summary>
    /// Defines the HTTP method to use.
    /// </summary>
    public string? Method { get; private set; }

    public HypermediaLinkModel(string? href, string? rel, string? method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }
}