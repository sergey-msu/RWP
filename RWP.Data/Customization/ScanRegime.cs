using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class ScanRegime : IEntityWithId, IUpdatable<ScanRegime>
  {
    void IUpdatable.UpdateFrom(object other, System.Data.Linq.DataContext dataContext)
    {
      UpdateFrom((ScanRegime)other, dataContext);
    }

    public void UpdateFrom(ScanRegime other, System.Data.Linq.DataContext dataContext)
    {
      this.Name = other.Name;
    }
  }
}
