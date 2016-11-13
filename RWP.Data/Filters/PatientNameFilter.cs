using System.Linq;
using System.Data.Linq.SqlClient;

namespace RWP.Data.Filters
{
  /// <summary>
  /// Filter Patient by its property values
  /// </summary>
  public class PatientNameFilter : FilterBase<Patient>
  {
    #region Properties

    /// <summary>
    /// Patient Name pattern
    /// </summary>
    public string Name { get; set; }

    #endregion Properties

    protected override IQueryable<Patient> DoApply(IQueryable<Patient> source)
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