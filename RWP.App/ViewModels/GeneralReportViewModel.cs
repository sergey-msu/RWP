using System;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.Models;
using RWP.Data;
using RWP.App.Reporting.Reports;
using RWP.Data.Repositories;

namespace RWP.App.ViewModels
{
  public class GeneralReportViewModel : ViewModelBase
  {
    private static readonly string REPORT_FILE_NAME_FORMAT = Utils.GetResource("GeneralReportFilenameFormat");
    private static readonly string USE_CONTRAST_REPORT_CHAR = Utils.GetResource("UseContrastReportChar");
    private const string REPORT_FOLDER_FORMAT = @"reports\{0}";

    private ReportSettingsModel _model;
    private List<ScanRegime> _scanRegimes;
    private List<ResearchScope> _researchScopes;

    private readonly ReportSettingsRepository _repository;
    private readonly DoctorRepository _doctorRepository;
    private readonly PatientRepository _patientRepository;
    private readonly MedicalResearchRepository _researchRepository;
    private readonly ScanRegimeRepository _regimeRepository;
    private readonly ResearchScopeRepository _scopeRepository;

    public GeneralReportViewModel()
    {
      _repository = new ReportSettingsRepository();
      _doctorRepository = new DoctorRepository();
      _patientRepository = new PatientRepository();
      _researchRepository = new MedicalResearchRepository();
      _regimeRepository = new ScanRegimeRepository();
      _scopeRepository = new ResearchScopeRepository();

      CreateReportCommand = new DelegateCommand(CreateReport);

      InitData();
      UpdateSettings();
    }

    #region Properties

    private IEnumerable<Doctor> _doctors;
    public IEnumerable<Doctor> Doctors
    {
      get { return _doctors; }
      set
      {
        _doctors = value;
        OnPropertyChanged("Doctors");
      }
    }

    private Doctor _selectedDoctor;
    public Doctor SelectedDoctor
    {
      get { return _selectedDoctor; }
      set
      {
        _selectedDoctor = value;
        OnPropertyChanged("SelectedDoctor");
      }
    }

    private DateTime m_StartDate;
    public DateTime StartDate
    {
      get { return m_StartDate; }
      set
      {
        m_StartDate = value;
        OnPropertyChanged("StartDate");
      }
    }

    private DateTime m_EndDate;
    public DateTime EndDate
    {
      get { return m_EndDate; }
      set
      {
        m_EndDate = value;
        OnPropertyChanged("EndDate");
      }
    }

    public string SelectedFont
    {
      get { return _model.GetSetting<string>(Constants.FONT_FAMILY_SETTING); }
      set
      {
        _model.SetSetting(Constants.FONT_FAMILY_SETTING, value);
        OnPropertyChanged("SelectedFont");
      }
    }

    public int SelectedFontSize
    {
      get { return _model.GetSetting<int>(Constants.FONT_SIZE_SETTING); }
      set
      {
        _model.SetSetting(Constants.FONT_SIZE_SETTING, value);
        OnPropertyChanged("SelectedFontSize");
      }
    }

    public int SelectedLineMargin
    {
      get { return _model.GetSetting<int>(Constants.LINE_MARGIN_SETTING); }
      set
      {
        _model.SetSetting(Constants.LINE_MARGIN_SETTING, value);
        OnPropertyChanged("SelectedLineMargin");
      }
    }

    public int SelectedDocMargin
    {
      get { return _model.GetSetting<int>(Constants.DOC_MARGIN_SETTING); }
      set
      {
        _model.SetSetting(Constants.DOC_MARGIN_SETTING, value);
        OnPropertyChanged("SelectedDocMargin");
      }
    }

    #endregion Properties

    #region CreateReportCommand

    public ICommand CreateReportCommand { get; private set; }

    private void CreateReport()
    {
      try
      {
        var items = GetReseachItems().OrderBy(i => i.ExaminationDate);
        var context = new GeneralReportContext(items)
        {
          StartDate = StartDate,
          EndDate = EndDate,
          Doctor = SelectedDoctor
        };

        var report = new GeneralPDFReport(context, _model);
        var path = CreateReportPath();
        report.Create(path);

        var info = new ProcessStartInfo();
        info.FileName = "explorer";
        info.Arguments = string.Format("/e, /select, \"{0}\"", path);
        Process.Start(info);
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }
    }

    #endregion CreateReportCommand

    private void UpdateSettings()
    {
      var entity = _repository.GetByUQ(Constants.PATIENT_RESEARCH_REPORT_NAME, Constants.PDF_REPORT_TYPE, 0) ??
                    new ReportSetting
                    {
                      IdMedicalResearch = 0,
                      Name = Constants.PATIENT_RESEARCH_REPORT_NAME,
                      Type = Constants.PDF_REPORT_TYPE
                    };

      _model = new ReportSettingsModel(entity);

      OnPropertyChanged("SelectedFont");
      OnPropertyChanged("SelectedFontSize");
      OnPropertyChanged("SelectedLineMargin");
      OnPropertyChanged("SelectedDocMargin");
    }

    private string CreateReportPath()
    {
      var ext = "pdf";
      var fileName = string.Format(REPORT_FILE_NAME_FORMAT, StartDate, EndDate, ext);

      var file = string.Format(REPORT_FOLDER_FORMAT, fileName);
      var dir = Path.GetDirectoryName(file);
      if (!Directory.Exists(dir))
        Directory.CreateDirectory(dir);

      return file;
    }

    private void InitData()
    {
      EndDate = DateTime.Now;
      StartDate = EndDate.AddMonths(-1);

      _doctors = _doctorRepository.GetEntities().ToList();
      _scanRegimes = _regimeRepository.GetEntities().ToList();
      _researchScopes = _scopeRepository.GetEntities().ToList();

      SelectedDoctor = _doctors.FirstOrDefault();
    }

    private IEnumerable<GeneralReportItemModel> GetReseachItems()
    {
      var result = new List<GeneralReportItemModel>();

      var patients = _patientRepository.GetPatientsWithResearches(StartDate, EndDate);
      foreach (var patient in patients)
      {
        foreach (var research in patient.MedicalResearches)
        {
          if (research.ExaminationDate.IsLessThan(StartDate) || EndDate.IsLessThan(research.ExaminationDate))
            continue;

          var scopes = _researchScopes.Where(rs => research.MedicalResearchScopes.Any(s => s.IdResearchScope == rs.Id))
                                      .Select(rs => rs.Name.ToLowerInvariant())
                                      .ToList();
          if (research.UseContrast)
            scopes.Add(USE_CONTRAST_REPORT_CHAR.ToLowerInvariant());

          var item = new GeneralReportItemModel
          {
            PatientName = Utils.ToFullName(patient.FirstName, patient.MiddleName, patient.LastName, true),
            ExaminationDate = research.ExaminationDate,
            ResearchNumber = research.Number,
            ResearchScopes = string.Join(Utils.PLUS_SEPARATOR, scopes),
            ScopesNum = scopes.Count
          };

          result.Add(item);
        }
      }

      return result;
    }
  }
}
