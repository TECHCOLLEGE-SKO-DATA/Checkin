using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace CheckInSystemAvalonia;

public partial class EmployeeOverviewWindow : Window
{
    private BackgroundTimeService _timeService;
    public EmployeeOverviewWindow(ViewModels.Windows.EmployeeOverviewViewModel employeeOverviewViewModel)
    { 
        InitializeComponent();
        /*KeyDown += (_, e) =>
        {
             if (e.Key == Key.F11
                FullScreenHelpers.ToggleFullScreenWpf()=;
        };*/
        // Start the background time service when the window opens
        StartBackgroundService();
    }

    private void StartBackgroundService()
    {
        _timeService = new BackgroundTimeService();
        _timeService.OnDailyReset += UpdateUIOnReset; // Subscribe to daily reset event
        _timeService.Start();
    }

    private void UpdateUIOnReset()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            // Example: Refreshing the view model or adding a message
            // vm.RefreshData();  Add this method in ViewModel to reload data
            //MessageBox.Show("Daily reset has been processed!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        });
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}