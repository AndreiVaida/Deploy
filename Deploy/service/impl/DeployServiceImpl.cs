using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Deploy.logger;
using Deploy.model;
using Deploy.service.api;
using Deploy.ui;

namespace Deploy.service.impl;

public class DeployServiceImpl : DeployService
{
    private readonly ProjectService _projectService = ServiceProvider.ProjectService;
    private readonly ServerService _serverService = ServiceProvider.ServerService;
    private readonly ApplicationService _applicationService = ServiceProvider.ApplicationService;
    private readonly Logger _logger = new LoggerImpl(nameof(DeployServiceImpl));

    public void Restart(Project project)
    {
        _logger.Info($"► Restart {project}");

        try
        {
            _applicationService.Stop();
            _serverService.Stop(project.ServerPath);
            _serverService.Start(project.ServerPath)
                .SubscribeOn(Scheduler.Default)
                .Subscribe(_ =>
                    {
                        try
                        {
                            _applicationService.Start();
                        }
                        catch (Exception e)
                        {
                            OnError("Restart: cannot start application", e);
                        }
                    },
                    exception => OnError("Restart: cannot start server", exception));
        }
        catch (Exception e)
        {
            OnError($"Cannot restart {project}", e);
        }
    }

    public void Deploy(Project project)
    {
        _logger.Info($"► Deploy {project}");

        try
        {
            _projectService.Build(project.ProjectPath)
                .SubscribeOn(Scheduler.Default)
                .Subscribe(jarPath =>
                {
                    _serverService.UpdateJar(project.ServerPath, jarPath);
                    _serverService.Start(project.ServerPath)
                        .Subscribe(_ =>
                            {
                                try
                                {
                                    _applicationService.Start();
                                }
                                catch (Exception e)
                                {
                                    OnError("Restart: cannot start application", e);
                                }
                            },
                            exception => OnError("Deploy: cannot start server", exception));
                },
                exception => OnError("Deploy: cannot build project", exception));

            _applicationService.Stop();
            _serverService.Stop(project.ServerPath);
        }
        catch (Exception e)
        {
            OnError($"Cannot deploy {project}", e);
        }
    }

    private void OnError(string message, Exception e)
    {
        _logger.Error(message, e);
        DialogService.ShowErrorMessage(message);
    }
}