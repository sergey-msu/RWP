using System.Windows;
using RWP.App.ViewModels;
using RWP.Data;

namespace RWP.App.Views
{
  /// <summary>
  /// Interaction logic for CreateReportWindow.xaml
  /// </summary>
  public partial class PatientResearchReportWindow : Window
  {
    public PatientResearchReportWindow(MedicalResearch research)
    {
      InitializeComponent();

      this.DataContext = new PatientResearchReportViewModel(research);
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
