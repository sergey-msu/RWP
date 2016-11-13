using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RWP.App.Infrastructure.Mvvm;
using RWP.Data;
using RWP.App.Infrastructure;
using System.Globalization;

namespace RWP.App.Models
{
  public class ResearchModel : ModelBase<MedicalResearch>
  {
    private const string SLICE1 = "2,5; 1,25; 0,625";
    private const string SLICE2 = "5; 1,25; 0,625";
    private static readonly List<string> SLICES = new List<string> { SLICE1, SLICE2 };

    public ResearchModel(MedicalResearch entity)
      : base(entity)
    {
      ResearchScopes.CollectionChanged += (o, e) =>
      {
        Validate("ResearchScopes");
        OnPropertyChanged("ResearchScopes");
      };
      ScanRegimes.CollectionChanged += (o, e) =>
      {
        Validate("ScanRegimes");
        OnPropertyChanged("ScanRegimes");
      };

      _patient = new PatientModel(Entity.Patient);

      Validate();
    }

    #region Properties

    public static List<string> Slices { get { return SLICES; } }

    private Customer _customer;
    public Customer Customer
    {
      get { return _customer; }
      set
      {
        _customer = value;
        Validate("Customer");
        OnPropertyChanged("Customer");
      }
    }

    private Doctor _doctor;
    public Doctor Doctor
    {
      get { return _doctor; }
      set
      {
        _doctor = value;
        Validate("Doctor");
        OnPropertyChanged("Doctor");
      }
    }

    private readonly PatientModel _patient;
    public PatientModel Patient
    {
      get { return _patient; }
    }

    private DateTime? _examinationDate;
    public DateTime? ExaminationDate
    {
      get { return _examinationDate; }
      set
      {
        _examinationDate = value;
        Validate("ExaminationDate");
        OnPropertyChanged("ExaminationDate");
      }
    }

    private DateTime? _researchDate;
    public DateTime? ResearchDate
    {
      get { return _researchDate; }
      set
      {
        _researchDate = value;
        Validate("ResearchDate");
        OnPropertyChanged("ResearchDate");
      }
    }

    private string _number;
    public string Number
    {
      get { return _number; }
      set
      {
        _number = value;
        Validate("Number");
        OnPropertyChanged("Number");
      }
    }

    private string _sliceThickness;
    public string SliceThickness
    {
      get { return _sliceThickness; }
      set
      {
        _sliceThickness = value;
        Validate("SliceThickness");
        OnPropertyChanged("SliceThickness");
      }
    }

    private bool? _useContrast;
    public bool? UseContrast
    {
      get { return _useContrast; }
      set
      {
        _useContrast = value;
        Validate("UseContrast");
        OnPropertyChanged("UseContrast");
      }
    }

    private string _dose;
    public string Dose
    {
      get { return _dose; }
      set
      {
        _dose = value;
        Validate("Dose");
        OnPropertyChanged("Dose");
      }
    }

    private ResearchTemplate _researchTemplate;
    public ResearchTemplate ResearchTemplate
    {
      get { return _researchTemplate; }
      set
      {
        _researchTemplate = value;
        OnPropertyChanged("ResearchTemplate");
      }
    }

    private string _content;
    public string Content
    {
      get { return _content; }
      set
      {
        _content = value;
        Validate("Content");
        OnPropertyChanged("Content");
      }
    }

    private string _conclusion;
    public string Conclusion
    {
      get { return _conclusion; }
      set
      {
        _conclusion = value;
        Validate("Conclusion");
        OnPropertyChanged("Conclusion");
      }
    }

    private ObservableCollection<ScanRegime> _scanRegimes;
    public ObservableCollection<ScanRegime> ScanRegimes
    {
      get
      {
        if (_scanRegimes == null)
          _scanRegimes = new ObservableCollection<ScanRegime>();

        return _scanRegimes;
      }
    }

    private ObservableCollection<ResearchScope> _researchScopes;
    public ObservableCollection<ResearchScope> ResearchScopes
    {
      get
      {
        if (_researchScopes == null)
          _researchScopes = new ObservableCollection<ResearchScope>();

        return _researchScopes;
      }
    }

    private ObservableCollection<Attachment> _attachments;
    public ObservableCollection<Attachment> Attachments
    {
      get
      {
        if (_attachments == null)
          _attachments = new ObservableCollection<Attachment>();

        return _attachments;
      }
    }

