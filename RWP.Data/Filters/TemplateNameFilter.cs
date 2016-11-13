using System.Linq;
using System.Data.Linq.SqlClient;

namespace RWP.Data.Filters
{
  /// <summary>
  /// Filter Template by its property values
  /// </summary>
  public class TemplateNameFilter : FilterBase<ResearchTemplate>
  {
    #region Properties

    /// <summary>
    /// Tempalte Name pattern
    /// </summary>
    public string Name { get; set; }

    #endregion Properties

    protected override IQueryable<ResearchTemplate> DoApply(IQueryable<ResearchTemplate> source)
    {
      var result = source;

      if (!string.IsNullOrWhiteSpace(Name))
      {
        result = result.Where(p => SqlMethods.Like(p.Name ?? string.Empty,
                                                    LIKE + Name + LIKE));
      }

      return result;
    }
  }
}