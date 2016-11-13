using System;
using System.Linq;
using System.ComponentModel;

namespace RWP.App.Infrastructure.Mvvm
{
  public abstract class ValidatableBase : NotifierBase, IDataErrorInfo
  {
    protected ValidatableBase()
    {
      _validator = new Validator();
      ConfigureValidation();
    }

    private readonly Validator _validator;
    public Validator Validator
    {
      get { return _validator; }
    }
    
    public string this[string propertyName]
    {
      get
      {
        var invalids = Validator.Results.Where(r => !r.IsValid && r.PropertyName == propertyName);
        if (invalids.Any())
        {
          var errors = invalids.Select(r => r.Error).Where(e => e != null);
          var error = string.Join(Environment.NewLine, errors);
          return error;
        }

        return null;
      }
    }

    public string Error
    {
      get
      {
        var errors = Validator.Results
                              .Where(r => !r.IsValid)
                              .Select(r => r.Error)
                              .Where(e => e != null);
        var error = string.Join(Environment.NewLine, errors);

        return error;
      }
    }

    protected void Validate()
    {
      Validate(null);
    }

    protected void Validate(params string[] propertyNames)
    {
      Validator.Validate(propertyNames);
    }

    protected virtual void ConfigureValidation()
    {
    }
  }
}
