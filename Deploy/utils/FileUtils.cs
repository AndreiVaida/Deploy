using System.IO;
using static System.Char;

namespace Deploy.utils;

public static class FileUtils
{
    private const int DateLength = 4 + 2 + 2;
    public static bool IsJar(string file) => Path.GetExtension(file).Equals(".jar");

    public static bool IsTxt(string file) => Path.GetExtension(file).Equals(".txt");

    public static bool IsDateFormat(string file)
    {
        var fileName = Path.GetFileNameWithoutExtension(file);
        return fileName.Length == DateLength && fileName.All(IsDigit);
    }
}