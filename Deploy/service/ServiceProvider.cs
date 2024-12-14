using Deploy.repository;
using Deploy.repository.impl;
using Deploy.service.api;
using Deploy.service.impl;

namespace Deploy.service;

public static class ServiceProvider
{
    // Repository
    public static ConfigRepository ConfigRepository { get; } = new ConfigRepositoryImpl();

    // Independent Service
    public static WindowService WindowService { get; } = new WindowServiceImpl();
    public static ProjectService ProjectService { get; } = new ProjectServiceImpl();
    public static ServerService ServerService { get; } = new ServerServiceImpl();
    public static ApplicationService ApplicationService { get; } = new ApplicationServiceImpl();

    // Global Service
    public static DeployService DeployService { get; } = new DeployServiceImpl();
}