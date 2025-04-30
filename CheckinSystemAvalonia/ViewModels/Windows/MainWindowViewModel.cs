
using Avalonia.Controls;
using CheckinLib.Database;
using CheckinLib.Models;
using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels.Windows;
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
using CheckinLib.CardReader;
using CheckinLib.Background_tasks;
using System.Windows;

namespace CheckinSystemAvalonia.ViewModels.Windows;
public class MainWindowViewModel : ViewModelBase
{

    AdminPanelViewModel _adminPanelViewModel;
    public AdminPanelViewModel AdminPanelViewModel
    {
        get => _adminPanelViewModel;
        set => this.RaiseAndSetIfChanged(ref _adminPanelViewModel, value, nameof(AdminPanelViewModel));
    }
    AdminGroupViewModel _adminGroupViewModel;
    public AdminGroupViewModel AdminGroupViewModel
    {
        get => _adminGroupViewModel;
        set => this.RaiseAndSetIfChanged(ref _adminGroupViewModel, value, nameof(AdminGroupViewModel));
    }

    AdminLoginViewModel _loginViewModel;
    public AdminLoginViewModel LoginScreenViewModel
    {
        get => _loginViewModel;
        set => this.RaiseAndSetIfChanged(ref _loginViewModel, value, nameof(LoginScreenViewModel));
    }
    EmployeeTimeViewModel _employeeTimeViewModel;
    public EmployeeTimeViewModel EmployeeTimeViewModel
    {
        get => _employeeTimeViewModel;
        set
        {
            // if (_employeeTimeView != null)
            // {
            //     _employeeTimeView.DataContext = value;
            // }
            this.RaiseAndSetIfChanged(ref _employeeTimeViewModel, value, nameof(EmployeeTimeViewModel));
        }
    }

    public ObservableCollection<Employee> Employees { get; private set; } = new();
    public ObservableCollection<Group> Groups { get; private set; } = new();

    int _selectedTab = 0;
    public int SelectedTab
    {
        get => _selectedTab;
        set => this.RaiseAndSetIfChanged(ref _selectedTab, value, nameof(SelectedTab));
    }

    ContentControl _mainContentControl;
    public ContentControl MainContentControl
    {
        get => _mainContentControl;
        set => this.RaiseAndSetIfChanged(ref _mainContentControl, value, nameof(MainContentControl));
    }

    public MainWindowViewModel(IPlatform platform) : base(platform)
    {
        platform.CardReader.CardScanned += (sender, args) => EmployeeCardScanned(args.CardId);
        
        LoginScreenViewModel = new(platform, this);
        AdminPanelViewModel = new(platform, this);
        AdminGroupViewModel = new(platform, this);
        EmployeeTimeViewModel = new(platform);

        CurrentViewModel = LoginScreenViewModel;
    }

    public void LoadDataFromDatabase()
    {
        AbsencBackGroundService absence = new();
        DatabaseHelper databaseHelper = new DatabaseHelper();
        foreach (var employee in databaseHelper.GetAllEmployees())
        {
            absence.AddEmployeesToAbsenceCheck(employee);

            absence.AbsenceTask();

            Employees.Add(employee);
        }
        Groups = new ObservableCollection<Group>(Group.GetAllGroups(new List<Employee>(Employees)));

        List<Employee> employees = new List<Employee>(Employees);

        Maintenance.CheckOutEmployeesIfTheyForgot(employees);
        Maintenance.CheckForEndedOffSiteTime(employees);
    }

    public void RequestView(UserControl control)
    {
        MainContentControl = control;
    }

    public void EmployeeCardScanned(string cardID)
    {
        if (State.UpdateNextEmployee)
        {
            UpdateNextEmployee(cardID);
            return;
        }

        if (State.UpdateCardId)
        {
            UpdateCardId(cardID);
            return;
        }

        UpdateEmployeeLocal(cardID);
    }

    void UpdateNextEmployee(string cardID)
    {
        DatabaseHelper databaseHelper = new();
        State.UpdateNextEmployee = false;
        Employee? editEmployee = Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        if (editEmployee == null)
        {
            databaseHelper.CardScanned(cardID);
            editEmployee = databaseHelper.GetFromCardId(cardID);
            if (editEmployee == null)
            {
                throw new Exception("Failed saving employee");
            }
            Employees.Add(editEmployee);
        }
        /*
        if (Views.Dialog.WaitingForCardDialog.Instance != null)
            Application.Current.Dispatcher.Invoke(() => {
                Views.Dialog.WaitingForCardDialog.Instance.Close();
            });

        EditEmployeeWindow.Open(editEmployee);*/
    }

    void UpdateEmployeeLocal(string cardID)
    {
        DatabaseHelper databaseHelper = new();
        Employee? employee = Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        if (employee != null)
        {
            databaseHelper.CardScanned(cardID); //Update DB
            employee.CardScanned(cardID); //Update UI
        }
        else
        {
            var dbEmployee = databaseHelper.GetFromCardId(cardID);
            if (dbEmployee != null)
            {
                Application.Current.Dispatcher.Invoke(() => {
                    Employees.Add(dbEmployee);
                });
            }
        }
    }

    private void UpdateCardId(string cardID)
    {
        State.UpdateCard(cardID);
    }

    private ViewModelBase _currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    public void SwitchToAdminPanel()
    {
        this.CurrentViewModel = new AdminPanelViewModel(_platform, this);
    }

    public void SwitchToGroupView()
    {
        this.CurrentViewModel = new AdminGroupViewModel(_platform, this);
    }

    public void SwitchToSettingsView()
    {
        this.CurrentViewModel = new SettingsViewModel(_platform);
    }

    public void SwitchToLoginView()
    {
        this.CurrentViewModel = new AdminLoginViewModel(_platform, this);
    }
}