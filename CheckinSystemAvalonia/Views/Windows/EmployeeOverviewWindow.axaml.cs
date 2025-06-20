using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using CheckInSystem.Background_tasks;
using CheckInSystemAvalonia.Controls;
using CheckInSystemAvalonia.Platform;
using System.Threading.Tasks;

namespace CheckInSystemAvalonia;

public partial class EmployeeOverviewWindow : Window
{
    private BackgroundTimeService _timeService;

    public EmployeeOverviewWindow(ViewModels.Windows.EmployeeOverviewViewModel employeeOverviewViewModel)
    { 
        InitializeComponent();
        KeyDown += (_, e) =>
        {
            if (e.Key == Key.F11)
                FullScreenHelpers.ToggleFullScreenAvalonia();
        };
        // Start the background time service when the window opens
        StartBackgroundService();
    }

    private async void StartBackgroundService()
    {
        _timeService = new BackgroundTimeService();
        _timeService.OnDailyReset += UpdateUIOnReset; // Subscribe to daily reset event
        _timeService.Start();
    }

    private async void UpdateUIOnReset()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            // Example: Refreshing the view model or adding a message
            // vm.RefreshData();  Add this method in ViewModel to reload data
            //await MessageBox.Show(,"Daily reset has been processed!", "Info", MessageBoxButton.OK);
        });
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}