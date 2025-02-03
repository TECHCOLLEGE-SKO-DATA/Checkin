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
    private AdminPanelViewModel _vm;
    public AdminPanel(AdminPanelViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = _vm;
    }

    private void BtnResetGroup(object sender, RoutedEventArgs e)
    {
        SelectedGroup.SelectedIndex = -1;
    }

    private void BtnLogOut(object sender, RoutedEventArgs e)
    {
        _vm.Logout();
    }

    private void BtnEditGroupsForEmployees(object sender, RoutedEventArgs e)
    {
        EditGroupsForEmployees editGroupsForEmployees = new (ViewmodelBase.Groups);
        if (editGroupsForEmployees.ShowDialog() == true && editGroupsForEmployees.SelectedGroup != null)
        {
            if (editGroupsForEmployees.AddGroup) _vm.AddSelectedUsersToGroup(editGroupsForEmployees.SelectedGroup);
            if (editGroupsForEmployees.RemoveGroup) _vm.RemoveSelectedUsersToGroup(editGroupsForEmployees.SelectedGroup);
        }
    }

    private void BtnEditOffsiteForEmployees(object sender, RoutedEventArgs e)
    {
        EditOffsiteDialog editOffsite = new EditOffsiteDialog();
        if (editOffsite.ShowDialog() == true)
        {
            _vm.UpdateOffsite(AdminEmployeeViewModel.SelectedEmployees, editOffsite.Isoffsite, editOffsite.OffsiteUntil);
        }
    }

    private void BtnDeleteEmployees(object sender, RoutedEventArgs e)
    {
        MessageBoxResult messageBoxResult = MessageBox.Show("Er du sikker på at du vil slette de valgte brugere.", "Sletning", MessageBoxButton.OKCancel);
        if (messageBoxResult == MessageBoxResult.OK)
        {
            _vm.DeleteEmployee(AdminEmployeeViewModel.SelectedEmployees);
            AdminEmployeeViewModel.SelectedEmployees.Clear();
        }
    }

    private void BtnSwitchToGroups(object sender, RoutedEventArgs e)
    {
        _vm.SwitchToGroups();
    }

    private void BtnEditScannedCard(object sender, RoutedEventArgs e)
    {
        _vm.EditNextScannedCard();
    }
}