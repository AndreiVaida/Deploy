namespace Deploy.model;

public class Project(string name, string projectPath, string serverPath)
{
    public string Name { get; set; } = name;
    public string ProjectPath { get; set; } = projectPath;
    public string ServerPath { get; set; } = serverPath;

    public override string ToString() => $"Project{{Name='{Name}', ProjectPath='{ProjectPath}', ServerPath='{ServerPath}'}}";
}