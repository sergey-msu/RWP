using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using RWP.App.Infrastructure;

namespace RWP.App.Views
{
  /// <summary>
  /// Interaction logic for RwpMessageBox.xaml
  /// </summary>
  public partial class RwpMessageBox : Window
  {
    public RwpMessageBox()
    {
      InitializeComponent();
    }

    public static MessageBoxResult Show(string message, string details = null, string caption = null, MessageBoxButton buttons = MessageBoxButton.OK)
    {
      var dialog = new RwpMessageBox(message, details, caption, buttons);
      dialog.ShowDialog();

      return dialog.m_Result;
    }

    public static MessageBoxResult Show(Exception error, string message = null, string caption = null, MessageBoxButton buttons = MessageBoxButton.OK)
    {
      message = message ?? error.Message;
      var details = error.ToString();
      var dialog = new RwpMessageBox(message, details, caption, buttons);
      dialog.ShowDialog();

      return dialog.m_Result;
    }

    private RwpMessageBox(string message, string details = null, string caption = null, MessageBoxButton buttons = MessageBoxButton.OK)
    {
      InitializeComponent();

      this.Style = string.IsNullOrWhiteSpace(details) ?
                   Utils.GetResource<Style>("RwpSimpleMessageBoxWindowStyle") :
                   Utils.GetResource<Style>("RwpDetailedMessageBoxWindowStyle");

      m_Result = MessageBoxResult.None;

      this.Title = caption ?? string.Empty;
      this.m_MessageBlock.Text = message;
      this.m_DetailsBox.Text = message;

      m_Toggle.Visibility = !string.IsNullOrWhiteSpace(details) ? Visibility.Visible : Visibility.Collapsed;

      this.m_YesButton.Visibility = buttons == MessageBoxButton.YesNo || buttons == MessageBoxButton.YesNoCancel ? Visibility.Visible : Visibility.Collapsed;
      this.m_NoButton.Visibility = buttons == MessageBoxButton.YesNo || buttons == MessageBoxButton.YesNoCancel ? Visibility.Visible : Visibility.Collapsed;
      this.m_OKButton.Visibility = buttons == MessageBoxButton.OK || buttons == MessageBoxButton.OKCancel ? Visibility.Visible : Visibility.Collapsed;
      this.m_CancelButton.Visibility = buttons == MessageBoxButton.YesNoCancel || buttons == MessageBoxButton.OKCancel ? Visibility.Visible : Visibility.Collapsed;
    }

    private MessageBoxResult m_Result;

    private void OnYesButtonClick(object sender, RoutedEventArgs e)
    {
      m_Result = MessageBoxResult.Yes;
      this.Close();
    }

    private void OnNoButtonClick(object sender, RoutedEventArgs e)
    {
      m_Result = MessageBoxResult.No;
      this.Close();
    }

    private void OnOKButtonClick(object sender, RoutedEventArgs e)
    {
      m_Result = MessageBoxResult.OK;
      this.Close();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
      m_Result = MessageBoxResult.Cancel;
      this.Close();
    }

    private void OnToggleClick(object sender, RoutedEventArgs e)
    {
      var btn = sender as ToggleButton;
      this.m_DetailsBox.Visibility = btn.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
    }
  }
}
