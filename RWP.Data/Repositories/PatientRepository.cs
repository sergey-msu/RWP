using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using RWP.Data.Filters;

namespace RWP.Data.Repositories
{
  /// <summary>
  /// Repositories of Patient table entities
  /// </summary>
  public class PatientRepository : RepositoryBase<Patient>
  {
    public int GetPatientsCount(PatientFilter filter)
    {
      using (var context = CreateDataContext())
      {
        var result = (filter != null) ? filter.Apply(context.Patients, false) : context.Patients;
        return result.Count();
      }
    }

    public List<Patient> GetPatientsWithResearches(PatientFilter filter)
    {
      using (var context = CreateDataContext())
      {
        var options = new DataLoadOptions();
        options.LoadWith<Patient>(p => p.MedicalResearches);
        options.LoadWith<MedicalResearch>(r => r.MedicalResearchScopes);
        options.LoadWith<MedicalResearchScope>(rs => rs.ResearchScope);
        context.LoadOptions = options;

        var result = (filter != null) ? filter.Apply(context.Patients) : context.Patients;

        return result.ToList();
      }
    }

    public List<Patient> GetPatientsWithResearches(DateTime startDate, DateTime endDate)
    {
      startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
      endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day).AddDays(1);

      using (var context = CreateDataContext())
      {
        var options = new DataLoadOptions();
        options.LoadWith<Patient>(p => p.MedicalResearches);
        options.LoadWith<MedicalResearch>(r => r.MedicalResearchScopes);
        options.LoadWith<MedicalResearchScope>(rs => rs.ResearchScope);
        context.LoadOptions = options;

        return context.Patients
                      .Where(p => p.MedicalResearches.Any(r => startDate <= r.ExaminationDate && r.ExaminationDate < endDate))
                      .ToList();
      }
    }
  }
}