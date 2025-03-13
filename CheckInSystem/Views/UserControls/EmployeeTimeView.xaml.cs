using System.Windows;
using System.Windows.Controls;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.Views.Dialog;

namespace CheckInSystem.Views.UserControls;

public partial class EmployeeTimeView : UserControl
{
    public EmployeeTimeViewModel _vm => (EmployeeTimeViewModel) DataContext;
    public EmployeeTimeView()
    {
        InitializeComponent();
    }

    private void BtnDelete(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        OnSiteTime siteTime = (OnSiteTime)button.DataContext;
        _vm.AppendSiteTimesToDelete(siteTime);
    }

    private void BtnCancel(object sender, RoutedEventArgs e)
    {
        _vm.RevertSiteTimes();
    }

    private void BtnSave(object sender, RoutedEventArgs e)
    {
        _vm.SaveChanges();
    }

    private void BtnAddTime(object sender, RoutedEventArgs e)
    {
        InputOnSiteTimeDialog siteTimeDialog = new InputOnSiteTimeDialog(_vm.SelectedEmployee);
        if (siteTimeDialog.ShowDialog() == true)
        {
            _vm.AppendSiteTimesToAddToDb(siteTimeDialog.NewSiteTime);
        }
    }

    private void BtnAddAbsence(object sender, RoutedEventArgs e)
    {
        var newAbsence = new Absence(0, _vm.SelectedEmployee.ID, DateTime.Now, DateTime.Now, "", Absence.absenceReason.Ferie);

        _vm.AppendAbsenceToAddToDb(newAbsence);
    }

    private void BtnDeleteAbsence(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        Absence absence = (Absence)button.DataContext;

        _vm.AppendAbsenceToDelete(absence);
    }
}