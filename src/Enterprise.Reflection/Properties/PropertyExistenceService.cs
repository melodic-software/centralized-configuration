using Enterprise.Constants;
using Enterprise.Reflection.Properties.Abstract;
using System.Reflection;

namespace Enterprise.Reflection.Properties;

public class PropertyExistenceService : IPropertyExistenceService
{
    public bool TypeHasProperties<T>(string? properties,
        BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
    {
        if (string.IsNullOrEmpty(properties))
            return true;

        Type sourceType = typeof(T);

        string[] propertySplit = properties.Split(CharacterConstants.Comma);
        string[] trimmedPropertyNames = propertySplit.Select(x => x.Trim()).ToArray();

        bool typeHasProperties = trimmedPropertyNames
            .Select(propertyName => sourceType.GetProperty(propertyName, bindingFlags))
            .All(propertyInfo => propertyInfo != null);

        return typeHasProperties;
    }
}