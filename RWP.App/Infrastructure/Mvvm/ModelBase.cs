using System;

namespace RWP.App.Infrastructure.Mvvm
{
  public abstract class ModelBase<TEntity> : ValidatableBase
  {
    public class DirtySupressor : IDisposable
    {
      private readonly ModelBase<TEntity> _source;
      private readonly bool _markOnComplete;

      public DirtySupressor(ModelBase<TEntity> source, bool markOnComplete)
      {
        _source = source;
        _markOnComplete = markOnComplete;
        _source._dirtySupressed = true;
      }

      public void Dispose()
      {
        _source._dirtySupressed = false;
        _source.IsDirty = _markOnComplete;
      }
    }

    protected readonly string FIELD_ISREQUIRED_ERROR = Utils.GetResource("FieldIsRequiredError");

    private bool _dirtySupressed = false;

    protected ModelBase(TEntity entity)
    {
      _entity = entity;
      ResetFromSource();
    }

    private readonly TEntity _entity;
    public TEntity Entity
    {
      get { return _entity; }
    }

    private bool _isDirty;
    public bool IsDirty
    {
      get { return _isDirty; }
      protected set
      {
        _isDirty = value;
        OnPropertyChanged("IsDirty");
      }
    }

    public void ResetFromSource()
    {
      using (Validator.SupressValidation())
      {
        ResetFromSourceImpl();
      }

      IsDirty = false;
    }

    public void SaveToSource()
    {
      SaveToSourceImpl();
      IsDirty = false;
    }

    public DirtySupressor SuppressDirty(bool markOnComplete = true)
    {
      return new DirtySupressor(this, markOnComplete);
    }

    protected abstract void ResetFromSourceImpl();

    protected abstract void SaveToSourceImpl();

    protected override void OnPropertyChanged(string propertyName)
    {
      if (propertyName != "IsDirty")
      {
        if (!_dirtySupressed)
          IsDirty = true;
      }

      base.OnPropertyChanged(propertyName);
    }
  }
}
