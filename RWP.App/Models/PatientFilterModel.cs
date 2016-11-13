using RWP.App.Infrastructure.Mvvm;
using RWP.Data.Filters;

namespace RWP.App.Models
{
  public class PatientFilterModel : NotifierBase
  {
    #region Properties

    private bool _isDirty;
    public bool IsDirty
    {
      get { return _isDirty; }
      set
      {
        _isDirty = value;
        OnPropertyChanged("IsDirty");
      }
    }

    
    private int? _id;
    public int? Id
    {
      get { return _id; }
      set
      {
        if (_id == value) return;
        _id = value;
        IsDirty = true;
        OnPropertyChanged("Id");
      }
    }

    private string _name;
    public string Name
    {
      get { return _name; }
      set
      {
        if (_name == value) return;
        _name = value;
        IsDirty = true;
        OnPropertyChanged("Name");
      }
    }

    private string _dob;
    public string DOB
    {
      get { return _dob; }
      set
      {
        if (_dob == value) return;
        _dob = value;
        IsDirty = true;
        OnPropertyChanged("DOB");
      }
    }

    private string _examinationDate;
    public string ExaminationDate
    {
      get { return _examinationDate; }
      set
      {
        if (_examinationDate == value) return;
        _examinationDate = value;
        IsDirty = true;
        OnPropertyChanged("ExaminationDate");
      }
    }

    private string _researchDate;
    public string ResearchDate
    {
      get { return _researchDate; }
      set
      {
        if (_researchDate == value) return;
        _researchDate = value;
        IsDirty = true;
        OnPropertyChanged("ResearchDate");
      }
    }

    private string _conclusion;
    public string Conclusion
    {
      get { return _conclusion; }
      set
      {
        if (_conclusion == value) return;
        _conclusion = value;
        IsDirty = true;
        OnPropertyChanged("Conclusion");
      }
    }

    public int Skip { get; set; }

    public int Take { get; set; }

    #endregion Properties

    public PatientFilter GetFilter()
    {
      return new PatientFilter
      {
        Id = Id,
        Name = Name,
        DOB = DOB,
        ExaminationDate = ExaminationDate,
        ResearchDate = ResearchDate,
        Conclusion = Conclusion,
        Skip = Skip,
        Take = Take
      };
    }

    public void Clear()
    {
      Id = null;
      Name = null;
      DOB = null;
      ExaminationDate = null;
      ResearchDate = null;
      Conclusion = null;
      IsDirty = false;
    }
  }
}