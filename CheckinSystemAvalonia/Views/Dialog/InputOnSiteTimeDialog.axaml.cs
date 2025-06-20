using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CheckInSystemAvalonia.Controls;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System;
using CheckinLibrary.Models;
using System.Threading.Tasks;
using Avalonia.Input;

namespace CheckInSystemAvalonia;

public partial class InputOnSiteTimeDialog : Window
{
    
    private static readonly Regex RegexHour = new("^(0?[0-9]|1[0-9]|2[0-3])$");
    private static readonly Regex RegexMinutes = new("^(0?[0-9]|[1-5][0-9])$");
    public OnSiteTime NewSiteTime;
    public Employee SelectedEmployee;
    public InputOnSiteTimeDialog(Employee employee)
    {
        InitializeComponent();
        DpSelectedDate.SelectedDate = DateTime.Today;
        SelectedEmployee = employee;
    }

    private void NumberValidationHour(object sender, TextInputEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var text = textBox.Text + e.Text;
            e.Handled = !RegexHour.IsMatch(text);
        }
    }

    private void NumberValidationMinutes(object sender, TextInputEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var text = textBox.Text + e.Text;
            e.Handled = !RegexMinutes.IsMatch(text);
        }
    }

    /*
    private void PastingHandler(object sender, DataObjectPastingEventArgs e)
    {
        e.CancelCommand();
    }
    */

    private async void BtnSaveTime(object sender, RoutedEventArgs e)
    {
        if (await UserInputError()) return;

        var arrivalTime = TimeOnly.FromDateTime(DateTime.ParseExact($"{TbArrivalHour.Text}:{TbArrivalMinutes.Text}", "HH:mm", CultureInfo.InvariantCulture));
        var departureTime = TimeOnly.FromDateTime(DateTime.ParseExact($"{TbDepartureHour.Text}:{TbDepartureMinutes.Text}", "HH:mm", CultureInfo.InvariantCulture));

        if (arrivalTime > departureTime)
        {
            await MessageBox.Show(this, "Ankomsttid skal være før Afgangstid", "Fejl", MessageBoxButton.OK);
            return;
        }

        var arrival = (DpSelectedDate.SelectedDate?.DateTime ?? DateTime.Now) + arrivalTime.ToTimeSpan();
        var departure = DpSelectedDate.SelectedDate?.DateTime + departureTime.ToTimeSpan();

        NewSiteTime = new OnSiteTime()
        {
            EmployeeID = SelectedEmployee.ID,
            ArrivalTime = arrival,
            DepartureTime = departure
        };

        this.Close(true);
    }

    private async Task<bool> UserInputError()
    {
        var isValidInput = (string s, Regex test) => test.IsMatch(s);

        var isValidDate = DpSelectedDate.SelectedDate != null;
        var isValidArrivalHour = isValidInput(TbArrivalHour.Text, RegexHour);
        var isValidArrivalMinute = isValidInput(TbArrivalMinutes.Text, RegexMinutes);
        var isValidDepartureHour = isValidInput(TbDepartureHour.Text, RegexHour);
        var isValidDepartureMinute = isValidInput(TbDepartureMinutes.Text, RegexMinutes);

        if (!isValidDate) return await ErrorTbAsync("Dato");
        if (!isValidArrivalHour) return await ErrorTbAsync("Ankomst time");
        if (!isValidArrivalMinute) return await ErrorTbAsync("Ankomst minut");
        if (!isValidDepartureHour) return await ErrorTbAsync("Afgangs time");
        if (!isValidDepartureMinute) return await ErrorTbAsync("Afgangs minut");

        return false;
    }


    private async Task<bool> ErrorTbAsync(string fieldName)
    {
        await MessageBox.Show(this, $"Der er en fejl i {fieldName}", "Fejl", MessageBoxButton.OK);
        return true;
    }
}