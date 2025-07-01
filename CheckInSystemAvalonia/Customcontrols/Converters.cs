using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CheckInSystemAvalonia.Converters
{
    public class VisibleIfTrueConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b ? b : false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b ? b : false;
        }
    }
    public class DrawingColorToAvaloniaColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Drawing.Color drawingColor)
            {
                return Avalonia.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
            }
            return Avalonia.Media.Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Avalonia.Media.Color avaloniaColor)
            {
                return System.Drawing.Color.FromArgb(avaloniaColor.A, avaloniaColor.R, avaloniaColor.G, avaloniaColor.B);
            }
            return System.Drawing.Color.Black;
        }
    }

    public class IdZeroToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is int id && id == 0;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class IdNotZeroToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is int id && id != 0;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
