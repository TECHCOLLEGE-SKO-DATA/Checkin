using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.Views.UserControls;
using CheckInSystem.Views.Windows;
using PCSC.Interop;
using CheckInSystem.Views.Dialog;

namespace CheckInSystem.ViewModels.UserControls;

public class AdminEmployeeViewModel : ViewModelBase
{
    ObservableCollection<Employee> _selectedEmployeeGroup = new();
    public ObservableCollection<Employee> SelectedEmployeeGroup
    {
        get => _selectedEmployeeGroup;
        set => SetProperty(ref _selectedEmployeeGroup, value, nameof(SelectedEmployeeGroup));
    }
    public static ObservableCollection<Employee> SelectedEmployees { get; set; }
    
    public AdminEmployeeViewModel(IPlatform platform) : base(platform)
    {
        platform.DataLoaded += (sender, args) =>
        {
            SelectedEmployeeGroup = platform.MainWindowViewModel.Employees;
            //foreach (var employee in platform.MainWindowViewModel.Employees)
            //{
            //    SelectedEmployeeGroup.Add(employee);
            //}
        };
        SelectedEmployees = new();
    }

    public void EditEmployee(Employee employee)
    {
        EditEmployeeWindow.Open(employee);
    }

    public void DeleteEmployee(Employee employee)
    {
        employee.DeleteFromDb();
        foreach (var group in _platform.MainWindowViewModel.Groups)
        {
            group.Members.Remove(employee);
        }
        _platform.MainWindowViewModel.Employees.Remove(employee);
    }

    public void SeeEmployeeTime(Employee employee)
    {
        _platform.MainWindowViewModel.EmployeeTimeViewModel = new(_platform) {
            SelectedEmployee = employee
        };
        _platform.MainWindowViewModel.RequestView(typeof(EmployeeTimeView));
    }

    public void EditEmployeeGroup(Employee employee)
    {
        EditGroupsForEmployees editGroupsForEmployees = new(_platform.MainWindowViewModel.Groups);
        if (editGroupsForEmployees.ShowDialog() == true && editGroupsForEmployees.SelectedGroup != null)
        {
            if (editGroupsForEmployees.AddGroup) editGroupsForEmployees.SelectedGroup.AddEmployee(employee);
            if (editGroupsForEmployees.RemoveGroup) editGroupsForEmployees.SelectedGroup.RemoveEmployee(employee);
        }
    }
}