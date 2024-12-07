namespace Deploy.model;

public class Project(string name, string projectPath, string deployPath)
{
    public string Name { get; set; } = name;
    public string ProjectPath { get; set; } = projectPath;
    public string DeployPath { get; set; } = deployPath;
}