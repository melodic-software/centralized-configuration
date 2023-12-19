using Enterprise.Mapping.Properties.Model;
using Enterprise.Mapping.Properties.Services;
using Enterprise.Sorting.Model;
using static Enterprise.Sorting.Constants.SortingConstants;

namespace Enterprise.Sorting.Services;

public static class SortService
{
    private static readonly PropertyNameService PropertyNameService;

    static SortService()
    {
        PropertyNameService = new SortedPropertyNameService();
    }

    public static string ApplyOrderByClause(string orderByClause, Dictionary<string, PropertyMappingValue> mappingDictionary, string orderByString)
    {
        // if the sort option ends with " desc", we order by descending, otherwise ascending
        bool orderDescending = orderByClause.EndsWith(DescSortSuffix);

        string propertyName = PropertyNameService.GetPropertyName(orderByClause);

        // find the matching property
        if (!mappingDictionary.ContainsKey(propertyName))
            throw new ArgumentException(MissingMappingKeyErrorMessage(propertyName));

        PropertyMappingValue? propertyMappingValue = mappingDictionary[propertyName];

        if (propertyMappingValue == null)
            throw new ArgumentNullException(nameof(propertyMappingValue));

        // reverse the sort order if necessary
        if (propertyMappingValue is SortablePropertyMappingValue { ReverseSortOrder: true })
            orderDescending = !orderDescending;

        // run through the property names
        foreach (string? destinationProperty in propertyMappingValue.DestinationProperties)
            orderByString = ApplyPropertySort(orderByString, orderDescending, destinationProperty);

        return orderByString;
    }

    private static string ApplyPropertySort(string orderByString, bool orderDescending, string destinationProperty)
    {
        string separator = string.IsNullOrWhiteSpace(orderByString) ? string.Empty : SortSeparator;
        string propertySortDirection = orderDescending ? DescendingSortSuffix : AscendingSortSuffix;
        orderByString += separator + destinationProperty + propertySortDirection;
        return orderByString;
    }

    private static string MissingMappingKeyErrorMessage(string propertyName) => $"Key mapping for {propertyName} is missing.";
}