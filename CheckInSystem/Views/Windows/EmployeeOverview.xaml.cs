using System.Windows;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.Windows;

namespace CheckInSystem.Views;

public partial class EmployeeOverview : Window
{
    private EmployeeOverviewViewModel vm;
    public EmployeeOverview()
    {
        vm = new EmployeeOverviewViewModel();
        this.DataContext = vm;
        InitializeComponent();
    }

    private void BtnZoomOut(object sender, RoutedEventArgs e)
    {
        vm.ZoomOut();
        vm.UpdateConfig();
    }

    private void BtnZoomIn(object sender, RoutedEventArgs e)
    {
        vm.ZoomIn();
        vm.UpdateConfig();
    }

    private void BtnFullScreenToggle(object sender, RoutedEventArgs e)
    {
        vm.ToggleFullscreen();
    }
}