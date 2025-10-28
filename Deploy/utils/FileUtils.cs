using Microsoft.VisualBasic.FileIO;
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

    public static bool IsTodayDate(string fileName)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var todayDateAsString = DateTime.Now.ToString("yyyyMMdd");
        return fileNameWithoutExtension.Equals(todayDateAsString);
    }

    public static void CopyFile(string destinationFolder, string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        var destinationFilePath = Path.Combine(destinationFolder, fileName);
        File.Move(filePath, destinationFilePath);
    }

    public static void DeleteFile(string filePath) =>
        FileSystem.DeleteFile(filePath,
            UIOption.OnlyErrorDialogs,
            RecycleOption.SendToRecycleBin,
            UICancelOption.ThrowException);

    public static string GetJarName(string jarPath) => string.Join('-', Path.GetFileName(jarPath).Split('-').Where(IsWord));

    private static bool IsWord(string str) => str.All(IsLetter);
}