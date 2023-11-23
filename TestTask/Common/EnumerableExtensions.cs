using System.Collections;
using System.Collections.Generic;

public static class EnumerableExtensions
{
    public static IEnumerable<string> CastToString(this IEnumerable collection)
    {
        foreach (object item in collection)
        {
            yield return item?.ToString();
        }
    }

    public static IEnumerable<T> TakeOrAppend<T>(
        this IEnumerable<T> collection,
        int count,
        T appendValue = default)
    {
        int i = 0;
        IEnumerator<T> enumerator = collection.GetEnumerator();
        while (i < count && enumerator.MoveNext())
        {
            yield return enumerator.Current;
            i++;
        }
        while (i < count)
        {
            yield return appendValue;
            i++;
        }
    }
}
