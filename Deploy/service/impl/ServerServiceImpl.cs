using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using Deploy.repository;
using Deploy.service.api;
using Deploy.utils;
using Microsoft.VisualBasic.FileIO;

namespace Deploy.service.impl;

internal class ServerServiceImpl : ServerService
{
    private const string ServerStartLog = "Server successfully started as a primary.";
    private readonly string _jarLocationInServer = Path.Combine("lib", "extensions");
    private readonly string _logsLocationInServer = "logs";
    private const string ServerDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    private readonly int ServerDateTimeCharacters = ServerDateTimeFormat.Length;
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
        CopyJar(serverPath, jarPath);
    }

    private void DeleteJar(string serverPath, string jarPath)
    {
        var jarName = GetJarName(jarPath);
        var jarsFolderOnServer = Path.Combine(serverPath, _jarLocationInServer);
        var serverJarsWithSameName = GetJarsWithSameName(jarsFolderOnServer, jarName);
        serverJarsWithSameName.ForEach(DeleteFile);
    }

    private static string GetJarName(string jarPath) => string.Join('-', Path.GetFileName(jarPath).Split('-').Where(IsWord));

    private static bool IsWord(string str) => str.All(char.IsLetter);

    private static List<string> GetJarsWithSameName(string jarsFolderOnServer, string name) =>
        Directory.GetFiles(jarsFolderOnServer)
            .OrderByDescending(File.GetCreationTime)
            .Where(FileUtils.IsJar)
            .Where(serverJar => name.Equals(GetJarName(serverJar)))
            .ToList();


    private static void DeleteFile(string filePath) =>
        FileSystem.DeleteFile(filePath,
            UIOption.OnlyErrorDialogs,
            RecycleOption.SendToRecycleBin,
            UICancelOption.ThrowException);

    private void CopyJar(string serverPath, string jarPath)
    {
        var jarName = Path.GetFileName(jarPath);
        var destinationFilePath = Path.Combine(serverPath, _jarLocationInServer, jarName);
        File.Move(jarPath, destinationFilePath);
    }

    private Predicate<string> GetServerStartLogPredicate()
    {
        var serverStartTimeAsString = DateTime.Now.ToString(ServerDateTimeFormat);
        return log => log.EndsWith(ServerStartLog) && GetTimeOfLog(log).IsGreaterThan(serverStartTimeAsString);
    }

    private string GetTimeOfLog(string log) => log[..ServerDateTimeCharacters];

    private void WaitForLog(string serverPath, Predicate<string> condition)
    {
        var logFile = GetLatestLogFile(serverPath);

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

    private string GetLatestLogFile(string serverPath)
    {
        var logsFolder = Path.Combine(serverPath, _logsLocationInServer);
        return Directory.GetFiles(logsFolder)
            .OrderByDescending(File.GetCreationTime)
            .Where(FileUtils.IsTxt)
            .Where(FileUtils.IsDateFormat)
            .First();
    }
}