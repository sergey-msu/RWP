using System;

namespace RWP.App.Infrastructure.Attributes
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public class EnumDescriptionKeyAttribute : Attribute
  {
    public EnumDescriptionKeyAttribute(string descriptionKey)
    {
      _descriptionKey = descriptionKey;
    }

    private readonly string _descriptionKey;
    public string DescriptionKey
    {
      get { return _descriptionKey; }
    }
  }
}
