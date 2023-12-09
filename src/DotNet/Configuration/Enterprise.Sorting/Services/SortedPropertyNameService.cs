using Enterprise.Mapping.Properties.Services;
using static Enterprise.Constants.CharacterConstants;

namespace Enterprise.Sorting.Services;

public class SortedPropertyNameService : PropertyNameService
{
    public override string GetPropertyName(string source)
    {
        // remove everything after the first space (if applicable)
        // if the property names are coming from an orderBy string, this part must be ignored (" asc" or " desc")
        // we get the property name to look for in the mapping dictionary

        int indexOfFirstSpace = source.IndexOf(Space, StringComparison.Ordinal);
        string propertyName = indexOfFirstSpace == -1 ? source : source.Remove(indexOfFirstSpace);
        return propertyName;
    }
}