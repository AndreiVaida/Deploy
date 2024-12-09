using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using Deploy.service.api;
using Deploy.utils;
using Microsoft.VisualBasic.FileIO;

namespace Deploy.service.impl;

internal class ServerServiceImpl : ServerService
{
    private readonly string _jarLocationInServer = Path.Combine("lib", "extensions");

    public IObservable<Unit> Start(string serverPath)
    {
        return Observable.FromAsync(() =>
        {
            return Task.FromResult(Unit.Default);
        });
    }

    public void Stop(string serverPath)
    {
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
}