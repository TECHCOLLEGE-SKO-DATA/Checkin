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
using CheckInSystem.ViewModels.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CheckInSystem;

public partial class EmployeeOverviewWindow : Window
{
    private BackgroundTimeService _timeService;
    IPlatform _platform;
    private readonly EmployeeOverviewViewModel _vm;

    public EmployeeOverviewWindow(EmployeeOverviewViewModel employeeOverviewViewModel, IPlatform platform)
    {
        _vm = employeeOverviewViewModel;
        _platform = platform;

        DataContext = _vm;
        InitializeComponent();

        KeyDown += (_, e) =>
        {
            if (e.Key == Key.F11)
                FullScreenHelpers.ToggleFullScreenAvalonia();
        };

        StartBackgroundService();
    }


    private void StartBackgroundService()
    {
        _timeService = new BackgroundTimeService();

        _timeService.GetEmployees = () =>
            _vm.GetAllEmployees();

        _timeService.PerformMaintenanceAction = employees =>
        {
            Maintenance.CheckOutEmployeesIfTheyForgot(employees);
            Maintenance.CheckForEndedOffSiteTime(employees);
        };

        _timeService.OnDailyReset += UpdateUIOnReset;

        _timeService.Start(_vm);
    }


    private void UpdateUIOnReset()
    {
        Dispatcher.UIThread.Post(() =>
        {
            _vm.SortEmployees();
            _ = MessageBox.Show(
                _platform.MainWindow,
                "Daily reset has been processed!",
                "Info",
                MessageBoxButton.OK);
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}