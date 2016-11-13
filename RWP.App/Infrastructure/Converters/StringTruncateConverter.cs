using System;
using System.Globalization;
using System.Windows.Data;

namespace RWP.App.Infrastructure.Converters
{
  public class StringTruncateConverter : IValueConverter
  {
    private const string DEFAULT_ELLIPSIS = "...";
    private const int DEFAULT_LENGTH = 33;

    public StringTruncateConverter()
    {
      Ellipsis = DEFAULT_ELLIPSIS;
      Length = DEFAULT_LENGTH;
    }

    public string Ellipsis { get; set; }
    public int Length { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var str = value as string;
      if (string.IsNullOrEmpty(str))
        return string.Empty;

      return Utils.TruncateString(str, Ellipsis, Length);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
