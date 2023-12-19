using System.Reflection;

namespace Enterprise.DataShaping;

public static class PropertyMappingService
{
    public static void MapProperty<TSource>(TSource sourceObject, Type sourceType, string propertyName, BindingFlags bindingFlags, IDictionary<string, object?> dictionary)
    {
        PropertyInfo? propertyInfo = sourceType.GetProperty(propertyName, bindingFlags);

        if (propertyInfo == null)
            throw new Exception($"Property {propertyName} wasn't found on {typeof(TSource)}");

        MapProperty(sourceObject, propertyInfo, dictionary);
    }

    public static void MapProperty<TSource>(TSource source, PropertyInfo propertyInfo, IDictionary<string, object?> dictionary)
    {
        string propertyName = propertyInfo.Name;
        object? propertyValue = propertyInfo.GetValue(source);
        dictionary.Add(propertyName, propertyValue);
    }
}