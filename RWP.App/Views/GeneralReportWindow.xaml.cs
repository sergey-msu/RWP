using System.Windows;
using RWP.App.ViewModels;
using RWP.Data;

namespace RWP.App.Views
{
  /// <summary>
  /// Interaction logic for CreateReportWindow.xaml
  /// </summary>
  public partial class GeneralReportWindow : Window
  {
    public GeneralReportWindow()
    {
      InitializeComponent();

      this.DataContext = new GeneralReportViewModel();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
