using System.Diagnostics;
using System.Windows;
using Deploy.repository;
using Deploy.service;
using Deploy.service.api;

namespace Deploy.ui;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ConfigRepository _configRepository = ServiceProvider.ConfigRepository;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new ProjectViewModel();
    }

    public void OnOpenProjectLocation(object sender, RoutedEventArgs e) => OpenConfigurationFile();

    public void OnOpenPlatformLocation(object sender, RoutedEventArgs e) => OpenConfigurationFile();

    private void OpenConfigurationFile() => _configRepository.OpenConfigurationFile();
}