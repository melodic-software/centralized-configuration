using Enterprise.Constants;
using System.Dynamic;
using System.Reflection;

namespace Enterprise.DataShaping;

public static class DataShapedObjectCreationService
{
    public static ExpandoObject CreateDataShapedObject<TSource>(TSource sourceObject, List<PropertyInfo> propertyInfos)
    {
        ExpandoObject dataShapedObject = new ExpandoObject();

        // Expando object implements IDictionary<string, object?>
        IDictionary<string, object?> dictionary = dataShapedObject;

        foreach (PropertyInfo propertyInfo in propertyInfos)
            PropertyMappingService.MapProperty(sourceObject, propertyInfo, dictionary);

        return dataShapedObject;
    }

    public static ExpandoObject CreateDataShapedObject<TSource>(TSource sourceObject, string? properties, BindingFlags bindingFlags)
    {
        ExpandoObject dataShapedObject = new ExpandoObject();

        // Expando object implements IDictionary<string, object?>
        IDictionary<string, object?> dictionary = dataShapedObject;

        Type sourceType = typeof(TSource);

        if (string.IsNullOrWhiteSpace(properties))
        {
            PropertyInfo[] propertyInfos = sourceType.GetProperties(bindingFlags);

            foreach (PropertyInfo propertyInfo in propertyInfos)
                PropertyMappingService.MapProperty(sourceObject, propertyInfo, dictionary);

            return dataShapedObject;
        }

        string[] propertySplit = properties.Split(CharacterConstants.Comma);
        string[] trimmedPropertyNames = propertySplit.Select(s => s.Trim()).ToArray();

        foreach (string propertyName in trimmedPropertyNames)
            PropertyMappingService.MapProperty(sourceObject, sourceType, propertyName, bindingFlags, dictionary);

        // return the shaped object
        return dataShapedObject;
    }
}