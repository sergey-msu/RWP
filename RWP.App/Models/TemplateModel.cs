using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.Data;

namespace RWP.App.Models
{
  public class TemplateModel : ModelBase<ResearchTemplate>, INamed
  {
    public TemplateModel(ResearchTemplate entity)
      : base(entity)
    {
      Validate();
    }

    #region Properties
    
    private bool _isSystem;
    public bool IsSystem
    {
      get { return _isSystem; }
    }

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

    private string _content;
    public string Content
    {
      get { return _content; }
      set
      {
        _content = value;
        OnPropertyChanged("Content");
      }
    }
    
    private int? _order;
    public int? Order
    {
      get { return _order; }
      set
      {
        _order = value;
        OnPropertyChanged("Order");
      }
    }

    #endregion Properties

    #region Overrides

    protected override void ResetFromSourceImpl()
    {
      Name = Entity.Name;
      Content = Entity.Content;
      Order = Entity.Order;
      _isSystem = Entity.IsSystem;
    }

    protected override void SaveToSourceImpl()
    {
      Entity.Content = Content;
      Entity.Name = Name;
      Entity.Order = Order;
    }

    protected override void ConfigureValidation()
    {
      Validator.AddRule("Name", () => !string.IsNullOrWhiteSpace(Name), FIELD_ISREQUIRED_ERROR);
    }

    #endregion Overrides
  }
}