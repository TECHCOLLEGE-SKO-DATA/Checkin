using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CheckInSystem.Models;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.Views.Dialog;
using CheckInSystem.Views.Windows;

namespace CheckInSystem.Views.UserControls;

public partial class AdminPanel : UserControl
{
    public AdminPanelViewModel _vm => (AdminPanelViewModel) DataContext;
    public AdminPanel()
    {
        InitializeComponent();
    }
    public AdminPanel(AdminPanelViewModel vm)
    {
        throw new Exception("Don't use");
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
        _vm.EditGroupsForEmployees();
    }

    private void BtnEditOffsiteForEmployees(object sender, RoutedEventArgs e)
    {
        EditOffsiteDialog editOffsite = new EditOffsiteDialog();
        Absence absence = new Absence();
        if (editOffsite.ShowDialog() == true)
        { 
            _vm.UpdateOffsite(AdminEmployeeViewModel.SelectedEmployees, /*editOffsite.Isoffsite, editOffsite.OffsiteUntil,*/
                (DateTime)editOffsite.FromDate, (DateTime)editOffsite.ToDate, editOffsite.Note, (Absence.absenceReason)editOffsite.AbsenceReason);
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