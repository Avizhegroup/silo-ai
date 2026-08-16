
public static class EnumTools
{
    public static bool NotEquals(this Enum s, Enum compareEnum)
    => !s.Equals(compareEnum);
}
