using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Deploy.model;
using Deploy.service;
using Deploy.service.api;

namespace Deploy.ui;

public class ProjectViewModel : INotifyPropertyChanged
{
    private static readonly DeployService DeployService = ServiceProvider.DeployService;

    public ICommand ExecuteBuildCommand { get; set; } = new RelayCommand<Project>(ExecuteBuild);

    public ObservableCollection<Project> Projects { get; set; } = [
        new("Project 1", @"C:\Projects\p1", @"C:\Projects\Platforms\p1\server"),
        new("Project 2", @"C:\Projects\p2", @"C:\Projects\Platforms\p2\server"),
        new("Project 3", @"C:\Projects\p3", @"C:\Projects\Platforms\p3\server"),
    ];

    public static void ExecuteBuild(Project project) => DeployService.Deploy(project);

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}