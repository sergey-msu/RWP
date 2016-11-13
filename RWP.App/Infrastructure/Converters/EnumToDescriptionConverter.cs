using System;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using RWP.App.Infrastructure.Attributes;

namespace RWP.App.Infrastructure.Converters
{
  public class EnumToDescriptionConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return DependencyProperty.UnsetValue;
      
      var type = value.GetType();
      if (!type.IsEnum)
        return DependencyProperty.UnsetValue;


      var fi = value.GetType().GetField(value.ToString());
      var descriptionAttr = fi.GetCustomAttributes(typeof(EnumDescriptionAttribute), false)
                              .OfType<EnumDescriptionAttribute>()
                              .FirstOrDefault();
      if (descriptionAttr != null)
        return descriptionAttr.Description;

      var descriptionKeyAttr = fi.GetCustomAttributes(typeof(EnumDescriptionKeyAttribute), false)
                              .OfType<EnumDescriptionKeyAttribute>()
                              .FirstOrDefault();
      if (descriptionKeyAttr != null)
        return Utils.GetResource(descriptionKeyAttr.DescriptionKey);

      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
