using Enterprise.Mapping.Properties.Model;
using Enterprise.Sorting.Services;
using System.Linq.Dynamic.Core;
using static Enterprise.Constants.CharacterConstants;

namespace Enterprise.Sorting.Dynamic.Extensions;

public static class SortingExtensions
{
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        if (mappingDictionary == null) throw new ArgumentNullException(nameof(mappingDictionary));

        if (string.IsNullOrWhiteSpace(orderBy))
            return query;

        string orderByString = string.Empty;

        // the orderBy string is separated by a comma, so we split it
        string[]? orderBySplit = orderBy.Split(Comma);

        // trim the orderBy clause, as it might contain leading or trailing spaces
        string[] trimmedOrderBySplit = orderBySplit.Select(x => x.Trim()).ToArray();

        // apply the orderBy clause
        foreach (string orderByClause in trimmedOrderBySplit)
            orderByString = SortService.ApplyOrderByClause(orderByClause, mappingDictionary, orderByString);

        IOrderedQueryable<T> result = query.OrderBy(orderByString);

        return result;
    }
}