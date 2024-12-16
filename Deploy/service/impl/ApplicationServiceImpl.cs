using System.Diagnostics;
using System.IO;
using Deploy.repository;
using Deploy.service.api;

namespace Deploy.service.impl;

public class ApplicationServiceImpl : ApplicationService
{
    private readonly ConfigRepository _configRepository = ServiceProvider.ConfigRepository;
    private readonly WindowService _windowService = ServiceProvider.WindowService;

    public void Start()
    {
        var executableFolder = Path.GetDirectoryName(_configRepository.GetSystemConfig().ApplicationLocation);
        var executable = _configRepository.GetSystemConfig().ApplicationLocation;
        var args = _configRepository.GetSystemConfig().ApplicationArguments;

        var startInfo = new ProcessStartInfo
        {
            WorkingDirectory = executableFolder,
            FileName = executable,
            Arguments = args
        };

        using var process = Process.Start(startInfo);
    }

    public void Stop()
    {
        _windowService.KillProcess(_configRepository.GetSystemConfig().ApplicationProcessName);
    }
}