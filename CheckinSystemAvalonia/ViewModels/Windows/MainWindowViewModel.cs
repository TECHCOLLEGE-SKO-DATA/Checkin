using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CheckinLib.Database;
using CheckinLib.Models;
using CheckinLib.Platform;
using CheckinLib.ViewModels.Windows;
using CheckinSystemAvalonia.ViewModels.UserControls;
using CheckinSystemAvalonia.Views.UserControls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckinSystemAvalonia.ViewModels.Windows;
public class MainWindowViewModel : ViewModelBase
{
    static private IPlatform _platform;

    private CheckinLib.ViewModels.Windows.MainWindowViewModel _coreViewModel;

    public ObservableCollection<Employee> Employees => _coreViewModel.Employees;
    public ObservableCollection<Group> Groups => _coreViewModel.Groups;

    public MainWindowViewModel(IPlatform platform) : base(platform)
    {
        _platform = platform;

        // Instantiate the original ViewModel from CheckinLib
        _coreViewModel = new CheckinLib.ViewModels.Windows.MainWindowViewModel(platform);
        _coreViewModel.LoadDataFromDatabase();

        CurrentViewModel = new AdminLoginViewModel(platform, this);
    }

    private ViewModelBase _currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public void SwitchToGroupView()
    {
        CurrentViewModel = new AdminGroupViewModel(_platform);
    }

    public void SwitchToSettingsView()
    {
        CurrentViewModel = new SettingsViewModel(_platform);
    }

    public void SwitchToLoginView()
    {
        CurrentViewModel = new AdminLoginViewModel(_platform, this);
    }

    // Optional: expose methods from the core ViewModel directly
    public void SeeEmployeeTime(Employee employee)
    {
        _coreViewModel.SeeEmployeeTime(employee);
    }

    public void EmployeeCardScanned(string cardId)
    {
        _coreViewModel.EmployeeCardScanned(cardId);
    }
}