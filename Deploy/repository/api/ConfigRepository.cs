using Deploy.model;

namespace Deploy.repository;

public interface ConfigRepository
{
    public SystemConfig GetSystemConfig();
    void OpenConfigurationFile();
}