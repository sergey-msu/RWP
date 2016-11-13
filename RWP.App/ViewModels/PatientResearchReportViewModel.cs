using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Windows.Input;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Enums;
using RWP.App.Infrastructure.Mvvm;
using RWP.Data;
using RWP.Data.Repositories;
using RWP.App.Reporting.Reports;
using RWP.App.Models;

namespace RWP.App.ViewModels
{
  public class PatientResearchReportViewModel : ViewModelBase
  {
    private static readonly string REPORT_FILE_NAME_FORMAT = Utils.GetResource("ReportFilenameFormat");
    private const string REPORT_FOLDER_FORMAT = @"reports\{0}";

    private ReportSettingsModel _model;
    private readonly MedicalResearch _research;
    private readonly ReportSettingsRepository _repository;

    public PatientResearchReportViewModel(MedicalResearch research)
    {
      if (research == null)
        throw new RwpException("CreateReportViewModel.ctor(research=null)");

      _research = research;
      _repository = new ReportSettingsRepository();

      CreateReportCommand = new DelegateCommand(CreateReport);

      UpdateSettings();
    }

    #region Properties

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
        if (_model.IsDefault())
        {
          _repository.Delete(_model.Entity);
        }
        else
        {
          _model.SaveToSource();
          _model.Entity.Id = _repository.Upsert(_model.Entity);
        }
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }

      try
      {
        var report = new PatientResearchPDFReport(_research, _model);
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
      var entity = _repository.GetByUQ(Constants.PATIENT_RESEARCH_REPORT_NAME, Constants.PDF_REPORT_TYPE, _research.Id) ??
                    new ReportSetting
                    {
                      IdMedicalResearch = _research.Id,
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
      var name = Utils.ToFullName(_research.Patient.FirstName, _research.Patient.MiddleName, _research.Patient.LastName);
      var normalizedName = Utils.Normalize(name);

      var scopes = string.Join(", ", _research.MedicalResearchScopes.Select(rs => rs.ResearchScope.Name));
      var normalizedScopes = Utils.Normalize(scopes);

      var ext = "pdf";
      var fileName = string.Format(REPORT_FILE_NAME_FORMAT, normalizedName, normalizedScopes, _research.ExaminationDate, ext);

      var folder = string.Format(REPORT_FOLDER_FORMAT, normalizedName);
      if (!Directory.Exists(folder))
        Directory.CreateDirectory(folder);

      return Path.Combine(folder, fileName);
    }
  }
}
