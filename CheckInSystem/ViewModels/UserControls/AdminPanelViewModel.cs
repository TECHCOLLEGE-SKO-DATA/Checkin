using System.Collections.ObjectModel;
using System.Windows.Controls;
using CheckInSystem.Models;
using CheckInSystem.Views.UserControls;

namespace CheckInSystem.ViewModels.UserControls;

public class AdminPanelViewModel : ViewmodelBase
{
    private ContentControl EmployeesControl { get; set; }

    public Group? UpdateEmployeesControl
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
    }
    
    public AdminPanelViewModel(ContentControl contentControl)
    {
        EmployeesControl = contentControl;
        EmployeesControl.Content = new AdminEmployeeView(Employees);
    }

    public void Logout()
    {
        MainContentControl.Content = new LoginScreen();
    }

    public void EditNextScannedCard()
    {
        CardReader.State.UpdateNextEmployee = true;
        Views.Dialog.WaitingForCardDialog.Open();
    }

    public void SwitchToGroups()
    {
        MainContentControl.Content = new AdminGroupView();
    }

    public void DeleteEmployee(Employee employee)
    {
        employee.DeleteFromDb();
        foreach (Group group in Groups)
        {
            group.Members.Remove(employee);
        }
        Employees.Remove(employee);
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
    
    public void UpdateOffsite(ObservableCollection<Employee> employees, bool isOffsite, DateTime? offsiteUntil)
    {
        foreach (Employee employee in employees)
        {
            UpdateOffsite(employee, isOffsite, offsiteUntil);
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
}