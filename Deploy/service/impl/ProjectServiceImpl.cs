using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using Deploy.service.api;
using Deploy.utils;

namespace Deploy.service.impl;

public class ProjectServiceImpl : ProjectService
{
    private const string GradleBuildCommand = "gradlew assemble --parallel"; // TODO: add option on UI to also run `clean`, and delete the folder `server\extension\build`
    private readonly string _jarRelativePath = Path.Combine("build", "distributions");

    public IObservable<string> Build(string projectPath)
    {
        return Observable.FromAsync(() =>
        {
            ExecUtils.RunCommand(projectPath, GradleBuildCommand);

            var jarFolderPath = Path.Combine(projectPath, _jarRelativePath);
            var jarName = GetJar(jarFolderPath);

            return jarName == null
                ? Task.FromException<string>(new NullReferenceException($"Jar not available in {jarFolderPath}"))
                : Task.FromResult(Path.Combine(jarFolderPath, jarName));
        });
    }

    private static string? GetJar(string jarFolderPath)
    {
        if (!Directory.Exists(jarFolderPath)) return null;
        return Directory.GetFiles(jarFolderPath)
            .OrderByDescending(File.GetCreationTime)
            .FirstOrDefault(FileUtils.IsJar);
    }
}