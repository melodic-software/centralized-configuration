using System.Reflection;
using Enterprise.Constants;

namespace Enterprise.DataShaping;

public static class PropertyInfoService
{
    public static List<PropertyInfo> GetPropertyInfos<TSource>(string? properties,
        BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
    {
        List<PropertyInfo> propertyInfos = new List<PropertyInfo>();

        Type sourceType = typeof(TSource);

        if (string.IsNullOrWhiteSpace(properties))
        {
            // no data shaping has been requested
            // all public properties should be in the expando object
            PropertyInfo[] allPropertyInfos = sourceType.GetProperties(bindingFlags);

            propertyInfos.AddRange(allPropertyInfos);
        }
        else
        {
            string[] propertySplit = properties.Split(CharacterConstants.Comma);
            string[] trimmedPropertyNames = propertySplit.Select(x => x.Trim()).ToArray();

            foreach (string propertyName in trimmedPropertyNames)
                AddPropertyInfo(sourceType, propertyName, bindingFlags, propertyInfos);
        }

        return propertyInfos;
    }

    public static void AddPropertyInfo(Type sourceType, string propertyName, BindingFlags bindingFlags, List<PropertyInfo> propertyInfos)
    {
        PropertyInfo? propertyInfo = sourceType.GetProperty(propertyName, bindingFlags);

        if (propertyInfo == null)
            throw new Exception($"Property {propertyName} wasn't found on {sourceType}");

        propertyInfos.Add(propertyInfo);
    }
}