using System;
using System.Linq;
using System.Collections.Generic;

namespace RWP.App.Infrastructure.Mvvm
{
  public class Validator : NotifierBase
  {
    #region Nested

    private interface IValidationRule
    {
      string PropertyName { get; }

      string Error { get; }

      bool Validate();
    }

    public class ValidationResult
    {
      private readonly string _propertyName;

      public ValidationResult(bool isValid, string propertyName, string error)
      {
        _propertyName = propertyName;
        IsValid = isValid;
        Error = error;
      }

      public string PropertyName { get { return _propertyName; } }
      public bool IsValid { get; internal set; }
      public string Error { get; internal set; }
    }

    private class ValidationRule : IValidationRule
    {
      public ValidationRule(string propertyName, Func<bool> rule, string error)
      {
        if (propertyName == null)
          throw new ArgumentNullException("propertyName");
        if (rule == null)
          throw new ArgumentNullException("rule");

        PropertyName = propertyName;
        Error = error;
        Rule = rule;
      }

      public readonly Func<bool> Rule;
      public string PropertyName { get; private set; }
      public string Error { get; private set; }

      public bool Validate()
      {
        return Rule();
      }
    }

    public class ValidationSupressor : IDisposable
    {
      private readonly Validator _validator;
      private readonly bool _validateOnComplete;

      public ValidationSupressor(Validator validator, bool validateOnComplete)
      {
        _validator = validator;
        _validateOnComplete = validateOnComplete;
        _validator.Suspended = true;
      }

      public void Dispose()
      {
        _validator.Suspended = false;
        if (_validateOnComplete)
          _validator.Validate();
      }
    }

    #endregion Nested

    private readonly List<IValidationRule> _rules;
    private readonly List<ValidationResult> _results;

    public Validator()
    {
      _rules = new List<IValidationRule>();
      _results = new List<ValidationResult>();
    }

    #region Properties

    private bool _isValid;
    public bool IsValid
    {
      get { return _isValid; }
      set
      {
        _isValid = value;
        OnPropertyChanged("IsValid");
      }
    }

    private bool _suspended;
    public bool Suspended
    {
      get { return _suspended; }
      set
      {
        _suspended = value;
        OnPropertyChanged("Suspended");
      }
    }

    public List<ValidationResult> Results { get { return _results; } }

    #endregion Properties

    #region Public

    public void AddRule(string propertyName, Func<bool> rule, string error = null)
    {
      if (_rules.Any(r => r.PropertyName == propertyName))
      {
        var message = string.Format("Rule for property '{0}' has already been added", propertyName);
        throw new InvalidOperationException(message);
      }

      var validationRule = new ValidationRule(propertyName, rule, error);
      _rules.Add(validationRule);

      var validationResult = new ValidationResult(true, propertyName, null);
      _results.Add(validationResult);
    }

    public void Validate()
    {
      Validate(null);
    }

    public void Validate(params string[] propertyNames)
    {
      if (this.Suspended)
        return;

      propertyNames = propertyNames ?? _rules.Select(p => p.PropertyName).ToArray();
      foreach (var propertyName in propertyNames)
      {
        var rule = _rules.First(r => r.PropertyName == propertyName);
        var result = _results.First(r => r.PropertyName == propertyName);

        var isValid = rule.Validate();
        if (isValid)
        {
          result.IsValid = true;
          result.Error = null;
        }
        else
        {
          result.IsValid = false;
          result.Error = rule.Error;
        }
      }

      IsValid = Results.All(r => r.IsValid);
    }

    public IDisposable SupressValidation(bool validateOnComplete = true)
    {
      return new ValidationSupressor(this, validateOnComplete);
    }

    #endregion Public
  }
}
