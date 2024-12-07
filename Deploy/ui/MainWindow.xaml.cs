using System.Windows;
using Deploy.service;
using Deploy.service.api;
using Deploy.ui;

namespace Deploy;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DeployService _deployService = ServiceProvider.DeployService;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new ProjectViewModel();
    }

    public void OnOpenProjectLocation(object sender, RoutedEventArgs e)
    {
        // TODO
    }

    public void OnOpenPlatformLocation(object sender, RoutedEventArgs e)
    {
        // TODO
    }
}