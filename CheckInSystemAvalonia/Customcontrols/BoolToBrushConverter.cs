using Avalonia.Data.Converters;
using Avalonia.Media;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystemAvalonia.Converters;
using CheckInSystemAvalonia.Platform;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CheckInSystemAvalonia.Customcontrols
{
    public class EmployeeStatusToBrushConverter : IMultiValueConverter
    {
        DatabaseHelper dbhelper = new DatabaseHelper();
        public IBrush CheckedInBrush { get; set; } = Brushes.Green;
        public IBrush NotCheckedInBrush { get; set; } = new SolidColorBrush(Color.Parse("#d55e00"));
        Brush absenceColor;
        public IBrush OffsiteBrush => absenceColor;

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count == 3 &&
                values[0] is bool isOffsite &&
                values[1] is bool isCheckedIn &&
                values[2] is Employee employee)
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
                        int reasonId = currentAbsence.AbsenceReasonId;
                        AbsenceReason absencereason = AbsenceReason.GetById(reasonId);

                        System.Drawing.Color drawingColor = absencereason.HexColor;
                        var avaloniaColor = Avalonia.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);

                        absenceColor = new SolidColorBrush(avaloniaColor);

                        return absenceColor;
                    }

                    return OffsiteBrush;
                }

                return isCheckedIn ? CheckedInBrush : NotCheckedInBrush;
            }

            return NotCheckedInBrush;
        }

    }
}
