using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using Deploy.service.api;

namespace Deploy.service.impl;

public class ProjectServiceImpl : ProjectService
{
    private const string GradleBuildCommand = "gradlew assemble --parallel"; // TODO: add option on UI to also run `clean`, and delete the folder `server\extension\build`
    private readonly string _jarRelativePath = Path.Combine("build", "distributions");

    public IObservable<string> Build(string projectPath)
    {
        return Observable.FromAsync(() =>
        {
            var process = CreateCmdProcess(projectPath);
            process.Start();
            process.WaitForExit();

            var jarFolderPath = Path.Combine(projectPath, _jarRelativePath);
            var jarName = GetJar(jarFolderPath);

            return jarName == null
                ? Task.FromException<string>(new NullReferenceException($"Jar not available in {jarFolderPath}"))
                : Task.FromResult(Path.Combine(jarFolderPath, jarName));
        });
    }

    private static Process CreateCmdProcess(string projectPath)
    {
        var process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = $"/C cd {projectPath} && {GradleBuildCommand}";
        process.StartInfo.UseShellExecute = false;
        return process;
    }

    private static string? GetJar(string jarFolderPath)
    {
        if (!Directory.Exists(jarFolderPath)) return null;
        return Directory.GetFiles(jarFolderPath)
            .OrderByDescending(File.GetCreationTime)
            .FirstOrDefault(IsJar);
    }

    private static bool IsJar(string file) => Path.GetExtension(file).Equals(".jar");
}