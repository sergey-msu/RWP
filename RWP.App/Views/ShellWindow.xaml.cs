using System;
using System.ComponentModel;
using System.Windows;
using RWP.App.ViewModels;
using RWP.App.Infrastructure;

namespace RWP.App.Views
{
  public partial class ShellWindow : Window
  {
    private static readonly string CLOSE_APP_MESSAGE = Utils.GetResource("CloseAppMessage");

    public ShellWindow()
    {
      InitializeComponent();

      this.DataContext = new ShellViewModel();
      TabManager.Instance.AttachTo(_hostTabControl);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      var result = RwpMessageBox.Show(CLOSE_APP_MESSAGE, buttons: MessageBoxButton.YesNo);
      if (result != MessageBoxResult.Yes)
      {
        e.Cancel = true;
      }

      foreach (var tab in TabManager.Instance.Tabs)
      {
        var closed = RwpRoot.Close(tab);
        if (!closed)
        {
          e.Cancel = true;
          return;
        }
      }

      base.OnClosing(e);
    }

    protected override void OnClosed(EventArgs e)
    {
      Application.Current.Shutdown(0);
    }
  }
}
