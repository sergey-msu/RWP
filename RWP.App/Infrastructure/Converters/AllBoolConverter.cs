using System;
using System.Linq;
using System.Globalization;
using System.Windows.Data;

namespace RWP.App.Infrastructure.Converters
{
  public class AllConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values == null)
        return true;

      var flags = values.Select(v => !(v is bool) ? true : (bool)v);

      return flags.All(e => e);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
