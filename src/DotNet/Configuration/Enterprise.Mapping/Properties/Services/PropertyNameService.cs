using static Enterprise.Constants.CharacterConstants;

namespace Enterprise.Mapping.Properties.Services;

public class PropertyNameService
{
    public virtual string GetPropertyName(string source)
    {
        // remove everything after the first space (if applicable)
        int indexOfFirstSpace = source.IndexOf(Space, StringComparison.Ordinal);
        string propertyName = indexOfFirstSpace == -1 ? source : source.Remove(indexOfFirstSpace);
        return propertyName;
    }
}