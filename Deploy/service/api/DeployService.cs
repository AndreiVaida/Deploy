using Deploy.model;

namespace Deploy.service.api;

public interface DeployService
{
    void Deploy(Project project);
    void Restart(Project project);
}