using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.Views.Dialog;
using CheckInSystem.Views.Windows;

namespace CheckInSystem.Views.UserControls;

public partial class AdminPanel : UserControl
{
    private AdminPanelViewModel vm;
    public AdminPanel()
    {
        InitializeComponent();
        vm = new AdminPanelViewModel(EmployeesControl);
        DataContext = vm;
    }

    private void BtnResetGroup(object sender, RoutedEventArgs e)
    {
        SelectedGroup.SelectedIndex = -1;
    }

    private void BtnLogOut(object sender, RoutedEventArgs e)
    {
        vm.Logout();
    }

    private void BtnEditGroupsForEmployees(object sender, RoutedEventArgs e)
    {
        EditGroupsForEmployees editGroupsForEmployees = new (ViewmodelBase.Groups);
        if (editGroupsForEmployees.ShowDialog() == true && editGroupsForEmployees.SelectedGroup != null)
        {
            if (editGroupsForEmployees.AddGroup) vm.AddSelectedUsersToGroup(editGroupsForEmployees.SelectedGroup);
            if (editGroupsForEmployees.RemoveGroup) vm.RemoveSelectedUsersToGroup(editGroupsForEmployees.SelectedGroup);
        }
    }

    private void BtnEditOffsiteForEmployees(object sender, RoutedEventArgs e)
    {
        EditOffsiteDialog editOffsite = new EditOffsiteDialog();
        if (editOffsite.ShowDialog() == true)
        {
            vm.UpdateOffsite(AdminEmployeeViewModel.SelectedEmployees, editOffsite.Isoffsite, editOffsite.OffsiteUntil);
        }
    }

    private void BtnDeleteEmployees(object sender, RoutedEventArgs e)
    {
        MessageBoxResult messageBoxResult = MessageBox.Show("Er du sikker på at du vil slette de valgte brugere.", "Sletning", MessageBoxButton.OKCancel);
        if (messageBoxResult == MessageBoxResult.OK)
        {
            vm.DeleteEmployee(AdminEmployeeViewModel.SelectedEmployees);
            AdminEmployeeViewModel.SelectedEmployees.Clear();
        }
    }

    private void BtnSwitchToGroups(object sender, RoutedEventArgs e)
    {
        vm.SwitchToGroups();
    }

    private void BtnEditScannedCard(object sender, RoutedEventArgs e)
    {
        vm.EditNextScannedCard();
    }
}