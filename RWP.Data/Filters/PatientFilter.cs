using System;
using System.Linq;
using System.Data.Linq.SqlClient;

namespace RWP.Data.Filters
{
  /// <summary>
  /// Filter Patient by its property values
  /// </summary>
  public class PatientFilter : FilterBase<Patient>
  {
    private readonly char[] DELIMS = new char[] { '.', '-', ' ' };
    private const char LESS = '<';
    private const char GREATER = '>';

    #region Properties

    /// <summary>
    /// Patient ID
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Patient Name pattern
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Patient DOB pattern
    /// </summary>
    public string DOB { get; set; }

    /// <summary>
    /// Patient date of Examination pattern
    /// </summary>
    public string ExaminationDate { get; set; }

    /// <summary>
    /// Patient date of Research pattern
    /// </summary>
    public string ResearchDate { get; set; }

    /// <summary>
    /// Patient Conclusion pattern
    /// </summary>
    public string Conclusion { get; set; }

    #endregion Properties

    protected override IQueryable<Patient> DoApply(IQueryable<Patient> source)
    {
      var result = source;

      // ID
      if (Id.HasValue)
        result = result.Where(p => p.Id == Id.Value);

      // Name
      if (!string.IsNullOrWhiteSpace(Name))
      {
        result = result.Where(p => SqlMethods.Like(((p.FirstName ?? string.Empty) + (p.MiddleName ?? string.Empty) + (p.LastName ?? string.Empty)),
                                                    LIKE + Name + LIKE));
      }

      // Date of Birth
      if (!string.IsNullOrWhiteSpace(DOB))
      {
        var dob = DOB.Trim();
        var first = dob[0];
        if (first == LESS)
        {
          var date = ParseDate(dob.Substring(1));
          if (date.HasValue)
            result = result.Where(p => p.DOB <= date.Value);
        }
        else if (first == GREATER)
        {
          var date = ParseDate(dob.Substring(1));
          if (date.HasValue)
            result = result.Where(p => p.DOB >= date.Value);
        }
        else
        {
          var date = ParseDate(dob);
          if (date.HasValue)
            result = result.Where(p => p.DOB.Year == date.Value.Year && p.DOB.Month == date.Value.Month && p.DOB.Day == date.Value.Day);
        }
      }

      // Examination Date
      if (!string.IsNullOrWhiteSpace(ExaminationDate))
      {
        var exd = ExaminationDate.Trim();
        var first = exd[0];
        if (first == LESS)
        {
          var date = ParseDate(exd.Substring(1));
          if (date.HasValue)
            result = result.Where(p => p.MedicalResearches.Any(r => r.ExaminationDate <= date.Value));
        }
        else if (first == GREATER)
        {
          var date = ParseDate(exd.Substring(1));
          if (date.HasValue)
            result = result.Where(p => p.MedicalResearches.Any(r => r.ExaminationDate >= date.Value));
        }
        else
        {
          var date = ParseDate(exd);
          if (date.HasValue)
            result = result.Where(p => p.MedicalResearches.Any(r => r.ExaminationDate.Year == date.Value.Year && r.ExaminationDate.Month == date.Value.Month && r.ExaminationDate.Day == date.Value.Day));
        }
      }

      // ResearchDate Date
      if (!string.IsNullOrWhiteSpace(ResearchDate))
      {
        var resd = ResearchDate.Trim();
        var first = resd[0];
        if (first == LESS)
        {
          var date = ParseDate(resd.Substring(1));
          if (date.HasValue)
            result = result.Where(p => p.MedicalResearches.Any(r => r.ResearchDate <= date.Value));
        }
        else if (first == GREATER)
        {
          var date = ParseDate(resd.Substring(1));
          if (date.HasValue)
            result = result.Where(p => p.MedicalResearches.Any(r => r.ResearchDate >= date.Value));
        }
        else
        {
          var date = ParseDate(resd);
          if (date.HasValue)
            result = result.Where(p => p.MedicalResearches.Any(r => r.ResearchDate.Year == date.Value.Year && r.ResearchDate.Month == date.Value.Month && r.ResearchDate.Day == date.Value.Day));
        }
      }

      // Conclusion
      if (!string.IsNullOrWhiteSpace(Conclusion))
      {
        result = result.Where(p => p.MedicalResearches.Any(r => r.Conclusion.Contains(Conclusion)));
      }

      return result;
    }

    private DateTime? ParseDate(string date)
    {
      var split = date.Split(DELIMS, StringSplitOptions.RemoveEmptyEntries);
      if (split.Length != 3)
        return null;

      int day;
      if (!int.TryParse(split[0].Trim(), out day))
        return null;

      int month;
      if (!int.TryParse(split[1].Trim(), out month))
        return null;

      int year;
      if (!int.TryParse(split[2].Trim(), out year))
        return null;

      try
      {
        return new DateTime(year, month, day);
      }
      catch
      {
        return null;
      }
    }
  }
}