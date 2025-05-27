using CheckInSystem.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

public static class UserActivityWatcher
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(40);
    private static DispatcherTimer _timer;
    private static Func<int> _getSelectedTabIndex;
    private static Func<Window> _getMainWindow;
    private static int _loginTabIndex = 0; // Default to 0, change if needed

    private static bool _isInitialized;

    public static void Initialize(Func<int> getSelectedTabIndex, Func<Window> getMainWindow, int loginTabIndex = 0)
    {
        if (_isInitialized) return;
        _isInitialized = true;

        _getSelectedTabIndex = getSelectedTabIndex;
        _getMainWindow = getMainWindow;
        _loginTabIndex = loginTabIndex;

        _timer = new DispatcherTimer { Interval = Timeout };
        _timer.Tick += OnTimeout;

        InputManager.Current.PreProcessInput += OnInput;
        _timer.Start();
    }

    private static void OnInput(object sender, PreProcessInputEventArgs e)
    {
        if (e.StagingItem.Input is MouseEventArgs || e.StagingItem.Input is KeyboardEventArgs)
        {
            if (!IsLoginTabActive())
            {
                _timer.Stop();
                _timer.Start(); // Reset the timer
            }
        }
    }

    private static void OnTimeout(object sender, EventArgs e)
    {
        _timer.Stop();

        if (!IsLoginTabActive())
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = _getMainWindow();
                if (mainWindow?.DataContext is MainWindowViewModel vm)
                {
                    vm.SelectedTab = _loginTabIndex; // Go back to login screen
                }
            });
        }
    }

    private static bool IsLoginTabActive() => _getSelectedTabIndex() == _loginTabIndex;
}
