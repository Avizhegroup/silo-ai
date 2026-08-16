namespace SiloAI.Shared;
public static class DictionaryTools
{
    /// <summary>
    /// Check if key exists, if so replace value, otherwise add new key-value pair
    /// </summary>
    public static Dictionary<T1, T2> ReplaceOrAdd<T1, T2>(this Dictionary<T1, T2> dict, T1 value1, T2 value2)
    {
        if (dict.ContainsKey(value1))
        {
            dict[value1] = value2;
        }
        else
        {
            dict.Add(value1, value2);
        }

        return dict;
    }
}
