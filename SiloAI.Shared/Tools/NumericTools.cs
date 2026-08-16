
public static class NumericTools
{
    public static bool NotEquals(this int s, int compareString)
    => !s.Equals(compareString);

    public static bool HasValue(this int? i)
    => i is not null;

    public static bool HasNoValue(this int? i)
    => i is null;
}
