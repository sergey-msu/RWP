using System.Linq;
using System.Data.Linq.SqlClient;

namespace RWP.Data.Filters
{
  /// <summary>
  /// Filter Doctor by its property values
  /// </summary>
  public class DoctorNameFilter : FilterBase<Doctor>
  {
    #region Properties

    /// <summary>
    /// Doctor Name pattern
    /// </summary>
    public string Name { get; set; }

    #endregion Properties

    protected override IQueryable<Doctor> DoApply(IQueryable<Doctor> source)
    {
      var result = source;

      if (!string.IsNullOrWhiteSpace(Name))
      {
        result = result.Where(p => SqlMethods.Like(((p.FirstName ?? string.Empty) + (p.MiddleName ?? string.Empty) + (p.LastName ?? string.Empty)),
                                                    LIKE + Name + LIKE));
      }

      return result;
    }
  }
}