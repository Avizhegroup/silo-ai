public static class UrlTools
{
    public static string RemoveUrlSitePart(string url, string seprator)
    => string.Join(seprator, url.Split('/').Skip(3));
}
