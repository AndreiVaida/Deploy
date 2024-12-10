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
    private const string ApplicationNameKey = "application-name";
    private const string ApplicationLocationKey = "application-location";
    private const string ServerNameKey = "server-name";

    private readonly FileIniDataParser _parser = new();
    private readonly SystemConfig _systemConfig;

    public ConfigRepositoryImpl()
    {
        _systemConfig = ReadSystemConfig();
    }

    public SystemConfig GetSystemConfig() => _systemConfig;

    private SystemConfig ReadSystemConfig() 
    {
        if (!File.Exists(ConfigurationFileName)) return new SystemConfig();

        var data = _parser.ReadFile(ConfigurationFileName);
        if (!data.Sections.ContainsSection(ApplicationSection)) return new SystemConfig();

        var applicationSection = data.Sections[ApplicationSection];
        var projectsSection = data.Sections[ProjectsSection];

        return new SystemConfig
        {
            ApplicationName = applicationSection[ApplicationNameKey],
            ApplicationLocation = applicationSection[ApplicationLocationKey],
            ServerName = applicationSection[ServerNameKey],
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