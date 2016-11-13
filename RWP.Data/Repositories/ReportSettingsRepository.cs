using System.Linq;

namespace RWP.Data.Repositories
{
  /// <summary>
  /// Repositories of ReportSettings table entities
  /// </summary>
  public class ReportSettingsRepository : RepositoryBase<ReportSetting>
  {
    public ReportSetting GetByUQ(string name, string type, int researchId)
    {
      using (var context = CreateDataContext())
      {
        var result = context.ReportSettings.FirstOrDefault(r => r.Name == name && r.Type == type && r.IdMedicalResearch == researchId);
        return result;
      }
    }
  }
}