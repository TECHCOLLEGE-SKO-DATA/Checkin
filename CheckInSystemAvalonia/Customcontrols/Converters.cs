﻿using System;
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
}
