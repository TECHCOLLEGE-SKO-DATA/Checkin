using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CheckinLibrary.Models;
using System;
using System.Collections.Generic;

namespace CheckInSystemAvalonia;

public partial class EditOffsiteDialog : Window
{
    public EditOffsiteDialog(List<AbsenceReason> absenceReasons, AbsenceReason? selectedReason = null,
    DateTime? fromDate = null, DateTime? toDate = null, string? note = null)
    {
        InitializeComponent();

        _FromDate.SelectedDate = fromDate;
        _ToDate.SelectedDate = toDate;
        _Note.Text = note;
        
        ComboBoxAbsenceReason.ItemsSource = absenceReasons;
        ComboBoxAbsenceReason.ItemTemplate = new FuncDataTemplate<AbsenceReason>((item, _) =>
        {
            return new TextBlock { Text = item.Reason };
        });
        ComboBoxAbsenceReason.SelectedItem = selectedReason;
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
    public AbsenceReason? AbsenceReason => ComboBoxAbsenceReason.SelectedItem as AbsenceReason;

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