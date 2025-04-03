using System.Windows;
using CheckInSystem.CardReader;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.UserControls;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using PCSC.Interop;
using CheckInSystem.Views.Windows;

namespace CheckInSystem.ViewModels.Windows;
public class MainWindowViewModel : ViewModelBase
{
    AdminPanelViewModel _adminPanelViewModel;
    public AdminPanelViewModel AdminPanelViewModel
    {
        get => _adminPanelViewModel;
        set => SetProperty(ref _adminPanelViewModel, value, nameof(AdminPanelViewModel));
    }
    AdminGroupViewModel _adminGroupViewModel;
    public AdminGroupViewModel AdminGroupViewModel
    {
        get => _adminGroupViewModel;
        set => SetProperty(ref _adminGroupViewModel, value, nameof(AdminGroupViewModel));
    }

    LoginScreenViewModel _loginViewModel;
    public LoginScreenViewModel LoginScreenViewModel
    {
        get => _loginViewModel;
        set => SetProperty(ref _loginViewModel, value, nameof(LoginScreenViewModel));
    }
    EmployeeTimeViewModel _employeeTimeViewModel;
    public EmployeeTimeViewModel EmployeeTimeViewModel
    {
        get => _employeeTimeViewModel;
        set {
            // if (_employeeTimeView != null)
            // {
            //     _employeeTimeView.DataContext = value;
            // }
            SetProperty(ref _employeeTimeViewModel, value, nameof(EmployeeTimeViewModel));
        }
    }

    public ObservableCollection<Employee> Employees { get; private set; } = new();
    public ObservableCollection<Group> Groups { get; private set; } = new();

    int _selectedTab = 0;
    public int SelectedTab
    {
        get => _selectedTab;
        set => SetProperty(ref _selectedTab, value, nameof(SelectedTab));
    }

    ContentControl _mainContentControl;
    public ContentControl MainContentControl
    {
        get => _mainContentControl;
        set => SetProperty(ref _mainContentControl, value, nameof(MainContentControl));
    }

    public MainWindowViewModel(IPlatform platform) : base(platform)
    {
        platform.CardReader.CardScanned += (sender, args) => EmployeeCardScanned(args.CardId);

        LoginScreenViewModel = new(platform);
        AdminPanelViewModel = new(platform);
        AdminGroupViewModel = new(platform);
        EmployeeTimeViewModel = new(platform);

        LoginScreenViewModel.LoginSuccessful += (sender, args) => {
            RequestView(typeof(AdminPanelViewModel));
        };
    }

    public void LoadDataFromDatabase()
    {
        Abse absence = new();
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

    public void RequestView(Type view)
    {
        if (view == typeof(LoginScreenViewModel))
        {
            SelectedTab = 0;
        }
        else if (view == typeof(AdminPanelViewModel))
        {
            SelectedTab = 1;
        } 
        else if (view == typeof(AdminGroupViewModel))
        {
            SelectedTab = 2;
        }
        else if (view == typeof(EmployeeTimeViewModel))
        {
            SelectedTab = 3;
        }
        else
        {
            throw new InvalidOperationException($"Cannot switch to unknown view {view}");
        }
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
    public void SeeEmployeeTime(Employee employee)
    {
        _employeeTimeViewModel.SelectedEmployee = employee;
        RequestView(typeof(EmployeeTimeViewModel));
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
            if (editEmployee == null) {
                throw new Exception("Failed saving employee");
            }
            Employees.Add(editEmployee);
        }
        if (Views.Dialog.WaitingForCardDialog.Instance != null) 
            Application.Current.Dispatcher.Invoke( () => {
                Views.Dialog.WaitingForCardDialog.Instance.Close();
            });
        
        EditEmployeeWindow.Open(editEmployee);
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
                Application.Current.Dispatcher.Invoke( () => {
                    Employees.Add(dbEmployee);
                });
            }
        }
    }
    
    private void UpdateCardId(string cardID)
    {
        State.UpdateCard(cardID);
    }
}