namespace Deploy.ui;

public class DialogService
{
    public static void ShowErrorMessage(string message) {
        System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
        {
            var mainWindow = System.Windows.Application.Current.MainWindow!;
            System.Windows.MessageBox.Show(mainWindow, message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        });
    }
}