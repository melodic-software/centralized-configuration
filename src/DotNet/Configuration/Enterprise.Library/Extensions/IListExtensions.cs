namespace Enterprise.Library.Extensions;

public static class IListExtensions
{
    public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
        if (list == null)
            throw new ArgumentNullException(nameof(list));

        if (items == null)
            throw new ArgumentNullException(nameof(items));

        if (list is List<T> concreteList)
        {
            concreteList.AddRange(items);
        }
        else
        {
            foreach (T item in items)
                list.Add(item);
        }
    }
}