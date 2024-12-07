using Deploy.service.api;
using Deploy.service.impl;

namespace Deploy.service;

public static class ServiceProvider
{
    public static ProjectService ProjectService { get; } = new ProjectServiceImpl();
    public static ServerService ServerService { get; } = new ServerServiceImpl();
    public static ApplicationService ApplicationService { get; } = new ApplicationServiceImpl();
    public static DeployService DeployService { get; } = new DeployServiceImpl();
}