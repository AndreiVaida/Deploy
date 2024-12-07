using System.Windows.Input;

namespace Deploy.ui;

public class RelayCommand<T>(Action<T> execute, Func<T, bool>? canExecute = null) : ICommand
{
    private readonly Action<T> _execute = execute ?? throw new ArgumentNullException(nameof(execute));

    public bool CanExecute(object parameter) => canExecute == null || canExecute((T)parameter);

    public void Execute(object parameter) => _execute((T)parameter);

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

}