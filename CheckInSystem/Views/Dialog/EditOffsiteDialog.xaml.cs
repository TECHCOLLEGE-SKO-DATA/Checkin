using CheckInSystem.Models;
using System.Windows;
using static CheckInSystem.Models.Absence;

namespace CheckInSystem.Views.Dialog;

public partial class EditOffsiteDialog : Window
{

    public EditOffsiteDialog(bool isOffsite = false, DateTime? offsiteUntil = null, DateTime? FromDate = null,
        DateTime? ToDate = null, absenceReason? AbsenceReason = null, string? Note = null)
    {
        InitializeComponent();
       // CbIsOffsite.IsChecked = isOffsite;
       //DpOffsiteUntil.SelectedDate = offsiteUntil;
        _FromDate.SelectedDate = FromDate;
        _ToDate.SelectedDate = ToDate;
        _Note.Text = Note;

        ComboBoxAbsenceReason.ItemsSource = Enum.GetValues(typeof(absenceReason)).Cast<absenceReason>();
    }

    public string? Note
    {
        get => _Note.Text;
    }

    public DateTime? FromDate
    {
        get => _FromDate.SelectedDate;
    }

    public DateTime? ToDate
    {
        get => _ToDate.SelectedDate;
    }
    public absenceReason? AbsenceReason
    {
        get => (absenceReason)ComboBoxAbsenceReason.SelectedIndex;
    }

    /*public bool Isoffsite
    {
        get => CbIsOffsite.IsChecked ?? false;
    }

    public DateTime? OffsiteUntil
    {
        get => DpOffsiteUntil.SelectedDate;
    }*/

    private void BtnConfrimed(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
    }
    
}