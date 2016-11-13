using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.Models;
using RWP.Data;
using RWP.Data.Repositories;
using RWP.App.Views;

namespace RWP.App.ViewModels
{
  public class PatientResearchViewModel : ViewModelBase
  {
    #region Fields

    private readonly string NEW_PATIENT_RESEARCH_HEADER = Utils.GetResource("NewPatientResearchHeader");
    private readonly string VIEW_PATIENT_RESEARCH_HEADER = Utils.GetResource("ViewPatientResearchHeader");
    private readonly string CANCEL_PATIENT_RESEARCH_MESSAGE = Utils.GetResource("CancelPatientResearchMessage");

    private readonly DoctorRepository _doctorRepository;
    private readonly PatientRepository _patientRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly MedicalResearchRepository _researchRepository;
    private readonly ScanRegimeRepository _regimeRepository;
    private readonly ResearchTemplateRepository _templateRepository;
    private readonly ResearchScopeRepository _scopeRepository;

    #endregion Fields

    public PatientResearchViewModel(MedicalResearch research)
    {
      if (research == null)
        throw new RwpException("PatientResearchViewModel.ctor(research=null)");
      if (research.Patient == null)
        throw new RwpException("PatientResearchViewModel.ctor(research.Patient=null)");

      _doctorRepository = new DoctorRepository();
      _patientRepository = new PatientRepository();
      _customerRepository = new CustomerRepository();
      _researchRepository = new MedicalResearchRepository();
      _regimeRepository = new ScanRegimeRepository();
      _templateRepository = new ResearchTemplateRepository();
      _scopeRepository = new ResearchScopeRepository();

      _research = new ResearchModel(research);
      Initialize();

      CreateReportCommand = new DelegateCommand(CreateReport);
      SaveCommand = new DelegateCommand(Save);
      CancelCommand = new DelegateCommand(Cancel);

      TabHeader = Utils.ToFullName(research.Patient.FirstName, research.Patient.MiddleName, research.Patient.LastName, true);
    }

    #region Properties

    private readonly ResearchModel _research;
    public ResearchModel Research { get { return _research; } }


    private string _tabHeader;
    public string TabHeader
    {
      get { return _tabHeader; }
      set
      {
        _tabHeader = value;
        OnPropertyChanged("TabHeader");
      }
    }

    private string _header;
    public string Header
    {
      get { return _header; }
      set
      {
        _header = value;
        OnPropertyChanged("Header");
      }
    }

    private List<Customer> _customers;
    public List<Customer> Customers
    {
      get { return _customers; }
      set
      {
        _customers = value;
        OnPropertyChanged("Customers");
      }
    }

    private List<Doctor> _doctors;
    public List<Doctor> Doctors
    {
      get { return _doctors; }
      set
      {
        _doctors = value;
        OnPropertyChanged("Doctors");
      }
    }

    private List<ScanRegime> _scanRegimes;
    public List<ScanRegime> ScanRegimes
    {
      get { return _scanRegimes; }
      set
      {
        _scanRegimes = value;
        OnPropertyChanged("ScanRegimes");
      }
    }

    private List<ResearchScope> _researchScopes;
    public List<ResearchScope> ResearchScopes
    {
      get { return _researchScopes; }
      set
      {
        _researchScopes = value;
        OnPropertyChanged("ResearchScopes");
      }
    }

    private List<ResearchTemplate> _researchTemplates;
    public List<ResearchTemplate> ResearchTemplates
    {
      get { return _researchTemplates; }
      set
      {
        _researchTemplates = value;
        OnPropertyChanged("ResearchTemplates");
      }
    }

    private ResearchTemplate _selectedResearchTemplate;
    public ResearchTemplate SelectedResearchTemplate
    {
      get { return _selectedResearchTemplate; }
      set
      {
        var prev = _selectedResearchTemplate;

        if (!Validator.Suspended && !string.IsNullOrWhiteSpace(Research.Content))
        {
          var message = Utils.GetResource("ChangeResearchTemplateMessage");
          var result = RwpMessageBox.Show(message, buttons: MessageBoxButton.YesNo);
          if (result != MessageBoxResult.Yes)
          {
            value = prev;
          }
        }

        _selectedResearchTemplate = value;
        Research.ResearchTemplate = value;
        if (value != null && value != prev)
          Research.Content = value.Content;

        OnPropertyChanged("SelectedResearchTemplate");
      }
    }

    #endregion Properties

    #region SaveCommand

