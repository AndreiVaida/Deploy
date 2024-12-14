using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using Deploy.repository;
using Deploy.service.api;
using Deploy.utils;

namespace Deploy.service.impl;

internal class ServerServiceImpl : ServerService
{
    private const string ServerStartLog = "Server successfully started as a primary.";
    private const string LogsLocationInServer = "logs";
    private readonly string _jarLocationInServer = Path.Combine("lib", "extensions");
    private const string ServerDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    private readonly int _serverDateTimeCharacters = ServerDateTimeFormat.Length;
    private readonly ConfigRepository _configRepository = ServiceProvider.ConfigRepository;

    public IObservable<Unit> Start(string serverPath)
    {
        return Observable.FromAsync(() =>
        {
            var startFile = _configRepository.GetSystemConfig().ServerStartFileRelativeLocation;
            ExecUtils.RunCommand(serverPath, startFile, false);

            WaitForLog(serverPath, GetServerStartLogPredicate());
            return Task.FromResult(Unit.Default);
        });
    }

    public void Stop(string serverPath)
    {
        // WARNING: The stop_server.bat command stops the server slowly (17 seconds).
        // If the jar building finishes before the server is closed, we must change the way we close the server.
        // Solution: press `Ctrl + C` on the server's window
        var stopFile = _configRepository.GetSystemConfig().ServerStopFileRelativeLocation;
        ExecUtils.RunCommand(serverPath, stopFile);
    }

    public void UpdateJar(string serverPath, string jarPath)
    {
        DeleteJar(serverPath, jarPath);
        var jarFolderPath = Path.Combine(serverPath, _jarLocationInServer);
        FileUtils.CopyFile(jarFolderPath, jarPath);
    }

    private void DeleteJar(string serverPath, string jarPath)
    {
        var jarName = FileUtils.GetJarName(jarPath);
        var jarsFolderOnServer = Path.Combine(serverPath, _jarLocationInServer);
        var serverJarsWithSameName = GetJarsWithSameName(jarsFolderOnServer, jarName);
        serverJarsWithSameName.ForEach(FileUtils.DeleteFile);
    }

    private static List<string> GetJarsWithSameName(string jarsFolderOnServer, string name) =>
        Directory.GetFiles(jarsFolderOnServer)
            .OrderByDescending(File.GetCreationTime)
            .Where(FileUtils.IsJar)
            .Where(serverJar => name.Equals(FileUtils.GetJarName(serverJar)))
            .ToList();

    private Predicate<string> GetServerStartLogPredicate()
    {
        var serverStartTimeAsString = DateTime.Now.ToString(ServerDateTimeFormat);
        return log => log.EndsWith(ServerStartLog) && GetTimeOfLog(log).IsGreaterThan(serverStartTimeAsString);
    }

    private string GetTimeOfLog(string log) => log[.._serverDateTimeCharacters];

    private static void WaitForLog(string serverPath, Predicate<string> condition)
    {
        var logFile = GetLatestLogFile(Path.Combine(serverPath, LogsLocationInServer));

        using var fileStream = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream);
        while (true)
        {
            while (streamReader.ReadLine() is { } line)
                if (condition.Invoke(line))
                    return;
            Thread.Sleep(500);
        }
    }

    private static string GetLatestLogFile(string logsFolder) =>
        Directory.GetFiles(logsFolder)
            .OrderByDescending(File.GetCreationTime)
            .Where(FileUtils.IsTxt)
            .Where(FileUtils.IsDateFormat)
            .First();
}