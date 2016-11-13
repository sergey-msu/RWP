using System;
using System.Linq;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace RWP.App.Infrastructure.Converters
{
  public class CollectionToHasItemsConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null) return false;
      
      if ((value is IList))
        return ((IList)value).Count > 0;

      if ((value is IEnumerable))
        return ((IEnumerable)value).Cast<object>().Any();

      return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
