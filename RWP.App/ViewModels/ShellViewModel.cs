using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.Views;
using RWP.App.Reporting;
using System;

namespace RWP.App.ViewModels
{
  public class ShellViewModel : ViewModelBase
  {
    private static readonly string CLOSE_ALL_WINDOWS_MESSAGE = Utils.GetResource("CloseAllTabsMessage");
    private static readonly string CLOSE_APP_MESSAGE = Utils.GetResource("CloseAppMessage");

    public ShellViewModel()
    {
      ExitCommand = new DelegateCommand(Exit);
      CloseAllTabsCommand = new DelegateCommand(CloseAllWindows);
      ShowTemplateListCommand = new DelegateCommand(ShowTemplateList);
      ShowScopesListCommand = new DelegateCommand(ShowScopesList);
      ShowDoctorListCommand = new DelegateCommand(ShowDoctorList);
      ShowCustomerListCommand = new DelegateCommand(ShowCustomerList);
      ShowPatientListCommand = new DelegateCommand(ShowPatientList);
      ShowResearchesListCommand = new DelegateCommand(ShowResearchesList);
      CreateGeneralReportCommand = new DelegateCommand(CreateGeneralReport);
    }

    #region ExitCommand

    public ICommand ExitCommand { get; private set; }

    private void Exit()
    {
      var result = RwpMessageBox.Show(CLOSE_APP_MESSAGE, buttons: MessageBoxButton.YesNo);
      if (result != MessageBoxResult.Yes)
        return;

      Application.Current.Shutdown(0);
    }

    #endregion ExitCommand

    #region CloseAllTabsCommand

    public ICommand CloseAllTabsCommand { get; private set; }

    private void CloseAllWindows()
    {
      var result = RwpMessageBox.Show(CLOSE_ALL_WINDOWS_MESSAGE, buttons: MessageBoxButton.YesNo);
      if (result != MessageBoxResult.Yes) return;

      foreach (var tab in TabManager.Instance.Tabs.ToList())
        RwpRoot.Close(tab);
    }

    #endregion CloseAllTabsCommand

    #region ShowDoctorListCommand

    public ICommand ShowDoctorListCommand { get; private set; }

    private void ShowDoctorList()
    {
      RwpRoot.Show<DoctorListTab>(Constants.WINDOW_DOCTOR_LIST);
    }

    #endregion ShowDoctorListCommand

    #region ShowCustomerListCommand

    public ICommand ShowCustomerListCommand { get; private set; }

    private void ShowCustomerList()
    {
      RwpRoot.Show<CustomerListTab>(Constants.WINDOW_CUSTOMER_LIST);
    }

    #endregion ShowCustomerListCommand

    #region ShowTemplateListCommand

    public ICommand ShowTemplateListCommand { get; private set; }

    private void ShowTemplateList()
    {
      RwpRoot.Show<TemplateListTab>(Constants.WINDOW_TEMPLATE_LIST);
    }

    #endregion ShowTemplateListCommand

    #region ShowScopesListCommand

    public ICommand ShowScopesListCommand { get; private set; }

    private void ShowScopesList()
    {
      RwpRoot.Show<ScopeListTab>(Constants.WINDOW_SCOPE_LIST);
    }

    #endregion ShowScopesListCommand

    #region ShowPatientListCommand

    public ICommand ShowPatientListCommand { get; private set; }

    private void ShowPatientList()
    {
      RwpRoot.Show<PatientListTab>(Constants.WINDOW_PATIENT_LIST);
    }

    #endregion ShowPatientListCommand

    #region ShowResearchesListCommand

    public ICommand ShowResearchesListCommand { get; private set; }

    private void ShowResearchesList()
    {
      RwpRoot.Show<ResearchListTab>(Constants.WINDOW_RESEARCH_LIST);
    }

    #endregion ShowResearchesListCommand

    #region CreateGeneralReportCommand

    public ICommand CreateGeneralReportCommand { get; private set; }

    private void CreateGeneralReport()
    {
      var repWindow = new GeneralReportWindow();
      repWindow.ShowDialog();
    }

    #endregion CreateGeneralReportCommand
  }
}