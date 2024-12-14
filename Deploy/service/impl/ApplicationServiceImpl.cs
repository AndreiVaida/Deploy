﻿using System.Diagnostics;
using Deploy.repository;
using Deploy.service.api;

namespace Deploy.service.impl;

public class ApplicationServiceImpl : ApplicationService
{
    private readonly ConfigRepository _configRepository = ServiceProvider.ConfigRepository;

    public void Start()
    {
        var executable = _configRepository.GetSystemConfig().ApplicationLocation;
        var args = _configRepository.GetSystemConfig().ApplicationArguments;

        var startInfo = new ProcessStartInfo
        {
            FileName = executable,
            Arguments = args
        };

        using var process = Process.Start(startInfo);
    }

    public void Stop()
    {
    }
}