using Avalonia.Data.Converters;
using Avalonia.Media;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CheckInSystem.Customcontrols
{
    public class EmployeeStatusToReasonConverter : IMultiValueConverter
    {
        DatabaseHelper dbHelper = new();

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count == 3 &&
                values[0] is bool isOffsite &&
                values[1] is bool isCheckedIn &&
                values[2] is Employee employee)
            {
                var (_, reason) = EmployeeStatusHelper.GetStatus(employee, isOffsite, isCheckedIn, dbHelper);
                return reason;
            }

            return string.Empty;
        }
    }
    public static class EmployeeStatusHelper
    {
        public static (IBrush brush, string reason) GetStatus(Employee employee, bool isOffsite, bool isCheckedIn, DatabaseHelper dbhelper)
        {
            if (isOffsite)
            {
                var now = DateTime.Now;
                var inEightHours = now.AddHours(8);
                var absences = dbhelper.GetAllAbsence(employee);

                var currentAbsence = absences.FirstOrDefault(abs =>
                    (abs.FromDate <= now && abs.ToDate >= now) ||
                    (abs.FromDate <= inEightHours && abs.ToDate >= inEightHours));

                if (currentAbsence != null)
                {
                    AbsenceReason absencereason = AbsenceReason.GetById(currentAbsence.AbsenceReasonId);

                    var avaloniaColor = Avalonia.Media.Color.FromArgb(
                        absencereason.HexColor.A,
                        absencereason.HexColor.R,
                        absencereason.HexColor.G,
                        absencereason.HexColor.B);

                    var brush = new SolidColorBrush(avaloniaColor);

                    return (brush, $"|{absencereason.Reason}|"); // brush + reason text
                }

                return (Brushes.Gray, "Offsite");
            }

            return isCheckedIn
                ? (Brushes.Green, "")
                : (new SolidColorBrush(Color.Parse("#d55e00")), "");
        }
    }
}
