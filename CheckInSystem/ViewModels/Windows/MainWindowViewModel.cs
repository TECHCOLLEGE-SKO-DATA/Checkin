using System.Windows;
using CheckInSystem.CardReader;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.Views.Windows;
using CheckInSystem.Views.UserControls;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using PCSC.Interop;

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
        set => SetProperty(ref _employeeTimeViewModel, value, nameof(EmployeeTimeViewModel));
    }

    public ObservableCollection<Employee> Employees { get; private set; } = new();
    public ObservableCollection<Group> Groups { get; private set; } = new();

    #region Views
    AdminPanel _adminPanel;
    AdminGroupView _adminGroupView;
    LoginScreen _loginScreen;
    EmployeeTimeView _employeeTimeView;
    #endregion
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

        _loginScreen = new(LoginScreenViewModel);
        _adminPanel = new(AdminPanelViewModel);
        _adminGroupView = new(AdminGroupViewModel);
        //_employeeTimeView = new(EmployeeTimeViewModel);

        MainContentControl = _loginScreen; //Set startup content
        

    }

    public void LoadDataFromDatabase()
    {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        foreach (var employee in databaseHelper.GetAllEmployees())
        {
            Employees.Add(employee);
        }
        Groups = new ObservableCollection<Group>(Group.GetAllGroups(new List<Employee>(Employees)));

        List<Employee> employees = new List<Employee>(Employees);

        Maintenance.CheckOutEmployeesIfTheyForgot(employees);
        Maintenance.CheckForEndedOffSiteTime(employees);
    }

    public void RequestView(Type view)
    {
        if (view == typeof(LoginScreen))
        {
            MainContentControl = _loginScreen;
        }
        else if (view == typeof(AdminPanel))
        {
            MainContentControl = _adminPanel;
        } 
        else if (view == typeof(AdminGroupView))
        {
            MainContentControl = _adminGroupView;
        }
        else if (view == typeof(EmployeeTimeView))
        {
            MainContentControl = _employeeTimeView;
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
    
    void UpdateNextEmployee(string cardID)
    {
        DatabaseHelper databaseHelper = new();
        State.UpdateNextEmployee = false;
        Employee? editEmployee = Employees.Where(e => e.CardID == cardID).FirstOrDefault();
        if (editEmployee == null)
        {
            databaseHelper.CardScanned(cardID);
            editEmployee = Employees.Where(e => e.CardID == cardID).FirstOrDefault();
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