using System;

namespace RWP.App.Infrastructure.Attributes
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public class EnumDescriptionAttribute : Attribute
  {
    public EnumDescriptionAttribute(string description)
    {
      _description = description;
    }

    private readonly string _description;
    public string Description
    {
      get { return _description; }
    }
  }
}
