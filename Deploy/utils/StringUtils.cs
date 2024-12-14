namespace Deploy.utils;

public static class StringUtils
{
    public static bool IsGreaterThan(this string thisString, string otherString) => string.CompareOrdinal(thisString, otherString) > 0;
}