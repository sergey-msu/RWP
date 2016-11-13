using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.Models;
using RWP.Data.Repositories;
using RWP.Data.Filters;
using RWP.App.Infrastructure;
using RWP.App.Reporting.Reports;
using RWP.App.Views;
using RWP.Data;

namespace RWP.App.ViewModels
{
  public class ResearchListViewModel : ViewModelBase
  {
    #region Fields

    private const int FILTER_TIMESPAN = 1;

    private DispatcherTimer _filterTimer;
    private readonly PatientRepository _patientRepository;
    private readonly MedicalResearchRepository _researchRepository;
    private readonly Pager _pager;

    #endregion Fields

    public ResearchListViewModel()
    {
      _patientRepository = new PatientRepository();
      _researchRepository = new MedicalResearchRepository();
      _pager = new Pager();

      CreatePatientResearchReportCommand = new DelegateCommand<ResearchListItemModel>(CreatePatientResearchReport);
      ViewPatientResearchCommand = new DelegateCommand<ResearchListItemModel>(ViewPatientResearch);
      DynamicPatientResearchCommand = new DelegateCommand<ResearchListItemModel>(DynamicPatientResearch);
      NewPatientCommand = new DelegateCommand(NewPatient);
      NewPatientResearchCommand = new DelegateCommand(NewPatientResearch);
      ClearFilterCommand = new DelegateCommand(ClearFilter);
      PageBackCommand = new DelegateCommand(PageBack);
      PageNextCommand = new DelegateCommand(PageNext);

      InitFilter();

      SelectedItemsOnScreen = Constants.ITEMS_ON_SCREEN.First();
    }

    #region Properties

    private List<PatientListItemModel> _patients;
    public List<PatientListItemModel> Patients
    {
      get { return _patients; }
      set
      {
        _patients = value;
        OnPropertyChanged("Patients");
      }
    }

    private PatientListItemModel _selectedPatient;
    public PatientListItemModel SelectedPatient
    {
      get { return _selectedPatient; }
      set
      {
        _selectedPatient = value;
        OnPropertyChanged("SelectedPatient");
      }
    }

    private PatientFilterModel _filter;
    public PatientFilterModel Filter
    {
      get { return _filter; }
      set
      {
        _filter = value;
        OnPropertyChanged("Filter");
      }
    }

    private int _selectedItemsOnScreen;
    public int SelectedItemsOnScreen
    {
      get { return _selectedItemsOnScreen; }
      set
      {
        _selectedItemsOnScreen = value;
        Pager.Step = value;
        ExecFilter();
        OnPropertyChanged("SelectedItemsOnScreen");
      }
    }

    public Pager Pager
    {
      get { return _pager; }
    }

    #endregion Properties

    #region CreatePatientResearchReportCommand

    public ICommand CreatePatientResearchReportCommand { get; private set; }

    private void CreatePatientResearchReport(ResearchListItemModel research)
    {
      if (research == null)
        throw new RwpException("ResearchListViewModel.CreatePatientResearchReport(research=null)");
        
      var entity = _researchRepository.GetFullResearch(research.Entity.Id);
      var repWindow = new PatientResearchReportWindow(entity);
      repWindow.ShowDialog();
    }

    #endregion CreatePatientResearchReportCommand

    #region ViewPatientResearchCommand

    public ICommand ViewPatientResearchCommand { get; private set; }

    private void ViewPatientResearch(ResearchListItemModel research)
    {
      if (research == null)
        throw new RwpException("ResearchListViewModel.ViewPatientResearch(research=null)");

      var entity = _researchRepository.GetFullResearch(research.Entity.Id);
      RwpRoot.Show<PatientResearchTab>(string.Format(Constants.WINDOW_PATIENT_RESERCH, research.Entity.IdPatient, research.Entity.Id),
                                      pars: new object[] { entity });
    }

    #endregion ViewPatientResearchCommand

    #region DynamicPatientResearchCommand

    public ICommand DynamicPatientResearchCommand { get; private set; }

