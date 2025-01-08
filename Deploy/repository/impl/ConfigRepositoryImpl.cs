using System.Configuration;
using System.Diagnostics;
using IniParser;
using System.IO;
using Deploy.model;
using IniParser.Model;

namespace Deploy.repository.impl;

public class ConfigRepositoryImpl : ConfigRepository
{
    private const string ConfigurationFileName = "Configuration.ini";
    private const string ApplicationSection = "application";
    private const string ProjectsSection = "projects";
    private const string ApplicationProcessNameKey = "application-process-name";
    private const string ApplicationLocationKey = "application-location";
    private const string ApplicationArgumentsKey = "application-arguments";
    private const string ServerNameKey = "server-name";
    private const string ServerWindowNameKey = "server-window-name";
    private const string ServerStartFileRelativeLocationKey = "server-start-file-relative-location";
    private const string ServerStartLogKey = "server-start-log";

    private readonly FileIniDataParser _parser = new();
    private readonly SystemConfig _systemConfig;

    public ConfigRepositoryImpl()
    {
        _systemConfig = ReadSystemConfig();
    }

    public SystemConfig GetSystemConfig() => _systemConfig;

    public void OpenConfigurationFile() => Process.Start("explorer.exe", ConfigurationFileName);

    private SystemConfig ReadSystemConfig() 
    {
        if (!File.Exists(ConfigurationFileName)) throw new ConfigurationErrorsException($"{ConfigurationFileName} not found. Please create as described on https://github.com/AndreiVaida/Deploy.");

        var data = _parser.ReadFile(ConfigurationFileName);
        if (!data.Sections.ContainsSection(ApplicationSection)) throw new ConfigurationErrorsException($"'{ApplicationSection}' not found in {ConfigurationFileName}.");

        var applicationSection = data.Sections[ApplicationSection];
        var projectsSection = data.Sections[ProjectsSection];

        return new SystemConfig
        {
            ApplicationProcessName = applicationSection[ApplicationProcessNameKey],
            ApplicationLocation = applicationSection[ApplicationLocationKey],
            ApplicationArguments = applicationSection[ApplicationArgumentsKey],
            ServerName = applicationSection[ServerNameKey],
            ServerWindowName = applicationSection[ServerWindowNameKey],
            ServerStartFileRelativeLocation = applicationSection[ServerStartFileRelativeLocationKey],
            ServerStartLog = applicationSection[ServerStartLogKey],
            Projects = GetProjects(projectsSection)
        };
    }

    private static List<Project> GetProjects(KeyDataCollection? projectsSection)
    {
        var projects = new List<Project>();
        if (projectsSection == null) return projects;

        foreach (var keyData in projectsSection)
        {
            var values = keyData.Value.Split(',');
            projects.Add(new Project(values[0], values[1], values[2]));
        }
        return projects;
    }
}