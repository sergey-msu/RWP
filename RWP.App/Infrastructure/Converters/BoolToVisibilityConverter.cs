using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RWP.App.Infrastructure.Converters
{
  public class BoolToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is bool))
        return DependencyProperty.UnsetValue;

      return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
