using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystem.Background_tasks;
using CheckInSystem.Controls;
using CheckInSystem.Platform;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CheckInSystem;

public partial class EmployeeOverviewWindow : Window
{
    private BackgroundTimeService _timeService;
    IPlatform _platform;
    public EmployeeOverviewWindow(ViewModels.Windows.EmployeeOverviewViewModel employeeOverviewViewModel, IPlatform platform)
    { 
        _platform = platform;
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
        _ = Dispatcher.UIThread.Invoke(async () =>
        {
            // Example: Refreshing the view model or adding a message
            // vm.RefreshData();  Add this method in ViewModel to reload data
            var resault = await MessageBox.Show(_platform.MainWindow, "Daily reset has been processed!", "Info", MessageBoxButton.OK);
        });
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}