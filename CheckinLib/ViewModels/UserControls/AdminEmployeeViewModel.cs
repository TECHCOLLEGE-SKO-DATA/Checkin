using System.Collections.ObjectModel;
using CheckinLib.Platform;
using PCSC.Interop;
using CheckinLib.Models;

namespace CheckinLib.ViewModels.UserControls;

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
            var sortedList = SelectedEmployeeGroup.OrderBy(emp => emp.FirstName).ToList();
            SelectedEmployeeGroup.Clear();
            foreach (var employee in sortedList)
            {
                SelectedEmployeeGroup.Add(employee);
            }
            //foreach (var employee in platform.MainWindowViewModel.Employees)
            //{
            //    SelectedEmployeeGroup.Add(employee);
            //}
        };
        SelectedEmployees = new();
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
        _platform.MainWindowViewModel.SeeEmployeeTime(employee);
    }
}