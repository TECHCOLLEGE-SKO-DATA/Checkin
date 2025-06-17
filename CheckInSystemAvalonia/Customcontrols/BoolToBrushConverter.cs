using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;
using CheckinLibrary.Models;
using System.Collections.Generic;

namespace CheckInSystemAvalonia.Customcontrols
{
    public class EmployeeStatusToBrushConverter : IMultiValueConverter
    {
        public IBrush CheckedInBrush { get; set; } = Brushes.Green;
        public IBrush NotCheckedInBrush { get; set; } = new SolidColorBrush(Color.Parse("#d55e00"));
        public IBrush OffsiteBrush { get; set; } = Brushes.Blue;

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count == 2 &&
                values[0] is bool isOffsite &&
                values[1] is bool isCheckedIn)
            {
                if (isOffsite)
                    return OffsiteBrush;

                return isCheckedIn ? CheckedInBrush : NotCheckedInBrush;
            }

            return NotCheckedInBrush;
        }
    }
}
