using System.IO;
using System.Reactive.Linq;
using Deploy.logger;
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
    private readonly Logger _logger = new LoggerImpl(nameof(ProjectServiceImpl));

    public IObservable<string> Build(string projectPath)
    {
        return Observable.FromAsync(() =>
        {
            _logger.Info($"Build project {projectPath}");

            DeleteServerCache(projectPath);
            BuildProject(projectPath);

            var jarFolderPath = Path.Combine(projectPath, _jarRelativePath);
            _logger.Info($"Build done. Retrieve jar from {jarFolderPath}");
            var jarName = GetJar(jarFolderPath);

            return jarName == null
                ? Task.FromException<string>(new NullReferenceException($"Jar not available in {jarFolderPath}"))
                : Task.FromResult(Path.Combine(jarFolderPath, jarName));
        });
    }

    private void DeleteServerCache(string projectPath)
    {
        var projectCacheFolder = _configRepository.GetSystemConfig().ProjectCacheFolderToDelete;
        if (string.IsNullOrEmpty(projectCacheFolder)) return;

        var folderToDelete = Path.Combine(projectPath, projectCacheFolder);
        if (FileSystem.DirectoryExists(folderToDelete))
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