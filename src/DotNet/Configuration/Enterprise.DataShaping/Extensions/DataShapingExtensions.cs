using System.Dynamic;
using System.Reflection;
using static Enterprise.DataShaping.DataShapedObjectCreationService;
using static Enterprise.DataShaping.PropertyInfoService;

namespace Enterprise.DataShaping.Extensions;

public static class DataShapingExtensions
{
    public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string? properties)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        List<PropertyInfo> propertyInfos = GetPropertyInfos<TSource>(properties, bindingFlags);
        List<ExpandoObject> dataShapedObjects = source.Select(sourceObject => CreateDataShapedObject(sourceObject, propertyInfos)).ToList();
        return dataShapedObjects;
    }

    /// <summary>
    /// This simply converts the source object to an ExpandoObject.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static ExpandoObject ShapeData<TSource>(this TSource source) => ShapeData(source, null);

    public static ExpandoObject ShapeData<TSource>(this TSource source, string? properties)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        ExpandoObject dataShapedObject = CreateDataShapedObject(source, properties, bindingFlags);
        return dataShapedObject;
    }
}