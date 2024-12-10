using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Deploy.model;
using Deploy.repository;
using Deploy.service;
using Deploy.service.api;

namespace Deploy.ui;

public class ProjectViewModel : INotifyPropertyChanged
{
    private static readonly DeployService DeployService = ServiceProvider.DeployService;
    private static readonly ConfigRepository ConfigRepository = ServiceProvider.ConfigRepository;

    public ICommand ExecuteBuildCommand { get; set; } = new RelayCommand<Project>(ExecuteBuild);

    public ObservableCollection<Project> Projects { get; set; } = new(ConfigRepository.GetSystemConfig().Projects ?? []);

    public static void ExecuteBuild(Project project) => DeployService.Deploy(project);

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}