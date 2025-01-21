using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CheckInSystem.Models;

namespace CheckInSystem.Views.Dialog;

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

    public void NumberValidationHour(object sender, TextCompositionEventArgs e)
    {
        var textBox = (TextBox)sender;
        var text = (string)textBox.Text;
        text += e.Text;
        e.Handled = !RegexHour.IsMatch(text);
    }

    public void NumberValidationMinutes(object sender, TextCompositionEventArgs e)
    {
        var textBox = (TextBox)sender;
        var text = (string)textBox.Text;
        text += e.Text;
        e.Handled = !RegexMinutes.IsMatch(text);
    }

    private void PastingHandler(object sender, DataObjectPastingEventArgs e)
    {
        e.CancelCommand();
    }

    private void BtnSaveTime(object sender, RoutedEventArgs e)
    {
        if (UserInputError()) return;
        var arrivalTime = TimeOnly.FromDateTime(DateTime.ParseExact($"{TbArrivalHour.Text}:{TbArrivalMinutes.Text}", "HH:mm", CultureInfo.InvariantCulture));
        var departureTime = TimeOnly.FromDateTime(DateTime.ParseExact($"{TbDepartureHour.Text}:{TbDepartureMinutes.Text}", "HH:mm", CultureInfo.InvariantCulture));
        if (arrivalTime > departureTime)
        {
            MessageBox.Show("Ankomsttid skal være før Afgangstid", "fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        var arrival = (DpSelectedDate.SelectedDate ?? DateTime.Now) 
                      + arrivalTime.ToTimeSpan();
        var departure = DpSelectedDate.SelectedDate + departureTime.ToTimeSpan();

        Debug.WriteLine($"Arrival is {arrival}");
        Debug.WriteLine($"Departure is {departure}");
        NewSiteTime = new OnSiteTime()
        {
            EmployeeID = SelectedEmployee.ID,
            ArrivalTime = arrival,
            DepartureTime = departure
        };
        this.DialogResult = true;
    }

    private bool UserInputError()
    {
        var isValidInput = (string s, Regex test) => test.IsMatch(s);

        var isValidDate = DpSelectedDate.SelectedDate != null;
        
        var isValidArrivalHour = isValidInput(TbArrivalHour.Text, RegexHour);
        var isValidArrivalMinute = isValidInput(TbArrivalMinutes.Text, RegexMinutes);
        
        var isValidDepartureHour = isValidInput(TbDepartureHour.Text, RegexMinutes);
        var isValidDepartureMinute = isValidInput(TbDepartureMinutes.Text, RegexMinutes);

        bool hadError = false;
        if (!isValidDate) ErrorTb("Dato",ref hadError);
        else if (!isValidArrivalHour) ErrorTb("Ankomst time", ref hadError); 
        else if (!isValidArrivalMinute) ErrorTb("Ankomst minut", ref hadError);
        else if (!isValidDepartureHour) ErrorTb("Afgangs time", ref hadError);
        else if (!isValidDepartureMinute) ErrorTb("Afgangs minut", ref hadError);
        
        return hadError;
    }

    private void ErrorTb(string Tb, ref bool hadError)
    {
        hadError = true;
        MessageBox.Show($"Der er fejl i {Tb}", "fejl", MessageBoxButton.OK, MessageBoxImage.Error); 
    }
}