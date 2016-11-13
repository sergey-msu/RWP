﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RWP.App.Infrastructure.Converters
{
  public class IsNullToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return Visibility.Collapsed;

      return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
