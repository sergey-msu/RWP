using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using RWP.App.Infrastructure.Mvvm;

namespace RWP.App.Infrastructure.Converters
{
  public class TabToMenuItemConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var tabs = value as IEnumerable<RwpTabBase>;
      if (tabs == null || !tabs.Any())
        return DependencyProperty.UnsetValue;

      var menuItems = new List<MenuItem>();

      foreach (var tab in tabs)
      {
        var menuItem = new MenuItem
        {
          Header = tab.Header,
          Command = new DelegateCommand<RwpTabBase>(ActivateWindow),
          CommandParameter = tab
        };
        var binding = new Binding("Header") { Source = tab };
        BindingOperations.SetBinding(menuItem, MenuItem.HeaderProperty, binding);

        menuItems.Add(menuItem);
      }

      return menuItems;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }


    protected virtual void ActivateWindow(RwpTabBase window)
    {
      if (window == null)
        throw new RwpException("TabToMenuItemConverter.ActivateWindow(window=null)");

      RwpRoot.Show(window.GetType(), window.HostID);
    }
  }
}
