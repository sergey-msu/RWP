using System.Data.Linq;
using System.Linq;

namespace RWP.Data.Repositories
{
  /// <summary>
  /// Repositories of MedicalReserach table entities
  /// </summary>
  public class MedicalResearchRepository : RepositoryBase<MedicalResearch>
  {
    public MedicalResearch GetFullResearch(int researchId)
    {
      using (var context = CreateDataContext())
      {
        var options = new DataLoadOptions();
        options.LoadWith<MedicalResearchScope>(rs => rs.ResearchScope);
        options.LoadWith<MedicalResearch>(r => r.MedicalResearchScopes);
        options.LoadWith<MedicalScanRegime>(sr => sr.ScanRegime);
        options.LoadWith<MedicalResearch>(r => r.MedicalScanRegimes);
        options.LoadWith<MedicalResearch>(r => r.Doctor);
        options.LoadWith<MedicalResearch>(r => r.Customer);
        options.LoadWith<MedicalResearch>(r => r.Patient);
        options.LoadWith<MedicalResearch>(r => r.ResearchTemplate);
        options.LoadWith<MedicalResearch>(r => r.MedicalResearchAttachments);
        context.LoadOptions = options;

        return context.MedicalResearches.FirstOrDefault(r => r.Id == researchId);
      }
    }

    public bool HasResearchesWithDoctor(int doctorId)
    {
      using (var context = CreateDataContext())
      {
        return context.MedicalResearches.Any(r => r.IdDoctor == doctorId);
      }
    }

    public bool HasResearchesWithCustomer(int customerId)
    {
      using (var context = CreateDataContext())
      {
        return context.MedicalResearches.Any(r => r.IdCustomer == customerId);
      }
    }

    public bool HasResearchesWithPatient(int patientId)
    {
      using (var context = CreateDataContext())
      {
        return context.MedicalResearches.Any(r => r.IdPatient == patientId);
      }
    }

    public bool HasResearchesWithScope(int scopeId)
    {
      using (var context = CreateDataContext())
      {
        return context.MedicalResearches.Any(r => r.MedicalResearchScopes.Any(rs => rs.IdResearchScope == scopeId));
      }
    }
  }
}