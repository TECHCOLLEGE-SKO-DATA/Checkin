using Avalonia.Controls;
using CheckinSystemAvalonia.CardReader;
using CheckinSystemAvalonia.Database;
using CheckinSystemAvalonia.Models;
using CheckinSystemAvalonia.Platform;
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
    AdminPanelViewModel _adminPanelViewModel;
    public AdminPanelViewModel AdminPanelViewModel
    {
        get => _adminPanelViewModel;
        set => this.RaiseAndSetIfChanged(ref _adminPanelViewModel, value);
    }
    AdminGroupViewModel _adminGroupViewModel;
    public AdminGroupViewModel AdminGroupViewModel
    {
        get => _adminGroupViewModel;
        set => this.RaiseAndSetIfChanged(ref _adminGroupViewModel, value, nameof(AdminGroupViewModel));
    }

    AdminLoginViewModel _loginViewModel;
    public AdminLoginViewModel AdminLoginViewModel
    {
        get => _loginViewModel;
        set => this.RaiseAndSetIfChanged(ref _loginViewModel, value, nameof(AdminLoginViewModel));
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
            this.RaiseAndSetIfChanged(ref _employeeTimeViewModel, value);
        }
    }

    public ObservableCollection<Employee> Employees { get; private set; } = new();
    public ObservableCollection<Group> Groups { get; private set; } = new();

    MainWindowViewModel(IPlatform platform) : base(platform)
    {
        platform.CardReader.CardScanned += (sender, args) => EmployeeCardScanned(args.CardId);

        AdminLoginViewModel = new(platform);
        AdminPanelViewModel = new(platform);
        AdminGroupViewModel = new(platform);
        EmployeeTimeViewModel = new(platform);

        AdminLoginViewModel.LoginSuccessful += (sender, args) =>
        {
            RequestView(typeof(AdminPanelViewModel));
        };
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

    private ViewModelBase _currentViewModel;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public void SwitchToGroupView()
    {
       //CurrentViewModel = new AdminGroupViewModel();
    }

    public void SwitchToSettingsView()
    {
        //CurrentViewModel = new SettingsViewModel();
    }

    public void SwitchToLoginView()
    {
       //CurrentViewModel = new AdminLoginViewModel(this);
    }
    /*
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
    */
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
            if (editEmployee == null)
            {
                throw new Exception("Failed saving employee");
            }
            Employees.Add(editEmployee);
        }
        if (Views.Dialog.WaitingForCardDialog.Instance != null)
            Application.Current.Dispatcher.Invoke(() => {
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
}

