using Deploy.model;
using Deploy.service.api;

namespace Deploy.service.impl;

public class DeployServiceImpl : DeployService
{
    private readonly ProjectService _projectService = ServiceProvider.ProjectService;
    private readonly ServerService _serverService = ServiceProvider.ServerService;
    private readonly ApplicationService _applicationService = ServiceProvider.ApplicationService;

    public void Deploy(Project project)
    {
        _projectService.Build(project.ProjectPath).Subscribe(jarPath =>
        {
            _serverService.UpdateJar(project.ProjectPath, jarPath);
            _serverService.Start(project.ProjectPath)
                .Subscribe(_ => _applicationService.Start());
        });

        _applicationService.Stop();
        _serverService.Stop(project.DeployPath);
    }
}