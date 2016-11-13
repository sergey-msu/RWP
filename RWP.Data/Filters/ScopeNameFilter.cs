using System.Linq;
using System.Data.Linq.SqlClient;

namespace RWP.Data.Filters
{
  /// <summary>
  /// Filter Scope by its property values
  /// </summary>
  public class ScopeNameFilter : FilterBase<ResearchScope>
  {
    #region Properties

    /// <summary>
    /// Scope name pattern
    /// </summary>
    public string Name { get; set; }

    #endregion Properties

    protected override IQueryable<ResearchScope> DoApply(IQueryable<ResearchScope> source)
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