using System.IO;

namespace Deploy.utils;

public static class FileUtils
{
    public static bool IsJar(string file) => Path.GetExtension(file).Equals(".jar");
}