    public ICommand SaveCommand { get; private set; }

    protected virtual void Save()
    {
      try
      {
        if (!Research.Validator.IsValid)
        {
          RwpMessageBox.Show(VALIDATION_ERRORS_MESSAGE);
          return;
        }

        Research.SaveToSource();
        Research.Entity.Id = _researchRepository.Upsert(Research.Entity);
        RwpMessageBox.Show(DATA_SUCCESSFULLY_SAVED_MESSAGE);

        RwpRoot.Execute(Constants.WINDOW_RESEARCH_LIST, Constants.COMMAND_REFRESH);

        Initialize();
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }
    }

    #endregion SaveCommand

    #region CancelCommand

    public ICommand CancelCommand { get; private set; }

    protected virtual void Cancel()
    {
      if (Research.IsDirty)
      {
        var result = RwpMessageBox.Show(CANCEL_PATIENT_RESEARCH_MESSAGE, buttons: MessageBoxButton.YesNo);
        if (result != MessageBoxResult.Yes)
          return;
      }

      Research.ResetFromSource();
      RwpRoot.Close(HostID);
    }

    #endregion CancelCommand

    #region CreateReportCommand

    public ICommand CreateReportCommand { get; private set; }

    private void CreateReport()
    {
      var entity = _researchRepository.GetFullResearch(Research.Entity.Id);
      var repWindow = new PatientResearchReportWindow(entity);
      repWindow.ShowDialog();
    }

    #endregion CreateReportCommand

    #region Overrides

    public override bool OnNavigatedFrom()
    {
      if (Research.IsDirty)
      {
        RwpRoot.Show<RwpTabBase>(this.HostID);
        var result = RwpMessageBox.Show(EDIT_IN_PROGRESS_MESSAGE, buttons: MessageBoxButton.YesNo);
        if (result != MessageBoxResult.Yes)
          return false;
      }

      return base.OnNavigatedFrom();
    }

    #endregion Overrides

    #region Private

    private void Initialize()
    {
      try
      {
        using (Validator.SupressValidation())
        using (Research.Validator.SupressValidation())
        using (Research.SuppressDirty(false))
        {
          // init database data
          InitData();

          // init
          if (Research.Entity.Id == 0)
            InitNewPatientResearch();
          else
            InitViewPatientResearch();
        }
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }
    }

    private void InitData()
    {
      _customers = _customerRepository.GetEntities().ToList();
      _doctors = _doctorRepository.GetEntities().ToList();
      _scanRegimes = _regimeRepository.GetEntities().ToList();
      _researchScopes = _scopeRepository.GetEntities().ToList();
      _researchTemplates = _templateRepository.GetEntities().OrderBy(t => t.Order).ToList();
    }

    private void InitNewPatientResearch()
    {
      Research.Customer = Customers.FirstOrDefault();
      Research.Doctor = Doctors.FirstOrDefault();
      Research.ResearchTemplate = ResearchTemplates.LastOrDefault();
      Research.ExaminationDate = DateTime.Now;
      Research.ResearchDate = DateTime.Now;
      Research.Number = null;
      Research.SliceThickness = ResearchModel.Slices.First();

      var dftRegime = ScanRegimes.FirstOrDefault();
      if (dftRegime != null)
        Research.ScanRegimes.Add(dftRegime);

      Utils.Rejuvenate(ResearchScopes, Research.ResearchScopes);
      Utils.Rejuvenate(ScanRegimes, Research.ScanRegimes);

      Header = string.Format(NEW_PATIENT_RESEARCH_HEADER,
                              Utils.ToFullName(Research.Patient.FirstName, Research.Patient.MiddleName, Research.Patient.LastName));
    }

    private void InitViewPatientResearch()
    {
      Research.Customer = Utils.Rejuvenate(Customers, Research.Customer);
      Research.Doctor = Utils.Rejuvenate(Doctors, Research.Doctor);
      Research.ResearchTemplate = Utils.Rejuvenate(ResearchTemplates, Research.ResearchTemplate);

      Utils.Rejuvenate(ResearchScopes, Research.ResearchScopes);
      Utils.Rejuvenate(ScanRegimes, Research.ScanRegimes);

      Header = string.Format(VIEW_PATIENT_RESEARCH_HEADER,
                              Utils.ToFullName(Research.Patient.FirstName, Research.Patient.MiddleName, Research.Patient.LastName),
                              Research.Number,
                              Research.ResearchDate);
    }

    #endregion Private
  }
}