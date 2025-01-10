using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using Deploy.repository;
using Deploy.service.api;
using Deploy.utils;
using Microsoft.VisualBasic.FileIO;

namespace Deploy.service.impl;

public class ProjectServiceImpl : ProjectService
{
    private const string GradleBuildCommand = "gradlew assemble --parallel";
    private readonly string _jarRelativePath = Path.Combine("build", "distributions");
    private readonly ConfigRepository _configRepository = ServiceProvider.ConfigRepository;

    public IObservable<string> Build(string projectPath)
    {
        return Observable.FromAsync(() =>
        {
            DeleteServerCache(projectPath);
            BuildProject(projectPath);

            var jarFolderPath = Path.Combine(projectPath, _jarRelativePath);
            var jarName = GetJar(jarFolderPath);

            return jarName == null
                ? Task.FromException<string>(new NullReferenceException($"Jar not available in {jarFolderPath}"))
                : Task.FromResult(Path.Combine(jarFolderPath, jarName));
        });
    }

    private void DeleteServerCache(string projectPath)
    {
        if (string.IsNullOrEmpty(_configRepository.GetSystemConfig().ProjectCacheFolderToDelete)) return;

        var folderToDelete = Path.Combine(projectPath, _configRepository.GetSystemConfig().ProjectCacheFolderToDelete!);
        FileSystem.DeleteDirectory(folderToDelete, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
    }

    private static void BuildProject(string projectPath) => ExecUtils.RunCommand(projectPath, GradleBuildCommand);

    private static string? GetJar(string jarFolderPath)
    {
        if (!Directory.Exists(jarFolderPath)) return null;
        return Directory.GetFiles(jarFolderPath)
            .OrderByDescending(File.GetCreationTime)
            .FirstOrDefault(FileUtils.IsJar);
    }
}