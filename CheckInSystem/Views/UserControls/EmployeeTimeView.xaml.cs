using System.Windows;
using System.Windows.Controls;
using CheckInSystem.Models;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.Views.Dialog;

namespace CheckInSystem.Views.UserControls;

public partial class EmployeeTimeView : UserControl
{
    public EmployeeTimeViewModel vm;
    
    public EmployeeTimeView(Employee employee)
    {
        vm = new(employee);
        DataContext = vm;
        InitializeComponent();
    }

    private void BtnDelete(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        OnSiteTime siteTime = (OnSiteTime)button.DataContext;
        vm.AppendSiteTimesToDelete(siteTime);
    }

    private void BtnCancel(object sender, RoutedEventArgs e)
    {
        vm.RevertSiteTimes();
        ViewmodelBase.MainContentControl.Content = new AdminPanel();
    }

    private void BtnSave(object sender, RoutedEventArgs e)
    {
        vm.SaveChanges();
        ViewmodelBase.MainContentControl.Content = new AdminPanel();
    }

    private void BtnAddTime(object sender, RoutedEventArgs e)
    {
        InputOnSiteTimeDialog siteTimeDialog = new InputOnSiteTimeDialog(vm.SelectedEmployee);
        if (siteTimeDialog.ShowDialog() == true)
        {
            vm.AppendSiteTimesToAddToDb(siteTimeDialog.NewSiteTime);
        }
    }
}