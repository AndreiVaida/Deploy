using Deploy.logger;
using Deploy.repository;
using Deploy.service.api;
using System.Diagnostics;
using System.IO;

namespace Deploy.service.impl;

public class ApplicationServiceImpl : ApplicationService
{
    private readonly ConfigRepository _configRepository = ServiceProvider.ConfigRepository;
    private readonly WindowService _windowService = ServiceProvider.WindowService;
    private readonly Logger _logger = new LoggerImpl(nameof(ApplicationServiceImpl));

    public void Start()
    {
        var executableFolder = Path.GetDirectoryName(_configRepository.GetSystemConfig().ApplicationLocation);
        var executable = _configRepository.GetSystemConfig().ApplicationLocation;
        var args = _configRepository.GetSystemConfig().ApplicationArguments;

        _logger.Info($"Start application {executable} {args} in {executableFolder}");

        var startInfo = new ProcessStartInfo
        {
            WorkingDirectory = executableFolder,
            FileName = executable,
            Arguments = args
        };

        using var process = Process.Start(startInfo);

        _logger.Info($"Application started: {process != null}");
    }

    public void Stop()
    {
        var applicationName = _configRepository.GetSystemConfig().ApplicationProcessName;
        _logger.Info($"Stop application {applicationName}");
        _windowService.KillProcess(applicationName);
    }
}