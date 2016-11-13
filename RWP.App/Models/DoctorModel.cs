using System.Windows.Media.Imaging;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.Data;

namespace RWP.App.Models
{
  public class DoctorModel : ModelBase<Doctor>, INamed
  {
    public DoctorModel(Doctor entity)
      : base(entity)
    {
      Validate();
    }

    #region Properties

    public string FullName
    {
      get { return Utils.ToFullName(FirstName, MiddleName, LastName); }
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
        OnPropertyChanged("FullName");
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
        OnPropertyChanged("FullName");
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
        OnPropertyChanged("FullName");
      }
    }

    private string _position;
    public string Position
    {
      get { return _position; }
      set
      {
        _position = value;
        Validate("Position");
        OnPropertyChanged("Position");
      }
    }

    private BitmapImage _printBitmap;
    public BitmapImage PrintBitmap
    {
      get { return _printBitmap; }
      set
      {
        _printBitmap = value;
        OnPropertyChanged("PrintBitmap");
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

    protected override void ResetFromSourceImpl()
    {
      FirstName = Entity.FirstName;
      MiddleName = Entity.MiddleName;
      LastName = Entity.LastName;
      Position = Entity.Position;
      PrintBitmap = Entity.Print != null ? Utils.DecodeImage(Entity.Print.ToArray()) : null;
      Note = Entity.Note;
    }

    protected override void SaveToSourceImpl()
    {
      Entity.FirstName = FirstName;
      Entity.MiddleName = MiddleName;
      Entity.LastName = LastName;
      Entity.Position = Position;
      Entity.Print = PrintBitmap != null ? Utils.EncodeImage(PrintBitmap) : null;
      Entity.Note = Note;
    }

    protected override void ConfigureValidation()
    {
      Validator.AddRule("FirstName", () => !string.IsNullOrWhiteSpace(FirstName), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("LastName", () => !string.IsNullOrWhiteSpace(LastName), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("Position", () => !string.IsNullOrWhiteSpace(Position), FIELD_ISREQUIRED_ERROR);
    }

    #endregion Overrides
  }
}