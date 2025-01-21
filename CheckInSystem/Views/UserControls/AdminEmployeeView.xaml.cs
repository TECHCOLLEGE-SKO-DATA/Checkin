using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CheckInSystem.Models;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.Views.Dialog;

namespace CheckInSystem.Views.UserControls;

public partial class AdminEmployeeView : UserControl
{
    private AdminEmployeeViewModel vm;
    
    public AdminEmployeeView(ObservableCollection<Employee> employees)
    {
        vm = new AdminEmployeeViewModel(employees);
        DataContext = vm;
        InitializeComponent();
    }

    private void BtnOpenEmployeeEdit(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        Employee employee = (Employee)button.DataContext;
        vm.EditEmployee(employee);
    }

    private void BtnSeeEmployeeTime(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        Employee employee = (Employee)button.DataContext;
        EmployeeTimeView timeView = new EmployeeTimeView(employee);
        ViewmodelBase.MainContentControl.Content = timeView;
    }

    private void BtnEditEmployeeGroup(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        Employee employee = (Employee)button.DataContext;
        
        EditGroupsForEmployees editGroupsForEmployees = new (ViewmodelBase.Groups);
        if (editGroupsForEmployees.ShowDialog() == true && editGroupsForEmployees.SelectedGroup != null)
        {
            if (editGroupsForEmployees.AddGroup) editGroupsForEmployees.SelectedGroup.AddEmployee(employee);
            if (editGroupsForEmployees.RemoveGroup) editGroupsForEmployees.SelectedGroup.RemoveEmployee(employee);
        }
    }

    private void CbSelected(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        Employee employee = (Employee)checkBox.DataContext;
        AdminEmployeeViewModel.SelectedEmployees.Add(employee);
        Debug.WriteLine($"{employee.FirstName} Checked");
    }

    private void CbUnSelected(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        Employee employee = (Employee)checkBox.DataContext;
        AdminEmployeeViewModel.SelectedEmployees.Remove(employee);
        Debug.WriteLine($"{employee.FirstName} Unchecked");
    }

    private void BtnDeleteEmployee(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        Employee employee = (Employee)button.DataContext;
        MessageBoxResult result =
            MessageBox.Show(
                $"Er du sikker på at du vil slette {employee.FirstName} {employee.MiddleName} {employee.LastName}",
                "Sletning",
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning);
        if (result == MessageBoxResult.Yes)
        {
            vm.DeleteEmployee(employee);
        }
    }
}