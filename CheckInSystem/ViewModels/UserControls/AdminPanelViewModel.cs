using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.Views.Dialog;
using CheckInSystem.Views.UserControls;
using static Dapper.SqlMapper;
using WpfScreenHelper;
using CheckInSystem.Settings;
using static System.Net.Mime.MediaTypeNames;

namespace CheckInSystem.ViewModels.UserControls;

public class AdminPanelViewModel : ViewModelBase
{
    //public ContentControl EmployeesControl { get; set; } = new();
    ObservableCollection<Group> _groups = new();
    public ObservableCollection<Group> Groups
    {
        get => _groups;
        set => SetProperty(ref _groups, value, nameof(Groups));
    }
    Control _adminPanelContent;
    public Control AdminPanelContent
    {
        get => _adminPanelContent;
        set => SetProperty(ref _adminPanelContent, value, nameof(AdminPanelContent));
    }
    AdminEmployeeViewModel _adminEmployeeViewModel;
    public AdminEmployeeViewModel AdminEmployeeViewModel 
    {
        get => _adminEmployeeViewModel;
        set => SetProperty(ref _adminEmployeeViewModel, value, nameof(AdminEmployeeViewModel));
    }
    EmployeeTimeViewModel _employeeTimeViewModel;
    public EmployeeTimeViewModel EmployeeTimeViewModel
    {
        get => _employeeTimeViewModel;
        set => SetProperty(ref _employeeTimeViewModel, value, nameof(EmployeeTimeViewModel));
    }

    /*public Group? UpdateEmployeesControl
    {
        set
        {
            if (value == null)
            {
                EmployeesControl.Content = new AdminEmployeeView(Employees);
            }
            else
            {
                EmployeesControl.Content = new AdminEmployeeView(value.Members);
            }
        }
    }*/
    
    public AdminPanelViewModel(IPlatform platform) : base(platform)
    {
        AdminEmployeeViewModel = new(platform);
       
        AdminPanelContent = new AdminEmployeeView(AdminEmployeeViewModel);

        platform.DataLoaded += (sender, args) =>
        {
            Groups = platform.MainWindowViewModel.Groups;
        };

        Screen = settings.GetEmployeeOverViewSettings();
    }

    public void Logout()
    {
        //MainContentControl.Content = new LoginScreen(new LoginScreenViewModel(_platform));
        _platform.MainWindowViewModel.RequestView(typeof(LoginScreen));
    }

    public void EditNextScannedCard()
    {
        CardReader.State.UpdateNextEmployee = true;
        Views.Dialog.WaitingForCardDialog.Open();
    }

    public void SwitchToGroups()
    {
        //MainContentControl.Content = new AdminGroupView(new AdminGroupViewModel(_platform));
        _platform.MainWindowViewModel.RequestView(typeof(AdminGroupView));
    }

    public void DeleteEmployee(Employee employee)
    {
        employee.DeleteFromDb();
        foreach (Group group in _platform.MainWindowViewModel.Groups)
        {
            group.Members.Remove(employee);
        }
        _platform.MainWindowViewModel.Employees.Remove(employee);
    }

    public void DeleteEmployee(ObservableCollection<Employee> employees)
    {
        foreach (Employee employee in employees)
        {
            DeleteEmployee(employee);
        }
    }

    public void UpdateOffsite(Employee employee, bool isOffsite, DateTime? offsiteUntil)
    {
        employee.IsOffSite = isOffsite;
        employee.OffSiteUntil = offsiteUntil;
        employee.UpdateDb();
    }
    
    public void UpdateOffsite(ObservableCollection<Employee> employees, /*bool isOffsite, DateTime? offsiteUntil,*/
        DateTime FromDate, DateTime ToDate,string Note, Absence.absenceReason AbsenceReason)
    {
        Absence absence = new();
        absence.ToTime = absence.ToTime.AddHours(23);
        foreach (Employee employee in employees)
        {
            absence.InsertAbsence(employee.ID, FromDate, ToDate, Note, AbsenceReason);

            absence.SetIsOffSite(employee);
            //UpdateOffsite(employee, isOffsite, offsiteUntil);
            //employee.IsOffSite = true;
        }
    }

    public void AddSelectedUsersToGroup(Group group)
    {
        foreach (var employee in AdminEmployeeViewModel.SelectedEmployees)
        {
            group.AddEmployee(employee);
        }
    }
    public void RemoveSelectedUsersToGroup(Group group)
    {
        foreach (var employee in AdminEmployeeViewModel.SelectedEmployees)
        {
            group.RemoveEmployee(employee);
        }
    }

    public void EditGroupsForEmployees()
    {
        EditGroupsForEmployees editGroupsForEmployees = new(_platform.MainWindowViewModel.Groups);

        if (editGroupsForEmployees.ShowDialog() == true && editGroupsForEmployees.SelectedGroup != null)
        {
            if (editGroupsForEmployees.AddGroup) AddSelectedUsersToGroup(editGroupsForEmployees.SelectedGroup);
            if (editGroupsForEmployees.RemoveGroup) RemoveSelectedUsersToGroup(editGroupsForEmployees.SelectedGroup);
        }
    }
    //TEST FOR CHANGING WHAT SCREEN EmployeeOverview
    public int Screen { get; set; }
    SettingsControl settings = new();

    public void setscreen()
    {
        settings.SetEmployeeOverViewSettings(Screen);
    }
    //TEST FOR CHANGING WHAT SCREEN EmployeeOverview
}