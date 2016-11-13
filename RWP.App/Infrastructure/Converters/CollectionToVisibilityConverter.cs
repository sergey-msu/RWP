using System;
using System.Linq;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RWP.App.Infrastructure.Converters
{
  public class CollectionToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      bool direct = true;
      if ((parameter is bool && !(bool)parameter) ||
          (parameter is string && bool.TryParse((string)parameter, out direct) && !direct))
        direct = false;

      var result = Visibility.Collapsed;

      if ((value != null) &&
          (value is IEnumerable) &&
          ((IEnumerable)value).Cast<object>().Any())
        result = Visibility.Visible;

      if ((value != null) &&
          (value is ICollection) &&
          ((ICollection)value).Count != 0)
        result = Visibility.Visible;

      return direct ? result :
            (result == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