    private void DynamicPatientResearch(ResearchListItemModel research)
    {
      if (research == null)
        throw new RwpException("ResearchListViewModel.DynamicPatientResearchCommand(research=null)");

      var entity = _researchRepository.GetFullResearch(research.Entity.Id);
      entity.Id = 0;
      RwpRoot.Show<PatientResearchTab>(string.Format(Constants.WINDOW_PATIENT_RESERCH, research.Entity.IdPatient, 0),
                                      pars: new object[] { entity });
    }

    #endregion DynamicPatientResearchCommand

    #region NewPatientResearchCommand

    public ICommand NewPatientResearchCommand { get; private set; }

    private void NewPatientResearch()
    {
      if (SelectedPatient == null)
        throw new RwpException("ResearchListViewModel.NewPatientResearch(): SelectedPatient=null");

      var research = new MedicalResearch { Patient = SelectedPatient.Entity };
      RwpRoot.Show<PatientResearchTab>(string.Format(Constants.WINDOW_PATIENT_RESERCH, research.IdPatient, 0),
                                      pars: new object[] { research });
    }

    #endregion NewPatientResearchCommand

    #region NewPatientCommand

    public ICommand NewPatientCommand { get; private set; }

    private void NewPatient()
    {
      var window = RwpRoot.Show<PatientListTab>(Constants.WINDOW_PATIENT_LIST);
      window.Execute(Constants.COMMAND_CREATE_NEW_PATIENT);
    }

    #endregion NewPatientCommand

    #region ClearFilterCommand

    public ICommand ClearFilterCommand { get; private set; }

    private void ClearFilter()
    {
      Filter.Clear();
      ExecFilter();
    }

    #endregion ClearFilterCommand

    #region PageBackCommand

    public ICommand PageBackCommand { get; private set; }

    private void PageBack()
    {
      _pager.Back();
      ExecFilter();
    }

    #endregion PageBackCommand

    #region PageNextCommand

    public ICommand PageNextCommand { get; private set; }

    private void PageNext()
    {
      _pager.Next();
      ExecFilter();
    }

    #endregion PageNextCommand

    #region Overrides

    public override bool Execute(string command, params object[] args)
    {
      if (command == Constants.COMMAND_REFRESH)
      {
        ExecFilter();
        return true;
      }

      if (command == Constants.COMMAND_FOCUS_PATIENT)
      {
        var id = (int)args[0];
        Filter.Id = id;
        ExecFilter();
        SelectedPatient = Patients.FirstOrDefault();

        return true;
      }

      return true;
    }

    #endregion Overrides

    #region Private

    private void LoadPatients(PatientFilter filter)
    {
      _pager.Total = _patientRepository.GetPatientsCount(filter);

      var patients = _patientRepository.GetPatientsWithResearches(filter);

      var result = new List<PatientListItemModel>();

      foreach (var patient in patients)
      {
        var patientModel = new PatientListItemModel(patient);

        foreach (var research in patient.MedicalResearches)
        {
          var researchModel = new ResearchListItemModel(research);
          patientModel.Researches.Add(researchModel);
        }

        result.Add(patientModel);
      }

      Patients = result.OrderByDescending(p => p.LastResearchDate).ToList();
    }

    private void InitFilter()
    {
      _filterTimer = new DispatcherTimer
      {
        Interval = TimeSpan.FromSeconds(FILTER_TIMESPAN),
        IsEnabled = false
      };

      Filter = new PatientFilterModel { Take = Constants.ITEMS_ON_SCREEN.First() };
      Filter.PropertyChanged += FilterPropertyChanged;
    }

    private void ExecFilter()
    {
      _filterTimer.Tick -= FilterTimerTick;
      Filter.Skip = _pager.Step * (_pager.Page - 1);
      Filter.Take = _pager.Step;
      var filter = Filter.GetFilter();
      LoadPatients(filter);
    }

    private void FilterPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      _filterTimer.Stop();
      _filterTimer.Tick -= FilterTimerTick;
      _filterTimer.Tick += FilterTimerTick;
      _filterTimer.Start();
    }

    private void FilterTimerTick(object sender, EventArgs e)
    {
      ExecFilter();
    }

    #endregion Private
  }
}