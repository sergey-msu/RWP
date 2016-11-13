using System;
using System.Globalization;
using System.Windows;

namespace RWP.Shell
{
  /// <summary>
  /// Application
  /// </summary>
  public class App : Application
  {
    public App()
    {
      InitResources();
    }

    private void InitLocal()
    {
      CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
    }

    private void InitResources()
    {
      Resources = new ResourceDictionary();
      Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(new Uri("/RWP.App;component/Resources/Strings.xaml", UriKind.Relative)));
      Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(new Uri("/RWP.App;component/Resources/Images.xaml", UriKind.Relative)));
      Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(new Uri("/RWP.App;component/Resources/Styles.xaml", UriKind.Relative)));
      Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(new Uri("/RWP.App;component/Resources/Converters.xaml", UriKind.Relative)));
    }
  }
}
