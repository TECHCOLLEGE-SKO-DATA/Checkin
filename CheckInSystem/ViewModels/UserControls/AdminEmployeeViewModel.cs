using System.Collections.ObjectModel;
using CheckInSystem.Models;
using CheckInSystem.Views.Windows;

namespace CheckInSystem.ViewModels.UserControls;

public class AdminEmployeeViewModel : ViewmodelBase
{
    public ObservableCollection<Employee> SelectedEmployeeGroup { get; set; }
    public static ObservableCollection<Employee> SelectedEmployees { get; set; }
    
    public AdminEmployeeViewModel(ObservableCollection<Employee> employees)
    {
        SelectedEmployeeGroup = employees;
        SelectedEmployees = new();
    }

    public void EditEmployee(Employee employee)
    {
        EditEmployeeWindow.Open(employee);
    }

    public void DeleteEmployee(Employee employee)
    {
        employee.DeleteFromDb();
        foreach (var group in Groups)
        {
            group.Members.Remove(employee);
        }
        Employees.Remove(employee);
    }
}