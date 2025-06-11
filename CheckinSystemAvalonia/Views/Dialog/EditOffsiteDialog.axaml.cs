using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CheckinLibrary.Models;
using System;

namespace CheckInSystemAvalonia;

public partial class EditOffsiteDialog : Window
{
    public EditOffsiteDialog(bool isOffsite = false, DateTime? offsiteUntil = null, DateTime? FromDate = null,
        DateTime? ToDate = null, Absence.absenceReason? AbsenceReason = null, string? Note = null)
    {
        InitializeComponent();
        // CbIsOffsite.IsChecked = isOffsite;
        //DpOffsiteUntil.SelectedDate = offsiteUntil;
        _FromDate.SelectedDate = FromDate;
        _ToDate.SelectedDate = ToDate;
        _Note.Text = Note;

        ComboBoxAbsenceReason.ItemsSource = Enum.GetValues(typeof(Absence.absenceReason));
    }

    public string? Note
    {
        get => _Note.Text;
    }

    public DateTimeOffset? FromDate
    {
        get => _FromDate.SelectedDate;
    }

    public DateTimeOffset? ToDate
    {
        get => _ToDate.SelectedDate;
    }
    public Absence.absenceReason? AbsenceReason
    {
        get => (Absence.absenceReason)ComboBoxAbsenceReason.SelectedIndex;
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
        Close(true);
    }
}