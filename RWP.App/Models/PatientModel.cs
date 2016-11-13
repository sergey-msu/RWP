using System;
using RWP.App.Infrastructure.Enums;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.Infrastructure;
using RWP.Data;

namespace RWP.App.Models
{
  public class PatientModel : ModelBase<Patient>, INamed
  {
    public PatientModel(Patient entity)
      : base(entity)
    {
      Validate();
    }

    #region Properties
    
    private int? _id;
    public int? Id
    {
      get { return _id; }
    }

    private string _firstName;
    public string FirstName
    {
      get { return _firstName; }
      set
      {
        _firstName = value;
        Validate("FirstName");
        OnPropertyChanged("FirstName");
      }
    }

    private string _middleName;
    public string MiddleName
    {
      get { return _middleName; }
      set
      {
        _middleName = value;
        OnPropertyChanged("MiddleName");
      }
    }

    private string _lastName;
    public string LastName
    {
      get { return _lastName; }
      set
      {
        _lastName = value;
        Validate("LastName");
        OnPropertyChanged("LastName");
      }
    }

    private DateTime? _dob;
    public DateTime? DOB
    {
      get { return _dob; }
      set
      {
        _dob = value;
        Validate("DOB");
        OnPropertyChanged("DOB");
      }
    }

    private Sex _sex;
    public Sex Sex
    {
      get { return _sex; }
      set
      {
        _sex = value;
        OnPropertyChanged("Sex");
      }
    }

    private string _note;
    public string Note
    {
      get { return _note; }
      set
      {
        _note = value;
        OnPropertyChanged("Note");
      }
    }
    
    public string Name { get { return Utils.ToFullName(FirstName, MiddleName, LastName, true); } }

    #endregion Properties

    #region Overrides

    protected override void ConfigureValidation()
    {
      Validator.AddRule("FirstName", () => !string.IsNullOrWhiteSpace(FirstName), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("LastName", () => !string.IsNullOrWhiteSpace(LastName), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("DOB", () => DOB.HasValue && DOB.Value.Year > Constants.MIN_YEAR, FIELD_ISREQUIRED_ERROR);
    }

    protected override void ResetFromSourceImpl()
    {
      _id = Entity.Id;
      OnPropertyChanged("Id");

      FirstName = Entity.FirstName;
      MiddleName = Entity.MiddleName;
      LastName = Entity.LastName;
      DOB = Entity.DOB;
      Sex = Entity.Sex ? Sex.Male : Sex.Female;
      Note = Entity.Note;
    }

    protected override void SaveToSourceImpl()
    {
      Entity.FirstName = FirstName;
      Entity.MiddleName = MiddleName;
      Entity.LastName = LastName;
      Entity.DOB = DOB.Value;
      Entity.Sex = Sex == Sex.Male;
      Entity.Note = Note;
    }

    #endregion Overrides
  }
}