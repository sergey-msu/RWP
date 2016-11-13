using System.Linq;
using System.Data.Linq.SqlClient;

namespace RWP.Data.Filters
{
  /// <summary>
  /// Filter Customer by its property values
  /// </summary>
  public class CustomerNameFilter : FilterBase<Customer>
  {
    #region Properties

    /// <summary>
    /// Customer's Name pattern
    /// </summary>
    public string Name { get; set; }

    #endregion Properties

    protected override IQueryable<Customer> DoApply(IQueryable<Customer> source)
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