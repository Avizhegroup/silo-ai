public static class ListTools
{
    public static List<T> Replace<T>(this List<T> source, Predicate<T> match, T item)
    {
        int index = source.FindIndex(match);

        if (index != -1)
        {
            source[index] = item;
        }

        return source;
    }
    
    public static List<T> ReplaceOrAdd<T>(this List<T> source, Predicate<T> match, T item)
    {
        int index = source.FindIndex(match);

        if (index != -1)
        {
            source[index] = item;
        }
        else
        {
            source.Add(item);
        }

        return source;
    }

    /// <summary>
    /// Not Any
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool Neither<T>(this List<T> source)
    => !source.Any();

    /// <summary>
    /// Not Any
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool Neither<T>(this List<T> source, Func<T, bool> predicate)
    => !source.Any(predicate);

    /// <summary>
    /// Does Not Exists
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool NotExists<T>(this List<T> source, Predicate<T> predicate)
    => !source.Exists(predicate);
}
