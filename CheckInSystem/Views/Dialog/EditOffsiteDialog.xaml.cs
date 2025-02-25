using CheckInSystem.Models;
using System.Windows;
using static CheckInSystem.Models.Absence;

namespace CheckInSystem.Views.Dialog;

public partial class EditOffsiteDialog : Window
{
    
    public EditOffsiteDialog(bool isOffsite = false, DateTime? offsiteUntil = null)
    {
        InitializeComponent();
        CbIsOffsite.IsChecked = isOffsite;
        DpOffsiteUntil.SelectedDate = offsiteUntil;

        ComboBoxAbsenceReason.ItemsSource = Enum.GetValues(typeof(AbsenceReason)).Cast<AbsenceReason>();
    }

    public bool Isoffsite
    {
        get => CbIsOffsite.IsChecked ?? false;
    }

    public DateTime? OffsiteUntil
    {
        get => DpOffsiteUntil.SelectedDate;
    }

    private void BtnConfrimed(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
    }
    
}