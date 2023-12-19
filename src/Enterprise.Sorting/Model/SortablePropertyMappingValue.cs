using Enterprise.Mapping.Properties.Model;

namespace Enterprise.Sorting.Model;

public class SortablePropertyMappingValue : PropertyMappingValue
{
    public bool ReverseSortOrder { get; private set; }

    public SortablePropertyMappingValue(IEnumerable<string> destinationProperties, bool reverseSortOrder = false) 
        : base(destinationProperties)
    {
        ReverseSortOrder = reverseSortOrder;
    }
}