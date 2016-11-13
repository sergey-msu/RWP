using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class ReportSetting : IEntityWithId, IUpdatable<ReportSetting>
  {
    void IUpdatable.UpdateFrom(object other, System.Data.Linq.DataContext dataContext)
    {
      UpdateFrom((ReportSetting)other, dataContext);
    }

    public void UpdateFrom(ReportSetting other, System.Data.Linq.DataContext dataContext)
    {
      this.IdMedicalResearch = other.IdMedicalResearch;
      this.Name = other.Name;
      this.Type = other.Type;
      this.Settings = other.Settings;
    }
  }
}
