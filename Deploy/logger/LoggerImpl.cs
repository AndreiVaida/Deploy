using System.IO;

namespace Deploy.logger;

public class LoggerImpl : Logger
{
    private readonly string _logFilePath;
    private readonly string _className;
    private static readonly object FileLock = new();

    public LoggerImpl(string className)
    {
        var logFileName = $"{DateTime.Now:yyyyMMdd}.txt";
        _logFilePath = Path.Combine("logs", logFileName);
        _className = className;
    }

    public void Info(string message) => Log("INFO", message);

    public void Warning(string message) => Log("WARNING", message);

    public void Error(string message, Exception exception)
    {
        var logLine = $"{message} | Exception: {exception.Message} | StackTrace: {exception.StackTrace}";
        Log("ERROR", logLine);
    }

    private void Log(string level, string message)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath)!);

        var logEntry = $"{DateTime.Now:yyyy.MM.dd HH:mm:ss.fff} ({level}) [{_className}] {message}";
        lock (FileLock)
        {
            using var writer = new StreamWriter(_logFilePath, append: true);
            writer.WriteLine(logEntry);
        }
    }
}