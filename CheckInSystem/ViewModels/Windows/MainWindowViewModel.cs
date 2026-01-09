
using Avalonia.Controls;
using Avalonia.Threading;
using CheckinLibrary.Background_tasks;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckinLibrary.Settings;
using CheckInSystem.CardReader;
using CheckInSystem.Controls;
using CheckInSystem.Customcontrols;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.Views;
using PCSC.Interop;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CheckInSystem.ViewModels.Windows;
public class MainWindowViewModel : ViewModelBase
{
    IPlatform _platform;
    public List<AbsenceReason> absenceReasons { get; set; }

    //ViewModels start here

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
    SettingsViewModel _settingsViewModel;
    public SettingsViewModel SettingsViewModel
    {
        get => _settingsViewModel;
        set
        {
            this.RaiseAndSetIfChanged(ref _settingsViewModel, value, nameof(SettingsViewModel));
        }
    }

    UpdateAdminViewModel _updateAdminViewModel;
    public UpdateAdminViewModel UpdateAdminViewModel
    {
        get => _updateAdminViewModel;
        set
        {
            this.RaiseAndSetIfChanged(ref _updateAdminViewModel, value, nameof(UpdateAdminViewModel));
        }
    }

    AdminsViewModel _adminsViewModel;
    public AdminsViewModel AdminsViewModel
    {
        get => _adminsViewModel;
        set
        {
            this.RaiseAndSetIfChanged(ref _adminsViewModel, value, nameof(AdminsViewModel));
        }
    }
    //Viewmodels ends here

    public bool DarkMode { get; set; }
    public ObservableCollection<Employee> Employees { get; private set; } = new();


    ObservableCollection<Group> _groups;
    public ObservableCollection<Group> Groups 
    { 
        get =>_groups;
        private set => this.RaiseAndSetIfChanged(ref _groups, value, nameof(Groups));
    }


    public Group GroupAll { get; private set; } = new();

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

    public AbsencBackGroundService absencBackGroundService = new();

    public EmployeeStatusToBrushConverter brushConverter { get; set; }

    private BackgroundTimeService _timeService;

    EmployeeOverviewViewModel _vm;

    public MainWindowViewModel(IPlatform platform) : base(platform)
    {
        _platform = platform;

        _vm = new EmployeeOverviewViewModel(_platform);
        if (!Design.IsDesignMode)
        {
            platform.CardReader.CardInserted += (sender, args) => EmployeeCardScanned(args.Value);

            //loads data before making instances of ViewModels
            LoadDataFromDatabase();
        }

        
        //Making an instance of the VeiwModels
        LoginScreenViewModel = new(platform);
        AdminPanelViewModel = new(platform);
        AdminGroupViewModel = new(platform);
        EmployeeTimeViewModel = new(platform);
        SettingsViewModel = new(platform);
        UpdateAdminViewModel = new(platform);
        AdminsViewModel = new(platform);

        SettingsControl settingsControl = new();

        absenceReasons = settingsControl.GetAbsenceReasons();

        DarkMode = settingsControl.GetDarkMode();

        //starting View and ViewModel
        CurrentViewModel = LoginScreenViewModel;

        StartBackgroundService(platform);
    }

    private void StartBackgroundService(IPlatform platform)
    {
        _timeService = new BackgroundTimeService();

        _timeService.GetEmployees = () =>
            GetAllEmployees();

        _timeService.PerformMaintenanceAction = employees =>
        {
            Maintenance.CheckOutEmployeesIfTheyForgot(employees.ToList());
            Maintenance.CheckForEndedOffSiteTime(employees.ToList());
        };

        _timeService.OnDailyReset += UpdateUIOnReset;

        _timeService.Start(platform);
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

    public ObservableCollection<Employee> GetAllEmployees()
    {
        return Employees;
    }

    public void LoadDataFromDatabase()
    {
        if (Design.IsDesignMode)
            return;

        foreach (var employee in _platform.Database.GetAllEmployees())
        {
            //Adds Employees to a list in AbsenceBackgroundService.cs
            absencBackGroundService.AddEmployeesToAbsenceCheck(employee);

            //runs a inital check on if people have upcoming absence
            absencBackGroundService.AbsenceTask();

            Employees.Add(employee);
        }

        GroupAll = new Group(0, "All");

        GroupAll.InitializeMembers(Employees);

        Groups = new ObservableCollection<Group>(Group.GetAllGroups(new List<Employee>(Employees)));

        List<Employee> employees = new List<Employee>(Employees);

        Maintenance.CheckOutEmployeesIfTheyForgot(employees);
        Maintenance.CheckForEndedOffSiteTime(employees);
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
        State.UpdateNextEmployee = false;
        Employee? editEmployee = Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        if (editEmployee == null)
        {
            _platform.Database.CardScanned(cardID);
            editEmployee = _platform.Database.GetFromCardId(cardID);
            if (editEmployee == null)
            {
                throw new Exception("Failed saving employee");
            }
            Employees.Add(editEmployee);
        }

        if (WaitingForCardDialog.Instance != null)
            Dispatcher.UIThread.Post(() => {
                WaitingForCardDialog.Instance.Close();
            });
        EditEmployeeWindow.Open(editEmployee, _platform);
    }

    void UpdateEmployeeLocal(string cardID)
    {
        Employee? employee = Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        if (employee != null)
        {
            _platform.Database.CardScanned(cardID); //Update DB
            employee.CardScanned(cardID); //Update UI
        }
        else
        {
            var dbEmployee = _platform.Database.GetFromCardId(cardID);
            if (dbEmployee != null)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    Employees.Add(dbEmployee);
                });

            }
        }
    }

    private void UpdateCardId(string cardID)
    {
        State.UpdateCard(cardID);
    }

    //made to set current Viewmodel used for MainWindow
    private ViewModelBase _currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    //switching methods for changing the View and ViewModel 

    public void SwitchToEmployeeTime(Employee employee)
    {
        _employeeTimeViewModel.SelectedEmployee = employee;
        //exists because for some reason i cant get avalonia to keep the reasonId when going in and out and then back into an employee's 
        _employeeTimeViewModel.RefreshAbsences();
        _platform.MainWindowViewModel.CurrentViewModel = EmployeeTimeViewModel;
    }


    public void SwitchToAdminPanel()
    {
        CurrentViewModel = AdminPanelViewModel;
    }

    public void SwitchToGroupView()
    {
        CurrentViewModel = AdminGroupViewModel;
    }

    public void SwitchToSettingsView()
    {
        CurrentViewModel = SettingsViewModel;
    }

    public void SwitchToLoginView()
    {
        LoginScreenViewModel.Username = "";
        LoginScreenViewModel.PassWord = "";
        CurrentViewModel = LoginScreenViewModel;
    }
    public void SwitchToUpdateAdmin(string Username, string Password)
    {
        UpdateAdminViewModel.txtUserName = "";
        UpdateAdminViewModel.txtPassword = "";

        UpdateAdminViewModel.OldUsername = Username;
        UpdateAdminViewModel.OldPassword = Password;

        CurrentViewModel = UpdateAdminViewModel;
    } 

    public void SwitchToAdmins()
    {
        CurrentViewModel = AdminsViewModel;
    }
}