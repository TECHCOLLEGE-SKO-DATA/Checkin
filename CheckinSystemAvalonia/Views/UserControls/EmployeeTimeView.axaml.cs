using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CheckinLibrary.Models;
using System;
using CheckInSystemAvalonia.ViewModels.UserControls;
using System.Collections.ObjectModel;
using System.Linq;
using CheckInSystemAvalonia.Platform;
using Avalonia.VisualTree;

namespace CheckInSystemAvalonia.Views.UserControls;

public partial class EmployeeTimeView : UserControl
{
    public EmployeeTimeViewModel _vm => (EmployeeTimeViewModel)DataContext;

    public EmployeeTimeView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void BtnDelete(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        OnSiteTime siteTime = (OnSiteTime)button.DataContext;
        _vm.AppendSiteTimesToDelete(siteTime);
    }

    private async void BtnAddTime(object sender, RoutedEventArgs e)
    {
        var mainWindow = this.GetVisualRoot() as Window;

        InputOnSiteTimeDialog siteTimeDialog = new InputOnSiteTimeDialog(_vm.SelectedEmployee);

        if (mainWindow != null)
        {
            bool? result = await siteTimeDialog.ShowDialog<bool?>(mainWindow);
            if (result == true)
            {
                _vm.AppendSiteTimesToAddToDb(siteTimeDialog.NewSiteTime);
            }
        }
        else
        {
            bool? result = await siteTimeDialog.ShowDialog<bool?>(null);
            if (result == true)
            {
                _vm.AppendSiteTimesToAddToDb(siteTimeDialog.NewSiteTime);
            }
        }
    }


    private void BtnAddAbsence(object sender, RoutedEventArgs e)
    {
        var defaultReason = _vm.AbsenceReasons.FirstOrDefault(r => r.Reason == "Ferie") ?? _vm.AbsenceReasons.First();

        var newAbsence = new Absence(0, _vm.SelectedEmployee.ID, DateTime.Now, DateTime.Now, "", defaultReason.Id);

        _vm.AppendAbsenceToAddToDb(newAbsence);
    }

    private void BtnDeleteAbsence(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        Absence absence = (Absence)button.DataContext;

        _vm.AppendAbsenceToDelete(absence);
    }
}