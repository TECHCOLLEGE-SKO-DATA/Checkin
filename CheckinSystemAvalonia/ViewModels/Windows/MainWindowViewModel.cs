
using Avalonia.Controls;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystemAvalonia.Platform;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.ViewModels.UserControls;
using CheckInSystemAvalonia.Views.UserControls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using CheckinLibrary.Background_tasks;
using System.Windows;
using CheckInSystemAvalonia.CardReader;
using Avalonia;
using Avalonia.Threading;

namespace CheckInSystemAvalonia.ViewModels.Windows;
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
        if (!Design.IsDesignMode)
        {
            platform.CardReader.CardScanned += (sender, args) => EmployeeCardScanned(args.CardId);

            //loads data before making instances of ViewModels
            LoadDataFromDatabase();
        }


        //Making an instance of the VeiwModels
        LoginScreenViewModel = new(platform);
        AdminPanelViewModel = new(platform);
        AdminGroupViewModel = new(platform);
        EmployeeTimeViewModel = new(platform);


        //starting View and ViewModel
        CurrentViewModel = LoginScreenViewModel;
    }

    public void LoadDataFromDatabase()
    {
        if (Design.IsDesignMode)
            return;

        AbsencBackGroundService absence = new();
        DatabaseHelper databaseHelper = new DatabaseHelper();
        foreach (var employee in databaseHelper.GetAllEmployees())
        {
            //Adds Employees to a list in AbsenceBackgroundService.cs
            absence.AddEmployeesToAbsenceCheck(employee);

            //runs a inital check on if people have upcoming absence
            absence.AbsenceTask();

            Employees.Add(employee);
        }
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
        
        /*if ( != null)
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
        CurrentViewModel = new SettingsViewModel(_platform);
    }

    public void SwitchToLoginView()
    {
        CurrentViewModel = LoginScreenViewModel;
    }
}