namespace Enterprise.Mapping.Properties.Model;

public class PropertyMappingValue
{
    public IEnumerable<string> DestinationProperties { get; private set; }

    public PropertyMappingValue(IEnumerable<string> destinationProperties)
    {
        DestinationProperties = destinationProperties ?? throw new ArgumentNullException(nameof(destinationProperties));
    }
}