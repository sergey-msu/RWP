using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.Data;

namespace RWP.App.Models
{
  public class CustomerModel : ModelBase<Customer>, INamed
  {
    public CustomerModel(Customer entity)
      : base(entity)
    {
      Validate();
    }

    #region Properties

    private string _name;
    public string Name
    {
      get { return _name; }
      set
      {
        _name = value;
        Validate("Name");
        OnPropertyChanged("Name");
      }
    }
    
    private string _address;
    public string Address
    {
      get { return _address; }
      set
      {
        _address = value;
        Validate("Address");
        OnPropertyChanged("Address");
      }
    }
    
    private string _researchPlace;
    public string ResearchPlace
    {
      get { return _researchPlace; }
      set
      {
        _researchPlace = value;
        OnPropertyChanged("ResearchPlace");
      }
    }
    
    private string _contactName;
    public string ContactName
    {
      get { return _contactName; }
      set
      {
        _contactName = value;
        OnPropertyChanged("ContactName");
      }
    }
    
    private string _contactEMail;
    public string ContactEMail
    {
      get { return _contactEMail; }
      set
      {
        _contactEMail = value;
        OnPropertyChanged("ContactEMail");
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

    #endregion Properties

    #region Overrides

    protected override void ResetFromSourceImpl()
    {
      Name = Entity.Name;
      Address = Entity.Address;
      ResearchPlace = Entity.ResearchPlace;
      ContactName = Entity.ContactName;
      ContactEMail = Entity.ContactEMail;
      Note = Entity.Note;
    }

    protected override void SaveToSourceImpl()
    {
      Entity.Name = Name;
      Entity.Address = Address;
      Entity.ResearchPlace = ResearchPlace;
      Entity.ContactName = ContactName;
      Entity.ContactEMail = ContactEMail;
      Entity.Note = Note;
    }

    protected override void ConfigureValidation()
    {
      Validator.AddRule("Name", () => !string.IsNullOrWhiteSpace(Name), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("Address", () => !string.IsNullOrWhiteSpace(Address), FIELD_ISREQUIRED_ERROR);
    }

    #endregion Overrides
  }
}