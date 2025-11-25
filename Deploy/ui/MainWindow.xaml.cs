using System.Windows;
using Deploy.logger;
using Deploy.repository;
using Deploy.service;

namespace Deploy.ui;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ConfigRepository _configRepository = ServiceProvider.ConfigRepository;
    private readonly Logger _logger = new LoggerImpl(nameof(MainWindow));

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new ProjectViewModel();
        _logger.Info("►►► Deploy started ◄◄◄");
    }

    public void OnOpenProjectLocation(object sender, RoutedEventArgs e) => OpenConfigurationFile();

    public void OnOpenPlatformLocation(object sender, RoutedEventArgs e) => OpenConfigurationFile();

    private void OpenConfigurationFile() => _configRepository.OpenConfigurationFile();
}