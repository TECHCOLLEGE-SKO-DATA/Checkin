using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace CheckInSystemAvalonia.Customcontrols
{
    public class BoolToBrushConverter : IValueConverter
    {
        public IBrush TrueBrush { get; set; } = Brushes.Green;
        public IBrush FalseBrush { get; set; } = new SolidColorBrush(Color.Parse("#d55e00"));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? TrueBrush : FalseBrush;

            return FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
