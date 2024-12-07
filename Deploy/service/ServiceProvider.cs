namespace Deploy.service;

public static class ServiceProvider
{
    public static DeployService DeployService { get; } = new DeployServiceImpl();
}