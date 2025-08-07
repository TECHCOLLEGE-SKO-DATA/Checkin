using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using CheckInSystem.ViewModels.UserControls;
using CheckinLibrary.Models;
using CheckInSystem.Controls;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.Windows;

namespace CheckInSystem.Views.UserControls;

public partial class AdminEmployeeView : UserControl
{
    public AdminEmployeeViewModel _vm => (AdminEmployeeViewModel)DataContext;

    IPlatform _platform;

    public AdminEmployeeView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private void BtnOpenEmployeeEdit(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        Employee employee = (Employee)button.DataContext;
        _vm.EditEmployee(employee);
    }

    private void BtnSeeEmployeeTime(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        Employee employee = (Employee)button.DataContext;
        _vm.SeeEmployeeTime(employee);
    }

    private void BtnEditEmployeeGroup(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        Employee employee = (Employee)button.DataContext;

        _vm.EditEmployeeGroup(employee);
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

        _vm.OpenMessageBoxDeleteAsync(employee);
    }

}