    #endregion Properties

    #region Overrides

    protected override void ConfigureValidation()
    {
      Validator.AddRule("Customer", () => Customer != null, FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("Doctor", () => Doctor != null, FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("Patient", () => Patient != null, FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("ExaminationDate", () => ExaminationDate.HasValue && ExaminationDate.Value.Year > Constants.MIN_YEAR, FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("ResearchDate", () => ResearchDate.HasValue && ResearchDate.Value.Year > Constants.MIN_YEAR, FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("Number", () => !string.IsNullOrWhiteSpace(Number), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("SliceThickness", () => Slices.Contains(SliceThickness), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("UseContrast", () => UseContrast.HasValue, FIELD_ISREQUIRED_ERROR);
      double dose;
      Validator.AddRule("Dose", () => Dose != null && !Dose.Contains(',') && double.TryParse(Dose, NumberStyles.Number, CultureInfo.InvariantCulture, out dose), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("Content", () => !string.IsNullOrWhiteSpace(Content), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("ResearchScopes", () => ResearchScopes != null && ResearchScopes.Any(), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("ScanRegimes", () => ScanRegimes != null && ScanRegimes.Any(), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("Conclusion", () => !string.IsNullOrWhiteSpace(Conclusion), FIELD_ISREQUIRED_ERROR);
    }

    protected override void ResetFromSourceImpl()
    {
      Customer = Entity.Customer;
      Doctor = Entity.Doctor;
      Number = Entity.Number;
      ResearchDate = Entity.ResearchDate;
      ExaminationDate = Entity.ExaminationDate;
      SliceThickness = Entity.SliceThickness;
      UseContrast = Entity.UseContrast;
      Dose = Entity.Dose.ToString(CultureInfo.InvariantCulture);
      ResearchTemplate = Entity.ResearchTemplate;
      Content = Entity.Content;
      Conclusion = Entity.Conclusion;

      var scopes = new ObservableCollection<ResearchScope>(Entity.MedicalResearchScopes.Select(rs => rs.ResearchScope));
      ResearchScopes.Clear();
      foreach (var scope in scopes)
      {
        ResearchScopes.Add(scope);
      }

      var regimes = new ObservableCollection<ScanRegime>(Entity.MedicalScanRegimes.Select(rs => rs.ScanRegime));
      ScanRegimes.Clear();
      foreach (var regime in regimes)
      {
        ScanRegimes.Add(regime);
      }

      var attachments = new ObservableCollection<Attachment>(Entity.MedicalResearchAttachments.Select(ra => ra.Attachment));
      Attachments.Clear();
      foreach (var attachment in attachments)
      {
        Attachments.Add(attachment);
      }
    }

    protected override void SaveToSourceImpl()
    {
      Entity.Customer = Customer;
      Entity.Doctor = Doctor;
      Entity.Number = Number;
      Entity.ResearchDate = ResearchDate.Value;
      Entity.ExaminationDate = ExaminationDate.Value;
      Entity.SliceThickness = SliceThickness;
      Entity.UseContrast = UseContrast.Value;
      double dose;
      if (double.TryParse(Dose, NumberStyles.Number, CultureInfo.InvariantCulture, out dose))
      Entity.Dose = dose;
      Entity.ResearchTemplate = ResearchTemplate;
      Entity.Content = Content;
      Entity.Conclusion = Conclusion;

      Entity.MedicalResearchScopes.Clear();
      foreach (var scope in ResearchScopes)
      {
        var association = new MedicalResearchScope { MedicalResearch = this.Entity, ResearchScope = scope };
        Entity.MedicalResearchScopes.Add(association);
      }

      Entity.MedicalScanRegimes.Clear();
      foreach (var regime in ScanRegimes)
      {
        var association = new MedicalScanRegime { MedicalResearch = this.Entity, ScanRegime = regime };
        Entity.MedicalScanRegimes.Add(association);
      }

      Entity.MedicalResearchAttachments.Clear();
      foreach (var attachment in Attachments)
      {
        var association = new MedicalResearchAttachment { MedicalResearch = this.Entity, Attachment = attachment };
        Entity.MedicalResearchAttachments.Add(association);
      }
    }

    #endregion Overrides
  }
}