namespace GaleonServer.Core._Common;

public static class StringExtensions
{
    public static bool HasValue(string str)
    {
        return string.IsNullOrWhiteSpace(str) is false;
    }
}